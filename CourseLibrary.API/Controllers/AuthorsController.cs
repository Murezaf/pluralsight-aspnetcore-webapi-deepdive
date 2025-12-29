using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly ICourseLibraryRepository _courseLibraryRepository;
    private readonly IMapper _mapper;

    public AuthorsController(
        ICourseLibraryRepository courseLibraryRepository,
        IMapper mapper)
    {
        _courseLibraryRepository = courseLibraryRepository ??
            throw new ArgumentNullException(nameof(courseLibraryRepository));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
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
                       pageSize = authorRecourseParameters.PageSize
                   });
            case ResorceUriType.PreviousPage:
                return Url.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorRecourseParameters.SearchQuery,
                        mainCategory = authorRecourseParameters.MainCategory,
                        pageNumber = authorRecourseParameters.PageNumber - 1,
                        pageSize = authorRecourseParameters.PageSize
                    });
            default:
                return Url.Link("GetAuthors",
                    new
                    {
                        searchQuery = authorRecourseParameters.SearchQuery,
                        mainCategory = authorRecourseParameters.MainCategory,
                        pageNumber = authorRecourseParameters.PageNumber,
                        pageSize = authorRecourseParameters.PageSize
                    });
        }
    }

    [HttpGet(Name = "GetAuthors")]
    [HttpHead]
    //public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery(Name = "category")] string? mainCategory = "")
    //public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(string? mainCategory = "", string? searchQuery = "")
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery] AuthorRecourseParameters authorRecourseParameters)
    {
        //throw new Exception("Exception Test for Fault handling");

        //var authorsFromRepo = await _courseLibraryRepository.GetAuthorsAsync(mainCategory, searchQuery); 
        var authorsFromRepo = await _courseLibraryRepository.GetAuthorsAsync(authorRecourseParameters);
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

        return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
    }

    [HttpGet("{authorId}", Name = "GetAuthor")]
    public async Task<ActionResult<AuthorDto>> GetAuthor(Guid authorId)
    {
        var authorFromRepo = await _courseLibraryRepository.GetAuthorAsync(authorId);

        if (authorFromRepo == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _courseLibraryRepository.AddAuthor(authorEntity);
        await _courseLibraryRepository.SaveAsync();

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