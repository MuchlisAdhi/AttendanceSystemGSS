using Domain.Abstracts;
using Domain.Entities.Masters;
using Domain.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Extensions;
using System.Reflection;
using System.Text.Json;

namespace Persistence.Context;

public class DataContext : DbContext, IDataContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public DataContext(
        DbContextOptions<DataContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
    ) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    private static ValueConverter OwnConvert<T>()
    {
        JsonSerializerOptions option = new()
        {
            PropertyNameCaseInsensitive = true
        };

        return new ValueConverter<T, string>(
            x => JsonSerializer.Serialize(x, option),
            x => JsonSerializer.Deserialize<T>(x, option)
            //v => JsonConvert.SerializeObject(v),
            //v => JsonConvert.DeserializeObject<T>(v)
        );
    }

    private static ValueConverter DateTimeConvert()
        => new ValueConverter<DateTime, DateTime>(
            x => x.ToUniversalTime(),
            x => x.ToLocalTime()
        );

    private static ValueConverter<TimeOnly, TimeSpan> TimeOnlyConverter()
        => new(
            timeOnly => timeOnly.ToTimeSpan(),
            timeSpan => TimeOnly.FromTimeSpan(timeSpan)
        );

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        IEnumerable<Type> softDeleteEntities = typeof(AuditTrail).Assembly.GetTypes()
                .Where(t => typeof(AuditTrail).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t != typeof(AuditTrail));

        foreach (var softDeleteEntity in softDeleteEntities)
        {
            builder.Entity(softDeleteEntity)
                    .HasQueryFilter(
                        QueryFilterExtension.GenerateQueryFilter(softDeleteEntity)
                    );
        }

        #region Masters
        builder.Entity<ApprovalStamp>().Property(x => x.DateStamp)
                                       .HasConversion(DateTimeConvert())
                                       .HasColumnType("datetime2")
                                       .HasDefaultValueSql("GETUTCDATE()");
        builder.Entity<ApprovalTransaction>().Property(x => x.ApprovalTransactionDate)
                                       .HasConversion(DateTimeConvert())
                                       .HasColumnType("datetime2")
                                       .HasDefaultValueSql("GETUTCDATE()");
        builder.Entity<Attendance>().Property(x => x.TimeIn)
                           .HasConversion(TimeOnlyConverter())
                           .HasColumnType("time");

        builder.Entity<Attendance>().Property(x => x.TimeOut)
                            .HasConversion(TimeOnlyConverter())
                            .HasColumnType("time");

        builder.Entity<LeaveSubmission>().Property(x => x.DateStart)
                                         .HasConversion(DateTimeConvert())
                                         .HasColumnType("datetime2")
                                         .HasDefaultValueSql("GETUTCDATE()");
        builder.Entity<LeaveSubmission>().Property(x => x.DateEnd)
                                         .HasConversion(DateTimeConvert())
                                         .HasColumnType("datetime2")
                                         .HasDefaultValueSql("GETUTCDATE()");
        #endregion

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableServiceProviderCaching();
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public DbSet<ApprovalConfig> ApprovalConfigs => Set<ApprovalConfig>();

    public DbSet<Approver> Approvers => Set<Approver>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<City> Cities => Set<City>();

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<EmployeePersonal> EmployeePersonals => Set<EmployeePersonal>();

    public DbSet<Grade> Grades => Set<Grade>();

    public DbSet<Leave> Leaves => Set<Leave>();

    public DbSet<Position> Positions => Set<Position>();

    public DbSet<Province> Provinces => Set<Province>();

    public DbSet<Title> Titles => Set<Title>();

    public DbSet<ApprovalStamp> ApprovalStamps => Set<ApprovalStamp>();

    public DbSet<ApprovalTransaction> ApprovalTransactions => Set<ApprovalTransaction>();

    public DbSet<Attendance> Attendances => Set<Attendance>();

    public DbSet<EarlyOut> EarlyOuts => Set<EarlyOut>();

    public DbSet<EmployeeAttendance> EmployeesAttendances => Set<EmployeeAttendance>();

    public DbSet<LatePermit> LatePermits => Set<LatePermit>();

    public DbSet<LeaveSubmission> LeaveSubmissions => Set<LeaveSubmission>();

    public DbSet<OutPermit> OutPermits => Set<OutPermit>();

    public DbSet<Overtime> Overtimes => Set<Overtime>();
}
