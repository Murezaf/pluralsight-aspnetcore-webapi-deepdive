using CourseLibrary.API.Helpers;
using CourseLibrary.API.ResourceParameters;
using MediatR;
using System.Dynamic;

namespace CourseLibrary.API.Application.Authors.Queries;

public class GetAuthorsQuery : IRequest<PagedList<ExpandoObject>>
{
    public AuthorRecourseParameters AuthorRecourseParameters { get; }

    public GetAuthorsQuery(AuthorRecourseParameters authorRecourseParameters)
    {
        AuthorRecourseParameters = authorRecourseParameters ?? throw new ArgumentNullException(nameof(authorRecourseParameters));
    }
}
