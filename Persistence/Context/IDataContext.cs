using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context;

public interface IDataContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    #region DataSet Masters
    DbSet<ApprovalConfig> ApprovalConfigs { get; }
    DbSet<Approver> Approvers { get; }
    DbSet<Country> Countries { get; }
    DbSet<City> Cities { get; }
    DbSet<Employee> Employees { get; }
    DbSet<EmployeePersonal> EmployeePersonals { get; }
    DbSet<Grade> Grades { get; }
    DbSet<Leave> Leaves { get; }
    DbSet<Position> Positions { get; }
    DbSet<Province> Provinces { get; }
    DbSet<Title> Titles { get; }
    #endregion

    #region DataSet Transactions
    DbSet<ApprovalStamp> ApprovalStamps { get; }
    DbSet<ApprovalTransaction> ApprovalTransactions { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<EarlyOut> EarlyOuts { get; }
    DbSet<EmployeeAttendance> EmployeesAttendances { get; }
    DbSet<LatePermit> LatePermits { get; }
    DbSet<LeaveSubmission> LeaveSubmissions { get; }
    DbSet<OutPermit> OutPermits { get; }
    DbSet<Overtime> Overtimes { get; }
    #endregion
}
