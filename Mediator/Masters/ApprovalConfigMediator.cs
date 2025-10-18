using Domain.Dtos.Masters;
using Domain.Entities.Masters;
using Domain.Enums;
using Domain.ViewModels.Masters;
using DomainShared.Models;
using DomainShared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Models;
using System.Linq.Expressions;
using Persistence.Extensions;
using FluentValidation.Results;

namespace Mediator.Masters.ApprovalConfigMediator;

#region "Get List Approval Config"
#region "Query"
public sealed record GetApprovalConfigsQuery(Expression<Func<ApprovalConfig, bool>>[] wheres) : IRequest<IEnumerable<ApprovalConfigListItem>>;
#endregion
#region "Handler"
public sealed class GetApprovalConfigsQueryHandler : IRequestHandler<GetApprovalConfigsQuery, IEnumerable<ApprovalConfigListItem>>
{
    private readonly IDataContext _context;

    public GetApprovalConfigsQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApprovalConfigListItem>> Handle(GetApprovalConfigsQuery request, CancellationToken cancellationToken)
    {
        var queries = _context.ApprovalConfigs.AsQueryable().Where(x => x.DeletedAt == null);

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var approvalConfigs = await queries.ToListAsync();

        var viewModel = approvalConfigs.Select(x => x.ConvertToListItem()).ToList();

        return viewModel;
    }
}
#endregion
#endregion

#region "Get List Approval Config With Pagination"
#region "Query"
public sealed record GetApprovalConfigsPaginationQuery(PaginationConfig pagination, Expression<Func<ApprovalConfig, bool>>[] wheres) : IRequest<PaginatedList<ApprovalConfigListItem>>;
#endregion
#region "Handler"
public sealed class GetApprovalConfigsPaginationQueryHandler : IRequestHandler<GetApprovalConfigsPaginationQuery, PaginatedList<ApprovalConfigListItem>>
{
    private readonly IDataContext _context;

    public GetApprovalConfigsPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ApprovalConfigListItem>> Handle(GetApprovalConfigsPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = _context.ApprovalConfigs.AsQueryable().Where(x => x.DeletedAt == null);

        string search = request.pagination.Find;
        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(b => EF.Functions.Like(b.Name, $"%{search}%") || EF.Functions.Like(b.Description, $"%{search}%"));
        }

        var result = await queries.Select(x => new ApprovalConfigListItem
        {
            Key = x.Key,
            Name = x.Name,
            Description = x.Description
        }).PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);

        return await Task.FromResult(result);
    }
}
#endregion
#endregion

#region "Get By Id Approval Config"
#region "Query"
public sealed record GetApprovalConfigQuery(Guid Key) : IRequest<ApprovalConfigForm>;
#endregion
#region "Handler"
public sealed class GetApprovalConfigQueryHandler : IRequestHandler<GetApprovalConfigQuery, ApprovalConfigForm>
{
    private readonly IDataContext _context;

    public GetApprovalConfigQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<ApprovalConfigForm> Handle(GetApprovalConfigQuery request, CancellationToken cancellationToken)
    {
        var approvalConfig = await (from apc in _context.ApprovalConfigs
                                    where apc.Key == request.Key
                                    select new ApprovalConfig
                                    {
                                        Key = apc.Key,
                                        Name = apc.Name,
                                        Description = apc.Description
                                    }).FirstOrDefaultAsync();

        if (approvalConfig == null)
            throw new Exception("Approval Config Not Found");

        var directSpv = new Approver
        {
            Key = Guid.Empty,
            ApprovalConfigKey = approvalConfig.Key,
            EmployeeKey = Guid.Empty,
            Email = String.Empty,
            Position = String.Empty,
            ApproverCategory = ApproverCategory.DirectSupervisor,
            Level = 1,
            Action = "Approver"
        };

        var approvers = await (from apr in _context.Approvers
                               join e in _context.Employees on apr.EmployeeKey equals e.Key
                               where apr.ApprovalConfigKey == approvalConfig.Key && apr.DeletedAt == null
                               select new Approver
                               {
                                   Key = apr.Key,
                                   ApprovalConfigKey = apr.ApprovalConfigKey,
                                   EmployeeKey = apr.EmployeeKey,
                                   Email = apr.Email,
                                   Position = apr.Position,
                                   ApproverCategory = apr.ApproverCategory,
                                   Level = apr.Level,
                                   Action = apr.Action,
                                   ApprovalConfig = approvalConfig,
                                   User = e
                               }).ToListAsync();

        // Add directSpv to approvers and sort approvers by Level in ascending order
        approvers.Add(directSpv);
        approvers = approvers.OrderBy(x => x.Level).ToList();

        var approversForm = approvers.Select(x => x.ConvertToApproverListItem());
        approvalConfig.Approvers = approvers;

        var viewModel = approvalConfig.ConvertToForm();

        return viewModel;
    }
}
#endregion
#endregion

