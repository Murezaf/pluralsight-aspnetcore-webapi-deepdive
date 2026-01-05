using AutoMapper;
using CourseLibrary.API.ActionConstraint;
using CourseLibrary.API.Application.Authors.Commands;
using CourseLibrary.API.Application.Authors.Queries;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories;
using CourseLibrary.API.Repositories.Interfaces;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using System.Dynamic;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    //private readonly ICourseLibraryRepository _courseLibraryRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IPropertyCheckerService _propertyCheckerService;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AuthorsController(
        //ICourseLibraryRepository courseLibraryRepository,
        IAuthorRepository authorRepository,
        IMediator mediator, IMapper mapper,
        IPropertyMappingService propertyMappingService, IPropertyCheckerService propertyCheckerService,
        ProblemDetailsFactory problemDetailsFactory)
    {
        //_courseLibraryRepository = courseLibraryRepository ??
        //    throw new ArgumentNullException(nameof(courseLibraryRepository));
        _authorRepository = authorRepository ??
            throw new ArgumentNullException(nameof(authorRepository));
        _mediator = mediator ??
            throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _propertyMappingService = propertyMappingService ??
            throw new ArgumentNullException(nameof(propertyMappingService));
        _propertyCheckerService = propertyCheckerService ??
            throw new ArgumentNullException(nameof(_propertyCheckerService));
        _problemDetailsFactory = problemDetailsFactory ??
            throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    [HttpGet(Name = "GetAuthors")]
    [HttpHead]
    //public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery(Name = "category")] string? mainCategory = "")
    //public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(string? mainCategory = "", string? searchQuery = "")
    //public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery] AuthorRecourseParameters authorRecourseParameters)
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorRecourseParameters authorRecourseParameters)
    {
        //throw new Exception("Exception Test for Fault handling");

        if (!_propertyMappingService.ValidMappingExist<AuthorDto, Author>(authorRecourseParameters.OrderBy))
            return BadRequest();

        if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(authorRecourseParameters.Fields))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                statusCode: 400,
                detail: $"Not all requested data shaping fields exist on the resource: {authorRecourseParameters.Fields}"
                ));
        }

        //var authorsFromRepo = await _courseLibraryRepository.GetAuthorsAsync(mainCategory, searchQuery); 
        //PagedList<Author> pagedAuthorsFromRepo = await _authorRepository.GetAuthorsAsync(authorRecourseParameters);
        PagedList<ExpandoObject> shapedPagedAuthors = await _mediator.Send(new GetAuthorsQuery(authorRecourseParameters));

        //string? previousPageLink = shapedPagedAuthors.HasPrevious ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.PreviousPage) : null;
        //string? nextPageLink = shapedPagedAuthors.HasNext ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.NextPage) : null;

        var paginationMetaData = new //Anonymous Object 
        {
            totalCount = shapedPagedAuthors.TotalCount,
            pageSize = shapedPagedAuthors.PageSize,
            pageNumber = shapedPagedAuthors.CurrentPage,
            totalPages = shapedPagedAuthors.TotalPages,
            //nextPageLink = nextPageLink,
            //previousPageLink = previousPageLink
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

        //return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));

        //IEnumerable<AuthorDto> authorsDtoToReturn = _mapper.Map<IEnumerable<AuthorDto>>(pagedAuthorsFromRepo);
        //IEnumerable<System.Dynamic.ExpandoObject> authorsDtoDataShapedToReturn = authorsDtoToReturn.ShapeData(authorRecourseParameters.Fields);
        //return Ok(authorsDtoDataShapedToReturn);
        //We have done these in the MediatR Handler

        IEnumerable<LinkDto> links = CreateLinksForAuthors(authorRecourseParameters,
            shapedPagedAuthors.HasNext, shapedPagedAuthors.HasPrevious);

        IEnumerable<IDictionary<string, object?>> shapedAuthorsWithLinks = shapedPagedAuthors.Select(author =>
        {
            var authorAsDictionary = author as IDictionary<string, object?>;
            var linksForEachAuthor = CreateLinksForAuthor((Guid)authorAsDictionary["Id"], null);
            authorAsDictionary.Add("links", linksForEachAuthor);

            return authorAsDictionary;
        });

        var wrepper = new
        {
            values = shapedAuthorsWithLinks,
            links = links
        };

        return Ok(wrepper);
    }

    private string? CreateAuthorsResourceUri(AuthorRecourseParameters authorRecourseParameters, ResorceUriType resorceUriType)
    {
        switch (resorceUriType)
        {
            case ResorceUriType.NextPage:
                return Url.Link("GetAuthors",
                   new
                   {
                       searchQuery = authorRecourseParameters.SearchQuery,
                       mainCategory = authorRecourseParameters.MainCategory,
                       pageNumber = authorRecourseParameters.PageNumber + 1,
                       pageSize = authorRecourseParameters.PageSize,

                       orderBy = authorRecourseParameters.OrderBy,

                       fields = authorRecourseParameters.Fields
                   });

            case ResorceUriType.PreviousPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorRecourseParameters.SearchQuery,
                        mainCategory = authorRecourseParameters.MainCategory,
                        pageNumber = authorRecourseParameters.PageNumber - 1,
                        pageSize = authorRecourseParameters.PageSize,

                        orderBy = authorRecourseParameters.OrderBy,

                        fields = authorRecourseParameters.Fields
                    });

            case ResorceUriType.Current:
            default:
                return Url.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorRecourseParameters.SearchQuery,
                        mainCategory = authorRecourseParameters.MainCategory,
                        pageNumber = authorRecourseParameters.PageNumber,
                        pageSize = authorRecourseParameters.PageSize,

                        orderBy = authorRecourseParameters.OrderBy,

                        fields = authorRecourseParameters.Fields
                    });
        }
    }

    [NonAction]
    public IEnumerable<LinkDto> CreateLinksForAuthors(AuthorRecourseParameters authorRecourseParameters,
        bool hasNext, bool hasPrevious)
    {
        List<LinkDto> links = new List<LinkDto>();

        links.Add(new LinkDto(CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.Current), "self", "GET"));

        if (hasNext)
            links.Add(new LinkDto(CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.NextPage), "nextPage", "GET"));

        if (hasPrevious)
            links.Add(new LinkDto(CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.PreviousPage), "previousPage", "GET"));

        return links;
    }

    [Produces(
      "application/json",
      "application/vnd.marvin.hateoas+json",
      "application/vnd.marvin.author.full+json",
      "application/vnd.marvin.author.full.hateoas+json",
      "application/vnd.marvin.author.friendly+json",
      "application/vnd.marvin.author.friendly.hateoas+json")]
    [HttpGet("{authorId}", Name = "GetAuthor")]
    //public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
    //public async Task<IActionResult> GetAuthor(Guid authorId, string? fields)
    public async Task<IActionResult> GetAuthor(Guid authorId, string? fields,
        [FromHeader(Name = "Accept")] string? mediaType)
    {
        if (!MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
        {
            return BadRequest(_problemDetailsFactory.CreateProblemDetails(HttpContext,
                statusCode: 400, detail: "Accept header media type is not a valid media type."));
        }

        bool includeLinks = parsedMediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

        IEnumerable<LinkDto> links = new List<LinkDto>();
        if (includeLinks)
        {
            links = CreateLinksForAuthor(authorId, fields);
        }

        var primaryDataType = includeLinks ? parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8) 
            : parsedMediaType.SubTypeWithoutSuffix;

        if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                statusCode: 400,
                detail: $"Not all requested data shaping fields exist on the resource: {fields}"
                ));
        }

        //Author authorFromRepo = await _authorRepository.GetAuthorAsync(authorId);
        //ExpandoObject shapedAuthorDto = await _mediator.Send(new GetAuthorByIdQuery(authorId, fields));

        //if (authorFromRepo == null)
        //{
        //    return NotFound();
        //}
        //if (shapedAuthorDto == null)
        //{
        //    return NotFound();
        //}

        //return Ok(shapedAuthorDto);

        if (primaryDataType == "vnd.marvin.author.full")
        {
            ExpandoObject shapedFullAuthorDto = await _mediator.Send(new GetFullAuthorByIdQuery(authorId, fields));
            if (shapedFullAuthorDto == null) return NotFound();

            var fullResourceToReturn = shapedFullAuthorDto as IDictionary<string, object?>;

            if(includeLinks)
            {
                fullResourceToReturn.Add("links", links);
            }

            return Ok(fullResourceToReturn);
        }

        //AuthorDto authorDtoToReturn = _mapper.Map<AuthorDto>(authorFromRepo);
        //System.Dynamic.ExpandoObject authorDtoDataShapedToReturn = authorDtoToReturn.ShapeData(fields);
        //return Ok(authorDtoDataShapedToReturn);
        //We have done these in the MediatR Handler

        //if (parsedMediaType.MediaType == "application/vnd.marvin.hateoas+json")
        //{
        //    IEnumerable<LinkDto> links = CreateLinksForAuthor(authorId, fields);

        //    IDictionary<string, object?> linkedResourceToReturn = shapedAuthorDto as IDictionary<string, object?>;
        //    linkedResourceToReturn.Add("links", links);

        //    return Ok(linkedResourceToReturn);
        //}

        ExpandoObject friendlyShapedAuthorDto = await _mediator.Send(new GetAuthorByIdQuery(authorId, fields));
        if (friendlyShapedAuthorDto == null) return NotFound();

        var friendlyResourceToReturn = friendlyShapedAuthorDto as IDictionary<string, object?>;

        if(includeLinks)
        {
            friendlyResourceToReturn.Add("links", links);
        }
        
        return Ok(friendlyResourceToReturn);
    }

    [NonAction]
    public IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string? fields)
    {
        List<LinkDto> links = new List<LinkDto>();

        if (string.IsNullOrWhiteSpace(fields))
            links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId }), "self", "GET"));
        else
            links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId, fields }), "self", "GET"));

        links.Add(new LinkDto(Url.Link("CreateCourseForAuthor", new { authorId }), "create_course_for_author", "POST"));

        links.Add(new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }), "courses", "GET"));

        return links;
    }

    [HttpPost(Name = "CreateAuthor")]
    [RequestHeaderMatchesMediaType("Content-Type", "application/json", "application/vnd.marvin.authorforcreation+json")]
    [Consumes("application/json", "application/vnd.marvin.authorforcreation+json")]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto authorForCreationDto)
    {
        //var authorEntity = _mapper.Map<Entities.Author>(authorForCreationDto);

        //_authorRepository.AddAuthor(authorEntity);
        //await _authorRepository.SaveAsync();

        //AuthorDto authorDtoToReturn = _mapper.Map<AuthorDto>(authorEntity); //There is no need to map from AuthorForCreationDto to AuthorDto and dealing with converting DateOfBirth to Age. This mapping is done indirectly and through the entity.

        AuthorDto authorDtoToReturn = await _mediator.Send(new CreateAuthorCommand(authorForCreationDto));

        IEnumerable<LinkDto> links = CreateLinksForAuthor(authorDtoToReturn.Id, null);

        ExpandoObject expandoObjectOfAuthorDtoToReturn = authorDtoToReturn.ShapeData(null);//Just for converting to ExpandoObject(no data shaping happens here)
        IDictionary<string, object?> linkedResourceToReturn = expandoObjectOfAuthorDtoToReturn as IDictionary<string, object?>;

        linkedResourceToReturn.Add("links", links);

        //201 Created:
        //return CreatedAtRoute("GetAuthor",
        //    new { authorId = authorDtoToReturn.Id },
        //    authorDtoToReturn);
        return CreatedAtRoute("GetAuthor",
            new { authorId = linkedResourceToReturn["Id"] },
            linkedResourceToReturn);
    }

    [HttpPost(Name = "CreateAuthorWithDateOdDeath")]
    [RequestHeaderMatchesMediaType("Content-Type", "application/vnd.marvin.authorforcreationwithdateofdeath+json")]
    [Consumes("application/vnd.marvin.authorforcreationwithdateofdeath+json")]
    public async Task<ActionResult<AuthorDto>> CreateAuthorWithDateOdDeath(AuthorForCreationWithDateOfDeathDto authorForCreationDto)
    {
        AuthorDto authorDtoToReturn = await _mediator.Send(new CreateAuthorWithDateOfDeathCommand(authorForCreationDto));

        IEnumerable<LinkDto> links = CreateLinksForAuthor(authorDtoToReturn.Id, null);

        ExpandoObject expandoObjectOfAuthorDtoToReturn = authorDtoToReturn.ShapeData(null);
        IDictionary<string, object?> linkedResourceToReturn = expandoObjectOfAuthorDtoToReturn as IDictionary<string, object?>;

        linkedResourceToReturn.Add("links", links);

        return CreatedAtRoute("GetAuthor",
            new { authorId = linkedResourceToReturn["Id"] },
            linkedResourceToReturn);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }
}