using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using Data.Exceptions;
using Data.Validation;
using Microsoft.EntityFrameworkCore;

namespace Data.Providers
{
	/// <summary>
	///		Represents a enrollment provider. 
	/// </summary>
	public class EnrollmentProvider
		: IEnrollmentProvider
	{
		private readonly UniversityContext _context;
		private readonly IValidator<Enrollment> _validator;

		///  <summary>
		/// 		Creates a new instance of a <see cref="EnrollmentProvider"/>. 
		///  </summary>
		///  <param name="context"> The instance of a <see cref="UniversityContext"/>. </param>
		///  <param name="validator"> The instance of a <see cref="IValidator{T}"/>. </param>
		public EnrollmentProvider(UniversityContext context, IValidator<Enrollment> validator)
		{
			_context = context;
			_validator = validator;
		}

		/// <summary>
		///		Creates a new course. 
		/// </summary>
		/// <param name="student"> The student to be enrolled. </param>
		/// <param name="course"> The course to enroll the student into. </param>
		/// <param name="professor"> The professor the instructs the course. </param>
		/// <returns> The created course. </returns>
		public Enrollment Enroll(Student student, Course course, Professor professor)
		{
			var enrollment = new Enrollment { CourseId = course.Id, StudentId = student.Id, ProfessorId = professor.Id };

			// Validate the enrollment. 
			if (!_validator.IsValid(enrollment, out var errors))
				throw new ValidationException(errors);

			_context.Enrollments.Add(enrollment);
			_context.SaveChanges();

			return _context.Enrollments
				.Where(e => e.Id == enrollment.Id)
				.Include(e => e.Professor)
				.Include(e => e.Student)
				.Include(e => e.Course)
				.SingleOrDefault();
		}

		/// <summary>
		///		Gets a list of all enrollments.
		/// </summary>
		/// <returns> The list of all enrollments. </returns>
		public IEnumerable<Enrollment> GetAll()
		{
			return _context.Enrollments
				.Include(e => e.Professor)
				.Include(e => e.Student)
				.Include(e => e.Course)
				.ToList();
		}
	}
}
