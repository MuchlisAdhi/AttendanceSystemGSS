using Domain.Dtos.Masters;
using Domain.Dtos.Transactions;
using Domain.Entities.Masters;
using Domain.ViewModels.Masters;
using DomainShared;
using DomainShared.Models;
using FluentValidation;
using FluentValidation.Results;
using Mediator.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Models;
using System.Linq.Expressions;

namespace Mediator.Masters.EmployeeMediator;

#region "Get List Employee"
#region "Query"
public sealed record GetEmployeesQuery(Expression<Func<Employee, bool>>[] wheres) : IRequest<EmployeeList>;
#endregion
#region "Handler"
public sealed class GetEmployeesQueryHandler(IEmployeeRepository _repo) : IRequestHandler<GetEmployeesQuery, EmployeeList>
{
    public async Task<EmployeeList> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        => await _repo.GetAllEmployees(request.wheres);
}
#endregion
#endregion

#region "Get List Employee With Pagination"
#region "Query"
public sealed record GetEmployeesPaginationQuery(Expression<Func<Employee, bool>>[] wheres, PaginationConfig pagination) : IRequest<PaginatedList<EmployeeItemPagination>>;
#endregion
#region "Handler"
public sealed class GetEmployeesPaginationQueryHandler(IEmployeeRepository _repo) : IRequestHandler<GetEmployeesPaginationQuery, PaginatedList<EmployeeItemPagination>>
{
    public async Task<PaginatedList<EmployeeItemPagination>> Handle(GetEmployeesPaginationQuery request, CancellationToken cancellationToken)
        => await _repo.GetAllEmployeesWithPagination(request.wheres, request.pagination);
}
#endregion
#endregion

#region "Get By Id Employee"
#region "Query"
public sealed record GetEmployeeQuery(Guid Key) : IRequest<EmployeeForm>;
#endregion
#region "Handler"
public sealed class GetEmployeeQueryHandler(IEmployeeRepository _repo) : IRequestHandler<GetEmployeeQuery, EmployeeForm>
{
    public async Task<EmployeeForm> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        => await _repo.GetEmployee(request.Key);
}
#endregion
#endregion

#region "Get List Direct Supervisor"
#region "Query"
public sealed record GetDirectSupervisorsQuery(Expression<Func<Employee, bool>>[] wheres) : IRequest<IEnumerable<DirectSupervisorList>>;
#endregion
#region "Handler"
public sealed class GetDirectSupervisorsQueryHandler(IEmployeeRepository _repo) : IRequestHandler<GetDirectSupervisorsQuery, IEnumerable<DirectSupervisorList>>
{
    public async Task<IEnumerable<DirectSupervisorList>> Handle(GetDirectSupervisorsQuery request, CancellationToken cancellationToken)
        => await _repo.GetAllDirectSupervisors(request.wheres);
}
#endregion
#endregion

