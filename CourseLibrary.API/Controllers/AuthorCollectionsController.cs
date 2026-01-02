using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        //private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionsController(ICourseRepository courseRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({authorIds})", Name = "GetAuthorCollection")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> authorIds)
        {
            IEnumerable<Author> authorEntities = await _authorRepository.GetAuthorsAsync(authorIds);

            if (authorEntities.Count() != authorIds.Count())
                return NotFound();

            IEnumerable<AuthorDto> authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            return Ok(authorsToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorCollection)
        {
            IEnumerable<Author> authorsEntities = _mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (Author author in authorsEntities)
            {
                _authorRepository.AddAuthor(author);
            }

            await _authorRepository.SaveAsync();

            IEnumerable<AuthorDto> authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
            string authorIdsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetAuthorCollection",
                new { authorIds = authorIdsString },
                authorCollectionToReturn);
        }
    }
}