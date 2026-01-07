using AutoMapper;
using CourseLibrary.Application.Contracts.RepositoryContracts;
using CourseLibrary.Application.Contracts.ServiceContracts;
using CourseLibrary.Application.Models;
using CourseLibrary.Domain;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.Services.ServiceImplementations;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CourseDto> CreateCourseForAuthorAsync(Guid authorId, CourseForCreationDto courseForCreationDto)
    {
        var courseEntity = _mapper.Map<Course>(courseForCreationDto);

        _courseRepository.AddCourse(authorId, courseEntity);
        await _courseRepository.SaveAsync();

        return _mapper.Map<CourseDto>(courseEntity);
    }

    public async Task<bool> DeleteCourseForAuthorAsync(Guid authorId, Guid courseId)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        if (courseEntity == null)
            return false;

        _courseRepository.DeleteCourse(courseEntity);
        await _courseRepository.SaveAsync();

        return true;
    }

    public async Task<CourseDto?> GetCourseForAuthorAsync(Guid authorId, Guid courseId)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        if (courseEntity == null)
        {
            return null;
        }

        return _mapper.Map<CourseDto>(courseEntity);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesForAuthorAsync(Guid authorId)
    {
        var courses = await _courseRepository.GetCoursesAsync(authorId);
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto?> PartiallyUpdateCourseForAuthorAsync(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        if (courseEntity == null)
        {
            var courseToAdd = new CourseForUpdateDto();

            patchDocument.ApplyTo(courseToAdd);
            Validate(courseToAdd);

            var newEntity = _mapper.Map<Course>(courseToAdd);
            newEntity.Id = courseId;

            _courseRepository.AddCourse(authorId, newEntity);
            await _courseRepository.SaveAsync();

            return _mapper.Map<CourseDto>(newEntity);
        }

        var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseEntity);

        patchDocument.ApplyTo(courseToPatch);
        Validate(courseToPatch);

        _mapper.Map(courseToPatch, courseEntity);
        await _courseRepository.SaveAsync();

        return null;
    }

    public async Task<CourseDto?> UpdateCourseForAuthorAsync(Guid authorId, Guid courseId, CourseForUpdateDto courseForUpdateDto)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(authorId, courseId);

        if (courseEntity == null)
        {
            var newCourse = _mapper.Map<Course>(courseForUpdateDto);
            newCourse.Id = courseId;

            _courseRepository.AddCourse(authorId, newCourse);
            await _courseRepository.SaveAsync();

            return _mapper.Map<CourseDto>(newCourse);
        }

        _mapper.Map(courseForUpdateDto, courseEntity);
        await _courseRepository.SaveAsync();

        return null;
    }

    private static void Validate(CourseForUpdateDto dto)
    {
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(dto);
        Validator.ValidateObject(
            dto,
            validationContext,
            validateAllProperties: true);
    }
}
