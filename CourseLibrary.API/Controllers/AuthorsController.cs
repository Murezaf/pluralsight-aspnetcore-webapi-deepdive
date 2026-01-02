using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories;
using CourseLibrary.API.Repositories.Interfaces;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Text.Json;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    //private readonly ICourseLibraryRepository _courseLibraryRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IPropertyCheckerService _propertyCheckerService;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AuthorsController(
        //ICourseLibraryRepository courseLibraryRepository,
        IAuthorRepository authorRepository, IMapper mapper,
        IPropertyMappingService propertyMappingService, IPropertyCheckerService propertyCheckerService,
        ProblemDetailsFactory problemDetailsFactory)
    {
        //_courseLibraryRepository = courseLibraryRepository ??
        //    throw new ArgumentNullException(nameof(courseLibraryRepository));
        _authorRepository = authorRepository ??
            throw new ArgumentNullException(nameof(authorRepository));
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
        var authorsFromRepo = await _authorRepository.GetAuthorsAsync(authorRecourseParameters);
        //var == PagedList<Entities.Author>

        string? previousPageLink = authorsFromRepo.HasPrevious ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.PreviousPage) : null;
        string? nextPageLink = authorsFromRepo.HasNext ? CreateAuthorsResourceUri(authorRecourseParameters, ResorceUriType.NextPage) : null;

        var paginationMetaData = new //Anonymous Object 
        {
            totalCount = authorsFromRepo.TotalCount,
            pageSize = authorsFromRepo.PageSize,
            pageNumber = authorsFromRepo.CurrentPage,
            totalPages = authorsFromRepo.TotalPages,
            nextPageLink = nextPageLink,
            previousPageLink = previousPageLink
        };

        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

        //return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        IEnumerable<AuthorDto> authorsDtoToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo);
        IEnumerable<System.Dynamic.ExpandoObject> authorsDtoDataShapedToReturn = authorsDtoToReturn.ShapeData(authorRecourseParameters.Fields);
        return Ok(authorsDtoDataShapedToReturn);
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

        var authorFromRepo = await _authorRepository.GetAuthorAsync(authorId);

        if (authorFromRepo == null)
        {
            return NotFound();
        }

        AuthorDto authorDtoToReturn = _mapper.Map<AuthorDto>(authorFromRepo);
        System.Dynamic.ExpandoObject authorDtoDataShapedToReturn = authorDtoToReturn.ShapeData(fields);
        return Ok(authorDtoDataShapedToReturn);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _authorRepository.AddAuthor(authorEntity);
        await _authorRepository.SaveAsync();

        var authorToReturn = _mapper.Map<AuthorDto>(authorEntity); //There is no need to map from AuthorForCreationDto to AuthorDto and dealing with converting DateOfBirth to Age. This mapping is done indirectly and through the entity.

        //201 Created:
        return CreatedAtRoute("GetAuthor",
            new { authorId = authorToReturn.Id },
            authorToReturn);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
        return Ok();
    }
}