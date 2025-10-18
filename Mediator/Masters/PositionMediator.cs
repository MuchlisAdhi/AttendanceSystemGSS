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

namespace Mediator.Masters.PositionMediator;

#region "Get List Position"
#region "Query"
public sealed record GetPositionsQuery(Expression<Func<Position, bool>>[] wheres) : IRequest<PositionList>;
#endregion
#region "Handler"
public sealed class GetPositionsQueryHandler : IRequestHandler<GetPositionsQuery, PositionList>
{
    private readonly IDataContext _context;

    public GetPositionsQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PositionList> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
    {
        var queries = from pos in _context.Positions
                      where pos.DeletedAt == null
                      select new Position
                      {
                          Key = pos.Key,
                          Code = pos.Code,
                          Name = pos.Name,
                          Description = pos.Description
                      };

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var positions = await queries.ToListAsync();

        var viewModel = new PositionList
        {
            Positions = positions
        };

        return viewModel;
    }
}
#endregion
#endregion

#region "Get List Position With Pagination"
#region "Query"
public sealed record GetPositionsPaginationQuery(Expression<Func<Position, bool>>[] wheres, PaginationConfig pagination) : IRequest<PaginatedList<Position>>;
#endregion
#region "Handler"
public sealed class GetPositionsPaginationQueryHandler : IRequestHandler<GetPositionsPaginationQuery, PaginatedList<Position>>
{
    private readonly IDataContext _context;

    public GetPositionsPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Position>> Handle(GetPositionsPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = from pos in _context.Positions
                      where pos.DeletedAt == null
                      select new Position
                      {
                          Key = pos.Key,
                          Code = pos.Code,
                          Name = pos.Name,
                          Description = pos.Description,
                      };

        string search = request.pagination.Find;

        if (!string.IsNullOrEmpty(search))
        {
            queries = queries.Where(p => EF.Functions.Like(p.Code, $"%{search}%") || EF.Functions.Like(p.Name, $"%{search}%") || EF.Functions.Like(p.Description, $"%{search}%"));
        }

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var positions = await queries.PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);

        return await Task.FromResult(positions);
    }
}
#endregion
#endregion

#region "Get By Id Position"
#region "Query"
public sealed record GetPositionQuery(Guid Key) : IRequest<PositionForm>;
#endregion
#region "Handler"
public sealed class GetPositionQueryHandler : IRequestHandler<GetPositionQuery, PositionForm>
{
    private readonly IDataContext _context;

    public GetPositionQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PositionForm> Handle(GetPositionQuery request, CancellationToken cancellationToken)
    {
        var position = await (from pos in _context.Positions
                              select new Position
                              {
                                  Key = pos.Key,
                                  Code = pos.Code,
                                  Name = pos.Name,
                                  Description = pos.Description
                              }).FirstOrDefaultAsync(p => p.Key == request.Key);
        if (position == null)
            throw new InvalidOperationException("Position not found.");

        return position.ConvertToViewModelPositionForm();
    }
}
#endregion
#endregion

#region "Save Position"
#region "Command"
public sealed record SavePositionCommand(PositionDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SavePositionCommandHandler : IRequestHandler<SavePositionCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<PositionDto> _validator;

    public SavePositionCommandHandler(IDataContext context, IValidator<PositionDto> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Result> Handle(SavePositionCommand command, CancellationToken cancellationToken)
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

            var position = command.Form.ConvertToEntity();
            if (position.Key == Guid.Empty)
            {
                position.Key = Guid.NewGuid();
            }

            //Check existing position
            var existingPosition = await _context.Positions.FirstOrDefaultAsync(x => x.Key == position.Key);
            if (existingPosition == null)
            {
                //Add position
                _context.Positions.Add(position);
            }
            else
            {
                //Update existing position
                position.CreatedAt = existingPosition.CreatedAt;
                position.CreatedBy = existingPosition.CreatedBy;
                _context.Positions.Entry(existingPosition).CurrentValues.SetValues(position);
            }

            var result = await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(new[] { ex.Message });
        }
    }
}
#endregion
#endregion

#region "Delete Position"
#region "Command"
public sealed record DeletePositionCommand(Guid Key) : IRequest<Result<Position>>;
#endregion
#region "Handler"
public sealed class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, Result<Position>>
{
    private readonly IDataContext _context;

    public DeletePositionCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<Result<Position>> Handle(DeletePositionCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var position = await _context.Positions.FirstOrDefaultAsync(p => p.Key == command.Key);
            if (position == null)
                throw new Exception("Position not found");

            _context.Positions.Remove(position);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return Result<Position>.Success(position);
        }
        catch (Exception ex)
        {
            return Result<Position>.Failure(new[] { ex.Message });
        }
    }
}
#endregion
#endregion
