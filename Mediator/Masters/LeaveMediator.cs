using Domain.Dtos.Masters;
using Domain.Entities.Masters;
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

namespace Mediator.Masters.LeaveMediator;

#region "Get List Leave"
#region "Query"
public sealed record GetLeavesQuery(Expression<Func<Leave, bool>>[] wheres) : IRequest<IEnumerable<Leave>>;
#endregion
#region "Handler"
public sealed class GetLeavesQueryHandler : IRequestHandler<GetLeavesQuery, IEnumerable<Leave>>
{
    private readonly IDataContext _context;

    public GetLeavesQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Leave>> Handle(GetLeavesQuery request, CancellationToken cancellationToken)
    {
        var queries = from leave in _context.Leaves
                      where leave.DeletedAt == null
                      select new Leave
                      {
                          Key = leave.Key,
                          Code = leave.Code,
                          Name = leave.Name,
                          MaxDays = leave.MaxDays,
                          MinSubmission = leave.MinSubmission,
                          MaxSubmission = leave.MaxSubmission
                      };

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(filter);
        }

        var leaves = await queries.ToListAsync();

        return leaves;
    }
}
#endregion
#endregion

#region "Get List Leave With Pagination"
#region "Query"
public sealed record GetLeavesPaginationQuery(PaginationConfig pagination, Expression<Func<Leave, bool>>[] wheres) : IRequest<PaginatedList<LeaveListItem>>;
#endregion
#region "Handler"
public sealed class GetLeavesPaginationQueryHandler : IRequestHandler<GetLeavesPaginationQuery, PaginatedList<LeaveListItem>>
{
    private readonly IDataContext _context;

    public GetLeavesPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<LeaveListItem>> Handle(GetLeavesPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = from leave in _context.Leaves
                      where leave.DeletedAt == null
                      select new Leave
                      {
                          Key = leave.Key,
                          Code = leave.Code,
                          Name = leave.Name,
                          MaxDays = leave.MaxDays,
                          MinSubmission = leave.MinSubmission,
                          MaxSubmission = leave.MaxSubmission,
                          Description = leave.Description
                      };

        string search = request.pagination.Find;
        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(b => EF.Functions.Like(b.Code, $"%{search}%") || EF.Functions.Like(b.Name, $"%{search}%"));
        }

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var leaves = await queries.Select(x => new LeaveListItem
        {
            Key = x.Key,
            Code = x.Code,
            Name = x.Name,
            MaxDays = x.MaxDays,
            MinSubmission = x.MinSubmission,
            MaxSubmission = x.MaxSubmission,
            Description = x.Description
        }).PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);

        return await Task.FromResult(leaves);
    }
}
#endregion
#endregion

#region "Get By Id Leave"
#region "Query"
public sealed record GetLeaveQuery(Guid Key) : IRequest<LeaveForm>;
#endregion
#region "Handler"
public sealed class GetLeaveQueryHandler : IRequestHandler<GetLeaveQuery, LeaveForm>
{
    private readonly IDataContext _context;

    public GetLeaveQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<LeaveForm> Handle(GetLeaveQuery request, CancellationToken cancellationToken)
    {
        var leave = await (from lea in _context.Leaves
                           where lea.Key == request.Key
                           select new Leave
                           {
                               Key = lea.Key,
                               Code = lea.Code,
                               Name = lea.Name,
                               MaxDays = lea.MaxDays,
                               MinSubmission = lea.MinSubmission,
                               MaxSubmission = lea.MaxSubmission,
                               Description = lea.Description,
                           }).FirstOrDefaultAsync();

        if (leave == null)
            throw new Exception("Leave not found.");

        return leave.ConvertToViewModelLeave();
    }
}
#endregion
#endregion

#region "Save Leave"
#region "Command"
public sealed record SaveLeaveCommand(LeaveDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveLeaveCommandHandler : IRequestHandler<SaveLeaveCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<LeaveDto> _validator;

    public SaveLeaveCommandHandler(IDataContext context, IValidator<LeaveDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Result> Handle(SaveLeaveCommand command, CancellationToken cancellationToken)
    {
        try
        {
            ValidationResult validator = await _validator.ValidateAsync(command.Form);
            if (!validator.IsValid)
            {
                var failures = validator.Errors
                                        .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                        .ToList();
                return Result.Failure(failures);
            }

            var leave = command.Form.ConvertToEntity();
            if (leave.Key == Guid.Empty)
            {
                leave.Key = Guid.NewGuid();
            }

            //Check if leave exists
            var existingLeave = await _context.Leaves.FirstOrDefaultAsync(x => x.Key == leave.Key && x.DeletedAt == null, cancellationToken);
            if (existingLeave == null)
            {
                //Add new Leave
                _context.Leaves.Add(leave);
            }
            else
            {
                //Update existing Leave
                leave.CreatedAt = existingLeave.CreatedAt;
                leave.CreatedBy = existingLeave.CreatedBy;
                _context.Leaves.Entry(existingLeave).CurrentValues.SetValues(leave);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);
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

#region "Delete Leave"
#region "Command"
public sealed record DeleteLeaveCommand(Guid Key) : IRequest<Result<Leave>>;
#endregion
#region "Handler"
public sealed class DeleteLeaveCommandHandler : IRequestHandler<DeleteLeaveCommand, Result<Leave>>
{
    private readonly IDataContext _context;

    public DeleteLeaveCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<Result<Leave>> Handle(DeleteLeaveCommand command, CancellationToken cancellationToken)
    {
        var leave = await _context.Leaves.FirstOrDefaultAsync(x => x.Key == command.Key);

        try
        {
            if (leave == null)
                throw new Exception("Leave not found.");

            _context.Leaves.Remove(leave);
            var result = await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Leave>.Failure(new[] { ex.Message });
        }

        return Result<Leave>.Success(leave);
    }
}
#endregion
#endregion