#region "Save Approval Config"
#region "Command"
public sealed record SaveApprovalConfigCommand(ApprovalConfigDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveApprovalConfigCommandHandler : IRequestHandler<SaveApprovalConfigCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<ApprovalConfigDto> _validator;
    private readonly IValidator<ApproverDto> _approverValidator;

    public SaveApprovalConfigCommandHandler(IDataContext context, IValidator<ApprovalConfigDto> validator, IValidator<ApproverDto> approverValidator)
    {
        _context = context;
        _validator = validator;
        _approverValidator = approverValidator;
    }

    public async Task<Result> Handle(SaveApprovalConfigCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //Validate Approval Config
            ValidationResult validator = await _validator.ValidateAsync(command.Form);
            if (!validator.IsValid)
            {
                var failures = validator.Errors
                                        .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                        .ToList();
                return Result.Failure(failures);
            }

            var approvalConfig = command.Form.ConvertToEntity();

            if (approvalConfig.Key == Guid.Empty)
            {
                approvalConfig.Key = Guid.NewGuid();
            }

            //Check if approval config is existing
            var existingApprovalConfig = await _context.ApprovalConfigs.FirstOrDefaultAsync(x => x.Key == approvalConfig.Key, cancellationToken);
            if (existingApprovalConfig == null)
            {
                //Add new Approval Config
                _context.ApprovalConfigs.Add(approvalConfig);
            }
            else
            {
                //Update existing Approval Config
                approvalConfig.CreatedAt = existingApprovalConfig.CreatedAt;
                approvalConfig.CreatedBy = existingApprovalConfig.CreatedBy;
                _context.ApprovalConfigs.Entry(existingApprovalConfig).CurrentValues.SetValues(approvalConfig);
            }

            if (command.Form.Approvers != null && command.Form.Approvers.Any())
            {
                foreach (var approverDto in command.Form.Approvers)
                {
                    approverDto.ApprovalConfigKey = approvalConfig.Key;
                    ValidationResult approverValidator = await _approverValidator.ValidateAsync(approverDto);
                    if (!approverValidator.IsValid)
                        return Result.Failure(approverValidator.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}").ToList());
                }

                _ = await _context.Approvers
                                  .Where(x => x.ApprovalConfigKey == approvalConfig.Key)
                                  .ExecuteDeleteAsync();

                var approversEntities = command.Form.Approvers.Select(x =>
                {
                    var entity = x.ConvertToEntity();
                    entity.Key = Guid.NewGuid();
                    entity.ApprovalConfigKey = approvalConfig.Key;
                    return entity;
                });

                await _context.Approvers.AddRangeAsync(approversEntities, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(new[] { $"Error saving approval config: {ex.Message}" });
        }

        return Result.Success();
    }
}
#endregion
#endregion

#region "Delete Approval Config"
#region "Command"
public sealed record DeleteApprovalConfigCommand(Guid Key) : IRequest<Result<ApprovalConfig>>;
#endregion
#region "Handler"
public sealed class DeleteApprovalConfigCommandHandler : IRequestHandler<DeleteApprovalConfigCommand, Result<ApprovalConfig>>
{
    private readonly IDataContext _context;

    public DeleteApprovalConfigCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<Result<ApprovalConfig>> Handle(DeleteApprovalConfigCommand command, CancellationToken cancellationToken)
    {
        var approvalConfig = await _context.ApprovalConfigs.FirstOrDefaultAsync(x => x.Key == command.Key);

        try
        {
            if (approvalConfig == null)
                throw new Exception("Approval Config Not Found");

            var approvers = await _context.Approvers.Where(x => x.ApprovalConfigKey == approvalConfig.Key).ToListAsync();
            if (approvers.Any())
            {
                _context.Approvers.RemoveRange(approvers);
            }

            _context.ApprovalConfigs.Remove(approvalConfig);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<ApprovalConfig>.Failure(new[] { ex.Message });
        }

        return Result<ApprovalConfig>.Success(approvalConfig);
    }
}
#endregion
#endregion

#region "Get List Approval User"
#region "Query"
public sealed record GetApprovalUsersQuery(PaginationConfig pagination, Expression<Func<Employee, bool>>[] wheres, ApproverCategory category) : IRequest<PaginatedList<ApproverUserList>>;
#endregion
#region "Handler"
public sealed class GetApprovalUsersQueryHandler(IDataContext _context) : IRequestHandler<GetApprovalUsersQuery, PaginatedList<ApproverUserList>>
{
    public async Task<PaginatedList<ApproverUserList>> Handle(GetApprovalUsersQuery request, CancellationToken cancellationToken)
    {
        var queries = from emp in _context.Employees
                      join pos in _context.Positions on emp.PositionKey equals pos.Key
                      where emp.DeletedAt == null
                      select new
                      {
                          Employee = emp,
                          Position = pos
                      };

        string search = request.pagination.Find;
        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(b => EF.Functions.Like(b.Employee.Name, $"%{search}%") ||
                                         EF.Functions.Like(b.Employee.Code, $"%{search}%") ||
                                         EF.Functions.Like(b.Position.Name, $"%{search}%"));
        }

        foreach (var where in request.wheres)
        {
            queries = queries.Where(x => where.Compile().Invoke(x.Employee));
        }

        if (request.category == ApproverCategory.DirectSupervisor)
            queries = queries.Where(x => x.Employee.DirectSupervisorKey == Guid.Empty || x.Employee.DirectSupervisorKey == null);
        else if (request.category == ApproverCategory.Employee)
            queries = queries.Where(x => x.Employee.DirectSupervisorKey != Guid.Empty);

        var users = await queries.Select(x => new ApproverUserList
        {
            Key = x.Employee.Key,
            NIK = x.Employee.Code,
            Name = x.Employee.Name,
            Position = x.Position.Name
        }).PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);

        return await Task.FromResult(users);
    }
}
#endregion
#endregion

