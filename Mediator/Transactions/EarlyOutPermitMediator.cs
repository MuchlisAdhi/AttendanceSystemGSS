using Domain.Dtos.Transactions;
using Domain.Enums;
using Domain.ViewModels.Transactions;
using DomainShared.Enums;
using DomainShared.Models;
using DomainShared;
using FluentValidation;
using Mediator.Masters.TitleMediator;
using Mediator.Transactions.ApprovalTransactionMediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Transactions;
using FluentValidation.Results;

namespace Mediator.Transactions.EarlyOutMediator;

#region "Get List Early Out Permit"
#region "Query"
public sealed record GetEarlyOutsQuery(Expression<Func<EarlyOut, bool>>[] wheres) : IRequest<IEnumerable<EarlyOut>>;
#endregion
#region "Handler"
public sealed class GetEarlyOutsQueryHandler : IRequestHandler<GetEarlyOutsQuery, IEnumerable<EarlyOut>>
{
    private readonly IDataContext _context;

    public GetEarlyOutsQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EarlyOut>> Handle(GetEarlyOutsQuery request, CancellationToken cancellationToken)
    {
        var queries = from eop in _context.EarlyOuts
                      join emp in _context.Employees on eop.EmployeeKey equals emp.Key
                      join apt in _context.ApprovalTransactions on eop.ApprovalTransactionKey equals apt.Key into approvalTransactionGroup
                      from approvalTransaction in approvalTransactionGroup.DefaultIfEmpty()
                      where eop.DeletedAt == null && approvalTransaction.Category == ApprovalCategory.EarlyOutPermit
                      select new EarlyOut
                      {
                          Key = eop.Key,
                          EmployeeKey = eop.EmployeeKey,
                          DateSubmission = eop.DateSubmission,
                          TimeOut = eop.TimeOut,
                          Description = eop.Description,
                          ApprovalStatus = eop.ApprovalStatus,
                          Number = eop.Number,
                          Employee = emp
                      };

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(filter);
        }

        var EarlyOuts = await queries.ToListAsync();

        return EarlyOuts;
    }
}
#endregion
#endregion

#region "Get List Early Out Permit With Pagination"
#region "Query"
public sealed record GetEarlyOutsPaginationQuery(PaginationConfig pagination, Expression<Func<EarlyOut, bool>>[] wheres, Guid? CompanyKey) : IRequest<PaginatedList<EarlyOutPermitListItem>>;
#endregion
#region "Handler"
public sealed class GetEarlyOutsPaginationQueryHandler : IRequestHandler<GetEarlyOutsPaginationQuery, PaginatedList<EarlyOutPermitListItem>>
{
    private readonly IDataContext _context;

