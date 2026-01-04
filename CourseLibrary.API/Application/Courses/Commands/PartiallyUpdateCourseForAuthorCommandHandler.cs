using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using CourseLibrary.API.Repositories.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Application.Courses.Commands;

public class PartiallyUpdateCourseForAuthorCommandHandler : IRequestHandler<PartiallyUpdateCourseForAuthorCommand, CourseDto?>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public PartiallyUpdateCourseForAuthorCommandHandler(
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository
            ?? throw new ArgumentNullException(nameof(courseRepository));
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<CourseDto?> Handle(PartiallyUpdateCourseForAuthorCommand request, CancellationToken cancellationToken)
    {
        var courseEntity = await _courseRepository.GetCourseAsync(request.AuthorId, request.CourseId);

        if (courseEntity == null)
        {
            CourseForUpdateDto courseToAdd = new CourseForUpdateDto();

            request.PatchDocument.ApplyTo(courseToAdd);

            Validate(courseToAdd);

            var newEntity = _mapper.Map<Course>(courseToAdd);
            newEntity.Id = request.CourseId;

            _courseRepository.AddCourse(request.AuthorId, newEntity);
            await _courseRepository.SaveAsync();

            return _mapper.Map<CourseDto>(newEntity);
        }

        var courseToPatch = _mapper.Map<CourseForUpdateDto>(courseEntity);

        request.PatchDocument.ApplyTo(courseToPatch);
        Validate(courseToPatch);

        _mapper.Map(courseToPatch, courseEntity);
        await _courseRepository.SaveAsync();

        return null;
    }

    private static void Validate(CourseForUpdateDto dto)
    {
        Validator.ValidateObject(
            dto,
            new System.ComponentModel.DataAnnotations.ValidationContext(dto),
            validateAllProperties: true);
    }
}