#region "Get Approver Detail"
#region "Query"
public sealed record GetApproverDetailsQuery(Guid employeeKey, ApproverCategory category, int index) : IRequest<ApproverListItem>;
#endregion
#region "Handler"
public sealed class GetApproverDetailQueryHandler : IRequestHandler<GetApproverDetailsQuery, ApproverListItem>
{
    private readonly IDataContext _context;

    public GetApproverDetailQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<ApproverListItem> Handle(GetApproverDetailsQuery request, CancellationToken cancellationToken)
    {
        var employee = await (from emp in _context.Employees
                              join psnl in _context.EmployeePersonals on emp.Key equals psnl.EmployeeKey
                              join pos in _context.Positions on emp.PositionKey equals pos.Key
                              where emp.Key == request.employeeKey
                              select new
                              {
                                  Employee = emp,
                                  EmployeePersonal = psnl,
                                  Position = pos
                              }).FirstOrDefaultAsync();

        if (employee == null)
            throw new Exception("Employee not found");

        var approver = new ApproverListItem
        {
            EmployeeKey = employee.Employee.Key,
            Email = employee.EmployeePersonal.Email,
            Position = employee.Position.Name,
            Level = request.category == ApproverCategory.DirectSupervisor ? 1 : request.index,
            Action = request.category == ApproverCategory.DirectSupervisor ? "Approver" : "Verifier",
            User = employee.Employee,
            ApproverCategory = request.category,
            CanDelete = request.category != ApproverCategory.DirectSupervisor
        };

        return approver;
    }
}
#endregion
#endregion
