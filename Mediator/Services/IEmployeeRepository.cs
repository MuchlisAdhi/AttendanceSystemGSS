using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Domain.ViewModels.Masters;
using DomainShared.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using Persistence.Models;
using System.Linq.Expressions;

namespace Mediator.Services;

public interface IEmployeeRepository
{
    Task<EmployeeList> GetAllEmployees(Expression<Func<Employee, bool>>[] wheres);
    Task<PaginatedList<EmployeeItemPagination>> GetAllEmployeesWithPagination(Expression<Func<Employee, bool>>[] wheres, PaginationConfig pagination);
    Task<EmployeeForm> GetEmployee(Guid Key);
    Task<IEnumerable<DirectSupervisorList>> GetAllDirectSupervisors(Expression<Func<Employee, bool>>[] wheres);
    Task SaveEmployeeAsync(Employee employee, CancellationToken cancellationToken);
    Task SaveEmployeePersonalAsync(EmployeePersonal employeePersonal, CancellationToken cancellationToken);
    Task SaveEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken);
}

public class EmployeeRepository(IDataContext _context) : IEmployeeRepository
{
    public async Task<IEnumerable<DirectSupervisorList>> GetAllDirectSupervisors(Expression<Func<Employee, bool>>[] wheres)
    {
        var queries = _context.Employees.AsQueryable().Where(x => (x.DirectSupervisorKey == Guid.Empty || x.DirectSupervisorKey == null) && x.DeletedAt == null);
        foreach (var where in wheres)
        {
            queries = queries.Where(where);
        }
        var directSupervisors = await queries.Select(x => new DirectSupervisorList
        {
            Key = x.Key,
            Code = x.Code,
            Name = x.Name
        }).ToListAsync();
        return await Task.FromResult(directSupervisors);
    }

    public async Task<EmployeeList> GetAllEmployees(Expression<Func<Employee, bool>>[] wheres)
    {
        // Start with the base query with the DeletedAt condition
        var employeeQuery = _context.Employees.Where(emp => emp.DeletedAt == null);

        // Apply each filter to the employee query
        foreach (var filter in wheres)
        {
            employeeQuery = employeeQuery.Where(filter);
        }

        var queries = from emp in employeeQuery
                      join personal in _context.EmployeePersonals on emp.Key equals personal.EmployeeKey
                      join position in _context.Positions on emp.PositionKey equals position.Key
                      join title in _context.Titles on emp.TitleKey equals title.Key
                      join grade in _context.Grades on emp.GradeKey equals grade.Key
                      join dr in _context.Employees on emp.DirectSupervisorKey equals dr.Key into parentSupervisor
                      from dr in parentSupervisor.DefaultIfEmpty()
                      where emp.DeletedAt == null
                      select new
                      {
                          Employee = emp,
                          EmployeePersonal = personal,
                          Position = position,
                          Title = title,
                          Grade = grade,
                          DirectSupervisor = dr
                      };

        var employees = await queries.ToListAsync();
        var viewModel = new EmployeeList
        {
            Employees = employees.Select(emp => new Employee
            {
                Key = emp.Employee.Key,
                Code = emp.Employee.Code,
                Name = emp.Employee.Name,
                PositionKey = emp.Employee.PositionKey,
                TitleKey = emp.Employee.TitleKey,
                GradeKey = emp.Employee.GradeKey,
                HireDate = emp.Employee.HireDate,
                Status = emp.Employee.Status,
                DirectSupervisorKey = emp.Employee.DirectSupervisorKey ?? Guid.Empty,
                Position = emp.Position,
                Title = emp.Title,
                Grade = emp.Grade,
                DirectSupervisor = emp.DirectSupervisor ?? null,
                EmployeePersonal = emp.EmployeePersonal
            })
        };

        return await Task.FromResult(viewModel);
    }

