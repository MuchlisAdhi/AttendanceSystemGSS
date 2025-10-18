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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Persistence.Extensions;
using FluentValidation.Results;

namespace Mediator.Masters.TitleMediator;

#region "Get List Title"
#region "Query"
public sealed record GetTitlesQuery(Expression<Func<Title, bool>>[] wheres) : IRequest<TitleList>;
#endregion
#region "Handler"
public sealed class GetTitlesQueryHandler : IRequestHandler<GetTitlesQuery, TitleList>
{
    private readonly IDataContext _context;

    public GetTitlesQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<TitleList> Handle(GetTitlesQuery request, CancellationToken cancellationToken)
    {
        var queries = from ti in _context.Titles
                      where ti.DeletedAt == null
                      select new Title
                      {
                          Key = ti.Key,
                          Code = ti.Code,
                          Name = ti.Name,
                          Description = ti.Description
                      };

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var titles = await queries.ToListAsync();
        var viewModel = new TitleList
        {
            Titles = titles
        };
        return viewModel;
    }
}
#endregion
#endregion

#region "Get List Title With Pagination"
#region "Query"
public sealed record GetTitlesPaginationQuery(Expression<Func<Title, bool>>[] wheres, PaginationConfig pagination) : IRequest<PaginatedList<Title>>;
#endregion
#region "Handler"
public sealed class GetTitlesPaginationQueryHandler : IRequestHandler<GetTitlesPaginationQuery, PaginatedList<Title>>
{
    private readonly IDataContext _context;

    public GetTitlesPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Title>> Handle(GetTitlesPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = from ti in _context.Titles
                      where ti.DeletedAt == null
                      select new Title
                      {
                          Key = ti.Key,
                          Code = ti.Code,
                          Name = ti.Name,
                          Description = ti.Description
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

        var titles = await queries.PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);
        return await Task.FromResult(titles);
    }
}
#endregion
#endregion

#region "Get By Id Title"
#region "Query"
public sealed record GetTitleQuery(Guid Key) : IRequest<TitleForm>;
#endregion
#region "Handler"
public sealed class GetTitleQueryHandler : IRequestHandler<GetTitleQuery, TitleForm>
{
    private readonly IDataContext _context;

    public GetTitleQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<TitleForm> Handle(GetTitleQuery request, CancellationToken cancellationToken)
    {
        var title = await (from ti in _context.Titles
                           select new Title
                           {
                               Key = ti.Key,
                               Code = ti.Code,
                               Name = ti.Name,
                               Description = ti.Description
                           }).FirstOrDefaultAsync(p => p.Key == request.Key);
        if (title == null)
            throw new InvalidOperationException("Title not found or has been deleted.");

        return title.ConvertToViewModelTitleForm();
    }
}
#endregion
#endregion

#region "Save Title"
#region "Command"
public sealed record SaveTitleCommand(TitleDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveTitleCommandHandler : IRequestHandler<SaveTitleCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<TitleDto> _validator;

    public SaveTitleCommandHandler(IDataContext context, IValidator<TitleDto> validator)
    {
        _context = context;
        _validator = validator;
    }
    public async Task<Result> Handle(SaveTitleCommand command, CancellationToken cancellationToken)
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

            var title = command.Form.ConverToEntity();
            if (title.Key == Guid.Empty)
            {
                title.Key = Guid.NewGuid();
            }

            //Check existing title
            var existingTitle = await _context.Titles.FirstOrDefaultAsync(x => x.Key == title.Key);
            if (existingTitle == null)
            {
                //Add Title
                _context.Titles.Add(title);
            }
            else
            {
                //Update existing Title
                title.CreatedAt = existingTitle.CreatedAt;
                title.CreatedBy = existingTitle.CreatedBy;
                _context.Titles.Entry(existingTitle).CurrentValues.SetValues(title);
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

#region "Delete Title"
#region "Command"
public sealed record DeleteTitleCommand(Guid Key) : IRequest<Result<Title>>;
#endregion
#region "Handler"
public sealed class DeleteTitleCommandHandler : IRequestHandler<DeleteTitleCommand, Result<Title>>
{
    private readonly IDataContext _context;

    public DeleteTitleCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<Result<Title>> Handle(DeleteTitleCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var title = await _context.Titles.FirstOrDefaultAsync(x => x.Key == command.Key);
            if (title == null)
                throw new Exception("Title not found");

            _context.Titles.Remove(title);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return Result<Title>.Success(title);
        }
        catch (Exception ex)
        {
            return Result<Title>.Failure(new[] { ex.Message });
        }
    }
}
#endregion
#endregion
