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

namespace Mediator.Masters.GradeMediator;

#region "Get List Grade"
#region "Query"
public sealed record GetGradesQuery(Expression<Func<Grade, bool>>[] wheres) : IRequest<GradeList>;
#endregion
#region "Handler"
public sealed class GetGradesQueryHandler : IRequestHandler<GetGradesQuery, GradeList>
{
    private readonly IDataContext _context;

    public GetGradesQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<GradeList> Handle(GetGradesQuery request, CancellationToken cancellationToken)
    {
        var queries = from gr in _context.Grades
                      where gr.DeletedAt == null
                      select new Grade
                      {
                          Key = gr.Key,
                          Code = gr.Code,
                          Name = gr.Name,
                          Description = gr.Description
                      };

        foreach (var filter in request.wheres)
        {
            queries = queries.Where(x => filter.Compile().Invoke(x));
        }

        var grades = await queries.ToListAsync();
        var viewModel = new GradeList
        {
            Grades = grades
        };
        return viewModel;
    }
}
#endregion
#endregion

#region "Get List Grade With Pagination"
#region "Query"
public sealed record GetGradesPaginationQuery(Expression<Func<Grade, bool>>[] wheres, PaginationConfig pagination) : IRequest<PaginatedList<Grade>>;
#endregion
#region "Handler"
public sealed class GetGradesPaginationQueryHandler : IRequestHandler<GetGradesPaginationQuery, PaginatedList<Grade>>
{
    private readonly IDataContext _context;

    public GetGradesPaginationQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Grade>> Handle(GetGradesPaginationQuery request, CancellationToken cancellationToken)
    {
        var queries = from gr in _context.Grades
                      where gr.DeletedAt == null
                      select new Grade
                      {
                          Key = gr.Key,
                          Code = gr.Code,
                          Name = gr.Name,
                          Description = gr.Description,
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

        var grades = await queries.PaginatedListAsync(request.pagination.PageNumber, request.pagination.PageSize);

        return await Task.FromResult(grades);
    }
}
#endregion
#endregion

#region "Get By Id Grade"
#region "Query"
public sealed record GetGradeQuery(Guid Key) : IRequest<GradeForm>;
#endregion
#region "Handler"
public sealed class GetGradeQueryHandler : IRequestHandler<GetGradeQuery, GradeForm>
{
    private readonly IDataContext _context;

    public GetGradeQueryHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<GradeForm> Handle(GetGradeQuery request, CancellationToken cancellationToken)
    {
        var grade = await (from gr in _context.Grades
                           where gr.Key == request.Key
                           select new Grade
                           {
                               Key = gr.Key,
                               Code = gr.Code,
                               Name = gr.Name,
                               Description = gr.Description
                           }).FirstOrDefaultAsync();

        if (grade == null)
            throw new InvalidOperationException("Grade not found.");

        return grade.ConvertToViewModelForm();
    }
}
#endregion
#endregion

#region "Save Grade"
#region "Command"
public sealed record SaveGradeCommand(GradeDto Form) : IRequest<Result>;
#endregion
#region "Handler"
public sealed class SaveGradeCommandHandler : IRequestHandler<SaveGradeCommand, Result>
{
    private readonly IDataContext _context;
    private readonly IValidator<GradeDto> _validator;

    public SaveGradeCommandHandler(IDataContext context, IValidator<GradeDto> validator)
    {
        _context = context;
        _validator = validator;
    }
    public async Task<Result> Handle(SaveGradeCommand command, CancellationToken cancellationToken)
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

            var grade = command.Form.ConvertToEntity();
            if (grade.Key == Guid.Empty)
            {
                grade.Key = Guid.NewGuid();
            }

            //Check existing grade
            var existingGrade = await _context.Grades.FirstOrDefaultAsync(x => x.Key == grade.Key);
            if (existingGrade == null)
            {
                //Add Grade
                _context.Grades.Add(grade);
            }
            else
            {
                //Update existing grade
                grade.CreatedAt = existingGrade.CreatedAt;
                grade.CreatedBy = existingGrade.CreatedBy;
                _context.Grades.Entry(existingGrade).CurrentValues.SetValues(grade);
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

#region "Delete Grade"
#region "Command"
public sealed record DeleteGradeCommand(Guid Key) : IRequest<Result<Grade>>;
#endregion
#region "Handler"
public sealed class DeleteGradeCommandHandler : IRequestHandler<DeleteGradeCommand, Result<Grade>>
{
    private readonly IDataContext _context;

    public DeleteGradeCommandHandler(IDataContext context)
    {
        _context = context;
    }

    public async Task<Result<Grade>> Handle(DeleteGradeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var grade = await _context.Grades.FirstOrDefaultAsync(g => g.Key == command.Key);
            if (grade == null)
            {
                throw new InvalidOperationException("Grade not found.");
            }

            _context.Grades.Remove(grade);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return Result<Grade>.Success(grade);
        }
        catch (Exception ex)
        {
            return Result<Grade>.Failure(new[] { ex.Message });
        }
    }
}
#endregion
#endregion