    public GetEarlyOutsPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<EarlyOutPermitListItem>> Handle(GetEarlyOutsPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = from eop in _context.EarlyOuts
                      join emp in _context.Employees on eop.EmployeeKey equals emp.Key
                      join apt in _context.ApprovalTransactions on eop.ApprovalTransactionKey equals apt.Key into approvalTransactionGroup
                      from approvalTransaction in approvalTransactionGroup.DefaultIfEmpty()
                      where eop.DeletedAt == null
                      select new
                      {
                          EarlyOut = eop,
                          Employee = emp,
                          ApprovalTransaction = approvalTransaction
                      };

        string search = request.pagination.Find;
        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(b => EF.Functions.Like(b.EarlyOut.Number, $"%{search}%") ||
                                         EF.Functions.Like(b.Employee.Name, $"%{search}%") ||
                                         EF.Functions.Like(b.Employee.Code, $"%{search}%") ||
                                         EF.Functions.Like(b.EarlyOut.DateSubmission.Year.ToString(), $"%{search}%") ||
                                         EF.Functions.Like(b.EarlyOut.DateSubmission.Month.ToString(), $"%{search}%") ||
                                         EF.Functions.Like(b.EarlyOut.DateSubmission.Day.ToString(), $"%{search}%") ||
                                         EF.Functions.Like(Enum.GetName(typeof(ApprovalStatus), b.EarlyOut.ApprovalStatus), $"%{search}%"));
        }

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x.EarlyOut));
        }

        // Add ordering based on pagination config
        queries = request.pagination.OrderBy switch
        {
            OrderBy.ASC when request.pagination.SortBy == "Number" =>
                queries.OrderBy(x => x.EarlyOut.Number),
            OrderBy.DESC when request.pagination.SortBy == "Number" =>
                queries.OrderByDescending(x => x.EarlyOut.Number),
            OrderBy.ASC when request.pagination.SortBy == "EmployeeName" =>
                queries.OrderBy(x => x.Employee.Name),
            OrderBy.DESC when request.pagination.SortBy == "EmployeeName" =>
                queries.OrderByDescending(x => x.Employee.Name),

            _ => queries.OrderByDescending(x => x.EarlyOut.Number)
        };

        var EarlyOuts = await queries.ToListAsync();

        var EarlyOutList = EarlyOuts.Select(x => new EarlyOutPermitListItem
        {
            Key = x.EarlyOut.Key,
            EmployeeKey = x.EarlyOut.EmployeeKey,
            DateSubmission = x.EarlyOut.DateSubmission,
            TimeOut = x.EarlyOut.TimeOut,
            Description = x.EarlyOut.Description,
            ApprovalStatus = x.EarlyOut.ApprovalStatus,
            StatusName = Enum.GetName(typeof(ApprovalStatus), x.EarlyOut.ApprovalStatus),
            Number = x.EarlyOut.Number,
            Employee = x.Employee,
        }).ToList();

        return new PaginatedList<EarlyOutPermitListItem>(EarlyOutList, EarlyOuts.Count(), request.pagination.PageNumber, request.pagination.PageSize);
    }
}
#endregion
#endregion

#region "Get Early Out Permit By Id"
#region "Query"
public sealed record GetEarlyOutQuery(Guid Key) : IRequest<EarlyOutPermitForm>;
#endregion
#region "Handler"
public sealed class GetEarlyOutQueryHandler : IRequestHandler<GetEarlyOutQuery, EarlyOutPermitForm>
{
    private readonly IDataContext _context;
    private readonly IMediator _mediator;

    public GetEarlyOutQueryHandler(IDataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }
    public async Task<EarlyOutPermitForm> Handle(GetEarlyOutQuery request, CancellationToken cancellationToken)
    {
        var earlyOut = await (from eop in _context.EarlyOuts
                                    join emp in _context.Employees on eop.EmployeeKey equals emp.Key
                                    where eop.Key == request.Key
                                    select new EarlyOut
                                    {
                                        Key = eop.Key,
                                        EmployeeKey = eop.EmployeeKey,
                                        Number = eop.Number,
                                        DateSubmission = eop.DateSubmission,
                                        TimeOut = eop.TimeOut,
                                        Description = eop.Description,
                                        ApprovalStatus = eop.ApprovalStatus,
                                        ApprovalTransactionKey = eop.ApprovalTransactionKey,
                                        Employee = emp
                                    }).FirstOrDefaultAsync();

        if (earlyOut == null)
            throw new Exception("Early Out Permit not found.");

        //var approvalStamps = await _mediator.Send(new GetApprovalTransactionQuery(earlyOut.ApprovalTransactionKey));


        var viewModel = earlyOut.ConvertToViewModelEarlyOutPermit();

        //viewModel.ApprovalStatuses = approvalStamps.ApprovalStamps.Select((detail, index) => new ApprovalStatusItemList
        //{
        //    No = index + 1,
        //    Approver = detail.Approver.FullName,
        //    Action = detail.Action,
        //    StatusName = detail.StatusName,
        //    Status = detail.Status,
        //    ApprovalDate = DateOnly.FromDateTime(detail.DateStamp ?? DateTime.MinValue),
        //    ApproverKey = detail.Approver?.Key,
        //    Level = detail.Level,
        //    Email = detail.Email
        //}).ToList();

        return viewModel;
    }
}
#endregion
#endregion

