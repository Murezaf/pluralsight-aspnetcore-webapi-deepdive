using AutoMapper;
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
        var pagedAuthors = await _mediator.Send(new GetAuthorsQuery(authorRecourseParameters));

        string? previousPageLink = pagedAuthors.HasPrevious ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.PreviousPage) : null;
        string? nextPageLink = pagedAuthors.HasNext ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.NextPage) : null;

        var paginationMetaData = new //Anonymous Object 
        {
            totalCount = pagedAuthors.TotalCount,
            pageSize = pagedAuthors.PageSize,
            pageNumber = pagedAuthors.CurrentPage,
            totalPages = pagedAuthors.TotalPages,
            nextPageLink = nextPageLink,
            previousPageLink = previousPageLink
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

        //return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));

        //IEnumerable<AuthorDto> authorsDtoToReturn = _mapper.Map<IEnumerable<AuthorDto>>(pagedAuthorsFromRepo);
        //IEnumerable<System.Dynamic.ExpandoObject> authorsDtoDataShapedToReturn = authorsDtoToReturn.ShapeData(authorRecourseParameters.Fields);
        //return Ok(authorsDtoDataShapedToReturn);
        //We have done these in the MediatR Handler

        return Ok(pagedAuthors);
    }

    [HttpGet("{authorId}", Name = "GetAuthor")]
    //public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
    public async Task<IActionResult> GetAuthor(Guid authorId, string? fields)
    {
        if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                statusCode: 400,
                detail: $"Not all requested data shaping fields exist on the resource: {fields}"
                ));
        }

        //Author authorFromRepo = await _authorRepository.GetAuthorAsync(authorId);
        ExpandoObject shapedAuthorDto = await _mediator.Send(new GetAuthorByIdQuery(authorId, fields));


        //if (authorFromRepo == null)
        //{
        //    return NotFound();
        //}
        if (shapedAuthorDto == null)
        {
            return NotFound();
        }

        //AuthorDto authorDtoToReturn = _mapper.Map<AuthorDto>(authorFromRepo);
        //System.Dynamic.ExpandoObject authorDtoDataShapedToReturn = authorDtoToReturn.ShapeData(fields);
        //return Ok(authorDtoDataShapedToReturn);
        //We have done these in the MediatR Handler

        return Ok(shapedAuthorDto);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto authorForCreationDto)
    {
        //var authorEntity = _mapper.Map<Entities.Author>(authorForCreationDto);

        //_authorRepository.AddAuthor(authorEntity);
        //await _authorRepository.SaveAsync();

        //AuthorDto authorDtoToReturn = _mapper.Map<AuthorDto>(authorEntity); //There is no need to map from AuthorForCreationDto to AuthorDto and dealing with converting DateOfBirth to Age. This mapping is done indirectly and through the entity.

        AuthorDto authorDtoToReturn = await _mediator.Send(new CreateAuthorCommand(authorForCreationDto));

        //201 Created:
        return CreatedAtRoute("GetAuthor",
            new { authorId = authorDtoToReturn.Id },
            authorDtoToReturn);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }
}