    public async Task<PaginatedList<EmployeeItemPagination>> GetAllEmployeesWithPagination(Expression<Func<Employee, bool>>[] wheres, PaginationConfig pagination)
    {
        var queries = from emp in _context.Employees
                      join personal in _context.EmployeePersonals on emp.Key equals personal.EmployeeKey
                      join position in _context.Positions on emp.PositionKey equals position.Key
                      join title in _context.Titles on emp.TitleKey equals title.Key
                      join grade in _context.Grades on emp.GradeKey equals grade.Key
                      join dr in _context.Employees on emp.DirectSupervisorKey equals dr.Key into parentSupervisor
                      from dr in parentSupervisor.DefaultIfEmpty()
                      where emp.DeletedAt == null
                      select new
                      {
                          Employee = emp,
                          EmployeePersonal = personal,
                          Position = position,
                          Title = title,
                          Grade = grade,
                          DirectSupervisor = dr
                      };
        string search = pagination.Find;
        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(b => EF.Functions.Like(b.Employee.Code, $"%{search}%") || EF.Functions.Like(b.Employee.Name, $"%{search}%") || EF.Functions.Like(b.Position.Name, $"%{search}%"));
        }

        // Applying additional filters from 'wheres'
        foreach (var filter in wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x.Employee));
        }

        var employees = await queries.Select(x => new EmployeeItemPagination
        {
            Employee = x.Employee
        })
        .PaginatedListAsync(pagination.PageNumber, pagination.PageSize);

        employees.Items.ForEach(x => {
            x.Position = _context.Positions.FirstOrDefault(f => f.Key == x.Employee.PositionKey);
            x.Title = _context.Titles.FirstOrDefault(f => f.Key == x.Employee.TitleKey);
            x.Grade = _context.Grades.FirstOrDefault(f => f.Key == x.Employee.GradeKey);
            x.DirectSupervisor = _context.Employees.FirstOrDefault(f => f.Key == x.Employee.DirectSupervisorKey);
        });

        return await Task.FromResult(employees);
    }

    public async Task<EmployeeForm> GetEmployee(Guid Key)
    {
        var employee = await(from e in _context.Employees
                             join pos in _context.Positions on e.PositionKey equals pos.Key
                             join ti in _context.Titles on e.TitleKey equals ti.Key
                             join gr in _context.Grades on e.GradeKey equals gr.Key
                             join dr in _context.Employees on e.DirectSupervisorKey equals dr.Key into parentSupervisor
                             from dr in parentSupervisor.DefaultIfEmpty()
                             join p in _context.EmployeePersonals on e.Key equals p.EmployeeKey into employeePersonalGroup
                             from employeePersonal in employeePersonalGroup.DefaultIfEmpty()
                             join cou in _context.Countries on employeePersonal.CountryKey equals cou.Key into personalCountryGroup
                             from personalCountry in personalCountryGroup.DefaultIfEmpty()
                             join prov in _context.Provinces on employeePersonal.ProvinceKey equals prov.Key into personalProvinceGroup
                             from personalProvince in personalProvinceGroup.DefaultIfEmpty()
                             join cty in _context.Cities on employeePersonal.CityKey equals cty.Key into personalCityGroup
                             from personalCity in personalCityGroup.DefaultIfEmpty()
                             where e.Key == Key
                             select new
                             {
                                 Employee = e,
                                 Position = pos,
                                 Title = ti,
                                 Grade = gr,
                                 DirectSupervisor = dr,
                                 EmployeePersonal = employeePersonal,
                                 PersonalCountry = personalCountry,
                                 PersonalProvince = personalProvince,
                                 PersonalCity = personalCity
                             }).FirstOrDefaultAsync();

        if (employee == null)
        {
            throw new Exception("Employee Not Found");
        }

        var employeeForm = new EmployeeForm
        {
            Key = employee.Employee.Key,
            Code = employee.Employee.Code,
            Name = employee.Employee.Name,
            PositionKey = employee.Employee.PositionKey,
            TitleKey = employee.Employee.TitleKey,
            GradeKey = employee.Employee.GradeKey,
            HireDate = employee.Employee.HireDate,
            Status = employee.Employee.Status,
            DirectSupervisorKey = employee.Employee.DirectSupervisorKey,
            Position = employee.Position,
            Title = employee.Title,
            Grade = employee.Grade,
            DirectSupervisor = employee.DirectSupervisor
        };

        if (employee.EmployeePersonal != null)
        {
            employeeForm.EmployeePersonal = new EmployeePersonalForm
            {
                Key = employee.EmployeePersonal.Key,
                EmployeeKey = employee.EmployeePersonal.EmployeeKey,
                PlaceOfBirth = employee.EmployeePersonal.PlaceOfBirth,
                DateOfBirth = employee.EmployeePersonal.DateOfBirth,
                Gender = employee.EmployeePersonal.Gender,
                Religion = employee.EmployeePersonal.Religion,
                MaritalStatus = employee.EmployeePersonal.MaritalStatus,
                Address = employee.EmployeePersonal.Address,
                CountryKey = employee.EmployeePersonal.CountryKey,
                ProvinceKey = employee.EmployeePersonal.ProvinceKey,
                CityKey = employee.EmployeePersonal.CityKey,
                PostalCode = employee.EmployeePersonal.PostalCode,
                PhoneNumber = employee.EmployeePersonal.PhoneNumber,
                Email = employee.EmployeePersonal.Email,
                Employee = employee.Employee,
                Country = employee.PersonalCountry,
                Province = employee.PersonalProvince,
                City = employee.PersonalCity,
            };
        }

        // Add Employee Attendance Information
        var employeeAttendance = await(from ea in _context.EmployeesAttendances
                                       join apr in _context.ApprovalConfigs on ea.ApprovalConfigKey equals apr.Key into approvalConfigGroup
                                       from approvalConfig in approvalConfigGroup.DefaultIfEmpty()
                                       where ea.EmployeeKey == Key
                                       select new EmployeeAttendance
                                       {
                                           Key = ea.Key,
                                           EmployeeKey = ea.EmployeeKey,
                                           ApprovalConfigKey = ea.ApprovalConfigKey,
                                           ApprovalConfig = approvalConfig
                                       }).FirstOrDefaultAsync();

        var employeeAttendanceForm = employeeAttendance?.ConvertToEmployeeAttendanceForm();


        employeeForm.EmployeeAttendance = employeeAttendanceForm;


        return employeeForm;
    }

    public async Task SaveEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        var existingEmployee = await _context.Employees.FirstOrDefaultAsync(x => x.Key == employee.Key, cancellationToken);
        if (existingEmployee == null)
        {
            employee.Key = Guid.NewGuid();
            await _context.Employees.AddAsync(employee);
        }
        else
        {
            //update existing employee
            employee.CreatedAt = existingEmployee.CreatedAt;
            employee.CreatedBy = existingEmployee.CreatedBy;
            _context.Employees.Entry(existingEmployee).CurrentValues.SetValues(employee);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken)
    {
        var existingAttendance = await _context.EmployeesAttendances.FirstOrDefaultAsync(x => x.Key == attendance.Key, cancellationToken);

        if (existingAttendance == null)
        {
            attendance.Key = Guid.NewGuid();
            await _context.EmployeesAttendances.AddAsync(attendance, cancellationToken);
        }
        else
        {
            //update existing EmployeeAttendance
            attendance.CreatedAt = existingAttendance.CreatedAt;
            attendance.CreatedBy = existingAttendance.CreatedBy;
            _context.EmployeesAttendances.Entry(existingAttendance).CurrentValues.SetValues(attendance);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveEmployeePersonalAsync(EmployeePersonal employeePersonal, CancellationToken cancellationToken)
    {
        var existingEmployeePersonal = await _context.EmployeePersonals.FirstOrDefaultAsync(x => x.Key == employeePersonal.Key, cancellationToken);
        if (existingEmployeePersonal == null)
        {
            employeePersonal.Key = Guid.NewGuid();
            await _context.EmployeePersonals.AddAsync(employeePersonal);
        }
        else
        {
            //update existing employee personal
            employeePersonal.CreatedAt = existingEmployeePersonal.CreatedAt;
            employeePersonal.CreatedBy = existingEmployeePersonal.CreatedBy;
            _context.EmployeePersonals.Entry(existingEmployeePersonal).CurrentValues.SetValues(employeePersonal);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