#region "Save Early Out Permit"
#region "Command"
public sealed record SaveEarlyOutCommand(EarlyOutDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveEarlyOutCommandHandler(IDataContext _context,
                                                     IMediator _mediator,
                                                     IValidator<EarlyOutDto> _validator) : IRequestHandler<SaveEarlyOutCommand, Result>
{
    public async Task<Result> Handle(SaveEarlyOutCommand command, CancellationToken cancellationToken)
    {
        try
        {
            //Check if approval statuses (from approval config or approval stamp) is exists and approval transaction key is empty (new early out permit)
            if (command.Form.ApprovalStatuses != null && command.Form.ApprovalStatuses.Any() && command.Form.ApprovalTransactionKey == Guid.Empty)
            {
                var approvalTransactionDto = new ApprovalTransactionDto
                {
                    Key = command.Form.ApprovalTransactionKey ?? Guid.Empty,
                    EmployeeKey = command.Form.EmployeeKey ?? Guid.Empty,
                    ApprovalTransactionDate = DateTime.Now,
                    Category = ApprovalCategory.EarlyOutPermit,
                    ApprovalStatus = ApprovalStatus.New,
                    RejectReason = String.Empty,
                    Description = command.Form.Description ?? String.Empty,
                    //ApprovalStamps = await _repository.CreateApprovalStamps(command.Form.ApprovalStatuses)
                };

                //Save approval transaction
                var approvalResult = await _mediator.Send(new SaveApprovalTransactionCommand(approvalTransactionDto), cancellationToken);
                if (!approvalResult.Succeeded)
                    return approvalResult;

                command.Form.ApprovalTransactionKey = approvalResult.Value.Key;
            }

            ValidationResult validator = await _validator.ValidateAsync(command.Form);
            if (!validator.IsValid)
            {
                var failures = validator.Errors
                .Select(x => $"{x.PropertyName}: {x.ErrorMessage}")
                                        .ToList();
                return Result.Failure(failures);
            }

            

            var earlyOut = command.Form.ConvertToEntity();

            if (earlyOut.Key == Guid.Empty)
            {
                earlyOut.Key = Guid.NewGuid();
            }

            //Check if Early Out Permit Exists
            var existingEarlyOut = await _context.EarlyOuts.FirstOrDefaultAsync(x => x.Key == earlyOut.Key && x.DeletedAt == null, cancellationToken);

            if (existingEarlyOut == null)
            {
                //Add new Early Out Permit
                _context.EarlyOuts.Add(earlyOut);
            }
            else
            {
                //Update existing Early Out Permit
                earlyOut.CreatedAt = existingEarlyOut.CreatedAt;
                earlyOut.CreatedBy = existingEarlyOut.CreatedBy;
                _context.EarlyOuts.Entry(existingEarlyOut).CurrentValues.SetValues(earlyOut);
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

#region "Delete Early Out Permit"
#region "Command"
public sealed record DeleteEarlyOutCommand(Guid Key) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class DeleteEarlyOutCommandHandler : IRequestHandler<DeleteEarlyOutCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IMediator _mediator;

    public DeleteEarlyOutCommandHandler(IDataContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteEarlyOutCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var EarlyOut = await _context.EarlyOuts.FirstOrDefaultAsync(x => x.Key == command.Key, cancellationToken);

            if (EarlyOut == null)
                throw new Exception("Early Out Permit not found.");

            //Delete approval transaction if exists
            if (EarlyOut.ApprovalTransactionKey != Guid.Empty)
            {
                var approvalResult = await _mediator.Send(new DeleteApprovalTransactionCommand(EarlyOut.ApprovalTransactionKey), cancellationToken);
                if (!approvalResult.Succeeded)
                    throw new Exception(approvalResult.Errors.FirstOrDefault());
            }

            _context.EarlyOuts.Remove(EarlyOut);
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