#region "Save Employee"
#region "Command"
public sealed record SaveEmployeeCommand(EmployeeDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveEmployeeCommandHandler : IRequestHandler<SaveEmployeeCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<EmployeeDto> _EmployeeValidator;
    private readonly IValidator<EmployeePersonalDto> _EmployeePersonalValidator;
    private readonly IValidator<EmployeeAttendanceDto> _EmployeeAttendanceValidator;
    private readonly IMediator _mediator;
    private readonly IEmployeeRepository _employeeRepository;

    public SaveEmployeeCommandHandler(IDataContext context,
                                      IValidator<EmployeeDto> EmployeeValidator,
                                      IValidator<EmployeePersonalDto> EmployeePersonalValidator,
                                      IValidator<EmployeeAttendanceDto> EmployeeAttendanceValidator,
                                      IMediator mediator,
                                      IEmployeeRepository employeeRepository)
    {
        _context = context;
        _EmployeeValidator = EmployeeValidator;
        _EmployeePersonalValidator = EmployeePersonalValidator;
        _EmployeeAttendanceValidator = EmployeeAttendanceValidator;
        _mediator = mediator;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result> Handle(SaveEmployeeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //Validate Employee
            ValidationResult employeeValidator = await _EmployeeValidator.ValidateAsync(command.Form);
            if (!employeeValidator.IsValid)
            {
                var failures = employeeValidator.Errors
                                        .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                        .ToList();
                return Result.Failure(failures);
            }

            //Save Employee 
            var employee = command.Form.ConvertToEntity();
            await _employeeRepository.SaveEmployeeAsync(employee, cancellationToken);

            //Save Employee Personal
            if (command.Form.EmployeePersonal != null)
            {
                command.Form.EmployeePersonal.EmployeeKey = employee.Key;

                //Check for EmployeePersonal Key to decide RuleSet
                var personalRuleSet = command.Form.EmployeePersonal.Key == Guid.Empty || command.Form.EmployeePersonal.Key == null ? "Create" : "Update";

                //Validate Employee Personal
                ValidationResult employeePersonalValidator = await _EmployeePersonalValidator.ValidateAsync(command.Form.EmployeePersonal, options => options.IncludeRuleSets(personalRuleSet));
                if (!employeePersonalValidator.IsValid)
                {
                    var failures = employeePersonalValidator.Errors
                                            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                            .ToList();
                    return Result.Failure(failures);
                }

                var personalEntity = command.Form.EmployeePersonal.ConvertToEntity();
                await _employeeRepository.SaveEmployeePersonalAsync(personalEntity, cancellationToken);
            }

            //Create or Update Employee Attendance With Details
            if (command.Form.EmployeeAttendance != null)
            {
                command.Form.EmployeeAttendance.EmployeeKey = employee.Key;

                //Check for EmployeeAttendance Key to decide RuleSet
                var attendanceRuleSet = command.Form.EmployeeAttendance.Key == Guid.Empty || command.Form.EmployeeAttendance.Key == null ? "Create" : "Update";

                //Validate Employee Attendance
                ValidationResult employeeAttendanceValidator = await _EmployeeAttendanceValidator.ValidateAsync(command.Form.EmployeeAttendance, options => options.IncludeRuleSets(attendanceRuleSet));
                if (!employeeAttendanceValidator.IsValid)
                {
                    var failures = employeeAttendanceValidator.Errors
                                            .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                            .ToList();
                    return Result.Failure(failures);
                }

                var attendanceEntity = command.Form.EmployeeAttendance.ConvertToEntity();
                await _employeeRepository.SaveEmployeeAttendanceAsync(attendanceEntity, cancellationToken);
            }

        }
        catch (Exception ex)
        {
            return Result.Failure(new[] { ex.Message });
        }
        return Result.Success();
    }
}
#endregion
#endregion

#region "Delete Employee"
#region "Command"
public sealed record DeleteEmployeeCommand(Guid Key) : IRequest<Result<Employee>>;
#endregion
#region "Handler"
public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result<Employee>>
{
    private readonly IDataContext _context;
    private readonly IMediator _mediator;

    public DeleteEmployeeCommandHandler(IDataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result<Employee>> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Key == command.Key);

        try
        {
            if (employee == null)
            {
                throw new Exception("Employee Not Found");
            }

            // Check if EmployeePersonal exists for this Employee
            var employeePersonal = await _context.EmployeePersonals.FirstOrDefaultAsync(ep => ep.EmployeeKey == employee.Key, cancellationToken);
            if (employeePersonal != null)
            {
                _context.EmployeePersonals.Remove(employeePersonal);
            }

            //Check if any Employee Attendance and Details exist for this Employee
            var employeeAttendance = await _context.EmployeesAttendances.FirstOrDefaultAsync(ea => ea.EmployeeKey == employee.Key, cancellationToken);
            if (employeeAttendance != null)
            {
                _context.EmployeesAttendances.Remove(employeeAttendance);
            }

            _context.Employees.Remove(employee);
            var result = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Employee>.Failure(new[] { ex.Message });
        }
        return Result<Employee>.Success(employee);
    }
}
#endregion
#endregion
