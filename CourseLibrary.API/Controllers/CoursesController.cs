using CourseLibrary.Application.Application.Authors.Queries;
using CourseLibrary.Application.Application.Courses.Commands;
using CourseLibrary.Application.Application.Courses.Queries;
using CourseLibrary.Application.Models;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors/{authorId}/courses")]
[ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
public class CoursesController : ControllerBase
{
    //private readonly ICourseRepository _courseRepository;
    //private readonly IAuthorRepository _authorRepository;
    //private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CoursesController(//ICourseRepository courseRepository, IAuthorRepository authorRepository, IMapper mapper,
        IMediator mediator)
    {
        //_courseRepository = courseRepository ??
        //    throw new ArgumentNullException(nameof(courseRepository));
        //_authorRepository = authorRepository ??
        //    throw new ArgumentNullException(nameof(authorRepository));
        //_mapper = mapper ??
        //    throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(_mediator));
    }

    [HttpGet(Name = "GetCoursesForAuthor")]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesForAuthor(Guid authorId)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //{
        //    return NotFound();
        //}
        if (!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
        {
            return NotFound();
        }

        //IEnumerable<Course> coursesForAuthorFromRepo = await _courseRepository.GetCoursesAsync(authorId);
        //return Ok(_mapper.Map<IEnumerable<CourseDto>>(coursesForAuthorFromRepo));
        
        IEnumerable<CourseDto> coursesForAuthorToReturn = await _mediator.Send(new GetCoursesForAuthorQuery(authorId));
        return Ok(coursesForAuthorToReturn);
    }

    [ResponseCache(Duration = 120)]
    [HttpGet("{courseId}", Name = "GetCourseForAuthor")]
    public async Task<ActionResult<CourseDto>> GetCourseForAuthor(Guid authorId, Guid courseId)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //{
        //    return NotFound();
        //}
        if(!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
            return NotFound();

        //var courseForAuthorFromRepo = await _courseRepository.GetCourseAsync(authorId, courseId);

        //if (courseForAuthorFromRepo == null)
        //{
        //    return NotFound();
        //}

        //return Ok(_mapper.Map<CourseDto>(courseForAuthorFromRepo));

        CourseDto? courseDtoToReturn = await _mediator.Send(new GetCourseForAuthorQuery(authorId, courseId));
        if (courseDtoToReturn == null)
            return NotFound();

        return Ok(courseDtoToReturn);
    }

    [HttpPost(Name = "CreateCourseForAuthor")]
    public async Task<ActionResult<CourseDto>> CreateCourseForAuthor(Guid authorId, CourseForCreationDto courseFroCreation)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //{
        //    return NotFound();
        //}
        if (!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
            return NotFound();

        //var courseEntity = _mapper.Map<Entities.Course>(course);

        //_courseRepository.AddCourse(authorId, courseEntity);
        //await _courseRepository.SaveAsync();

        //var courseToReturn = _mapper.Map<CourseDto>(courseEntity);
        CourseDto courseToReturn = await _mediator.Send(new CreateCourseForAuthorCommand(authorId, courseFroCreation));
        
        //return Ok(courseToReturn); //for POST method, returning a 200 Ok is not standard
        return CreatedAtRoute("GetCourseForAuthor",
            //new { authorId = authorId, courseId = courseToReturn.Id },
            new { authorId, courseId = courseToReturn.Id },
            courseToReturn
            );
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto updatedCourse)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //{
        //    return NotFound();
        //}
        if (!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
            return NotFound();

        //Course courseToUpdateEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        //if (courseToUpdateEntity == null)
        //{
        //    return NotFound();
        //}
        //if (courseToUpdateEntity == null)
        //{
        //    Course courseToAddEntity = _mapper.Map<Course>(updatedCourse);
        //    courseToAddEntity.Id = courseId;

        //    _courseRepository.AddCourse(authorId, courseToAddEntity);
        //    await _courseRepository.SaveAsync();

        //    CourseDto returningCourse = _mapper.Map<CourseDto>(courseToAddEntity);

        //    return CreatedAtRoute("GetCourseForAuthor",
        //    new { authorId, courseId = returningCourse.Id },
        //    returningCourse);
        //}

        //_mapper.Map(updatedCourse, courseToUpdateEntity);

        //await _courseRepository.SaveAsync();
        //Changes are made to the tracked entity in memory. EF Core detects these modifications via change tracking.
        //Until SaveAsync is called, there is no interaction with the database.
        //aside from loading the entity(GetCourseAsync) and saving changes(SaveAsync), the repository is not involved.

        //return NoContent();

        CourseDto upsertedCourse = await _mediator.Send(new UpdateCourseForAuthorCommand(authorId, courseId, updatedCourse));

        if(upsertedCourse == null) return NoContent();

        return CreatedAtRoute("GetCourseForAuthor",
            new { authorId, courseId },
            upsertedCourse);
    }

    [HttpPatch("{courseId}")]
    public async Task<IActionResult> PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //    return NotFound();
        if(!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
            return NotFound();

        if (patchDocument == null)
            return BadRequest();

        //Course courseToUpdateEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        //if (courseToUpdateEntity == null)
        //    return NotFound();
        //if (courseToUpdateEntity == null)
        //{
        //    CourseForUpdateDto courseForUpdateDto = new CourseForUpdateDto();
        //    patchDocument.ApplyTo(courseForUpdateDto, ModelState);

        //    if (!TryValidateModel(courseForUpdateDto))
        //    {
        //        return ValidationProblem(ModelState);
        //    }

        //    Course courseToAddEntity = _mapper.Map<Course>(courseForUpdateDto);
        //    courseToAddEntity.Id = courseId;

        //    _courseRepository.AddCourse(authorId, courseToAddEntity);
        //    await _courseRepository.SaveAsync();

        //    CourseDto returningCourse = _mapper.Map<CourseDto>(courseToAddEntity);

        //    return CreatedAtRoute("GetCourseForAuthor",
        //    new { authorId, courseId = returningCourse.Id },
        //    returningCourse);
        //}

        //CourseForUpdateDto courseToPatch = _mapper.Map<CourseForUpdateDto>(courseToUpdateEntity);
        //patchDocument.ApplyTo(courseToPatch, ModelState); //Move patch errors from exceptions to ModelState => 400 instead of 500

        //if (!TryValidateModel(courseToPatch))
        //{
        //    return ValidationProblem(ModelState);
        //}

        //_mapper.Map(courseToPatch, courseToUpdateEntity);

        //await _courseRepository.SaveAsync();

        //return NoContent();

        try
        {
            var createdCourse =
                await _mediator.Send(
                    new PartiallyUpdateCourseForAuthorCommand(
                        authorId,
                        courseId,
                        patchDocument));

            if (createdCourse == null)
                return NoContent();

            return CreatedAtRoute(
                "GetCourseForAuthor",
                new { authorId, courseId = createdCourse.Id },
                createdCourse);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{courseId}")]
    public async Task<ActionResult> DeleteCourseForAuthor(Guid authorId, Guid courseId)
    {
        //if (!await _authorRepository.AuthorExistsAsync(authorId))
        //{
        //    return NotFound();
        //}
        if (!await _mediator.Send(new CheckAuthorExistsQuery(authorId)))
            return NotFound();

        //var courseForAuthorFromRepo = await _courseRepository.GetCourseAsync(authorId, courseId);

        //if (courseForAuthorFromRepo == null)
        //{
        //    return NotFound();
        //}

        bool deleted = await _mediator.Send(new DeleteCourseForAuthorCommand(authorId, courseId));

        //_courseRepository.DeleteCourse(courseForAuthorFromRepo);
        //await _courseRepository.SaveAsync();

        //return NoContent();

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    {
        IOptions<ApiBehaviorOptions> options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

        return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}