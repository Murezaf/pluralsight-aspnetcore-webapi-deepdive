using CourseLibrary.Application.Helpers;
using CourseLibrary.Application.ResourceParameters;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.Application.Application.Authors.Queries;

public class GetAuthorsQuery : IRequest<PagedList<ExpandoObject>>
{
    public AuthorRecourseParameters AuthorRecourseParameters { get; }

    public GetAuthorsQuery(AuthorRecourseParameters authorRecourseParameters)
    {
        AuthorRecourseParameters = authorRecourseParameters ?? throw new ArgumentNullException(nameof(authorRecourseParameters));
    }
}
