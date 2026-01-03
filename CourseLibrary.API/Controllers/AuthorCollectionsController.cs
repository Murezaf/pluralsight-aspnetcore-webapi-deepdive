using AutoMapper;
using CourseLibrary.API.Application.AuthorCollections.Commands;
using CourseLibrary.API.Application.AuthorCollections.Queries;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        //private readonly ICourseLibraryRepository _courseLibraryRepository;
        //private readonly IAuthorRepository _authorRepository;
        //private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AuthorCollectionsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("({authorIds})", Name = "GetAuthorCollection")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthorCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> authorIds)
        {
            //IEnumerable<Author> authorEntities = await _authorRepository.GetAuthorsAsync(authorIds);

            //if (authorEntities.Count() != authorIds.Count())
            //    return NotFound();

            //IEnumerable<AuthorDto> authorsToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);

            var authorsToReturn = await _mediator.Send(new GetAuthorCollectionQuery(authorIds));

            if (authorsToReturn == null)
                return NotFound();

            return Ok(authorsToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> CreateAuthorCollection(IEnumerable<AuthorForCreationDto> authorForCreationCollection)
        {
            //IEnumerable<Author> authorsEntities = _mapper.Map<IEnumerable<Author>>(authorForCreationCollection);

            //foreach (Author author in authorsEntities)
            //{
            //    _authorRepository.AddAuthor(author);
            //}

            //await _authorRepository.SaveAsync();

            //IEnumerable<AuthorDto> authorCollectionToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorsEntities);
            //We have done these in the MediatR Handler

            IEnumerable<AuthorDto> authorCollectionToReturn = await _mediator.Send(new CreateAuthorCollectionCommand(authorForCreationCollection));

            string authorIdsString = string.Join(",", authorCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetAuthorCollection",
                new { authorIds = authorIdsString },
                authorCollectionToReturn);
        }
    }
}