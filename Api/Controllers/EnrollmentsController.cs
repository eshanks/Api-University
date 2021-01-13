using Data.Providers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
	/// <summary>
	///		An API controller used to manage student enrollments. 
	/// </summary>
	[Route("api/enrollment")]
	public class EnrollmentsController
		: Controller
	{
		// TODO : Need to create an endpoint to allow a student to enroll in a course. 

		private readonly IEnrollmentProvider _enrollmentProvider;
		private readonly IProfessorProvider _professorProvider;
		private readonly IStudentProvider _studentProvider;
		private readonly ICourseProvider _courseProvider;

		/// <summary>
		///		Creates a new instance of a <see cref="EnrollmentsController"/>. 
		/// </summary>
		/// <param name="courseProvider"> Provider for enrollment information. </param>
		public EnrollmentsController(IEnrollmentProvider enrollmentProvider, IProfessorProvider professorProvider, IStudentProvider studentProvider, ICourseProvider courseProvider)
		{
			_enrollmentProvider = enrollmentProvider;
			_professorProvider = professorProvider;
			_studentProvider = studentProvider;
			_courseProvider = courseProvider;
		}

		[HttpPost]
		[Route("enroll")]
		public IActionResult Enroll(int studentId, int courseId, int professorId)
		{
            try
			{
				var student = _studentProvider.GetById(studentId);
				if(student == null)
					return StatusCode(400, "No student exists with that id");
				var course = _courseProvider.GetById(courseId);
				if (course == null)
					return StatusCode(400, "No course exists with that id");
				var professor = _professorProvider.GetById(professorId);
				if (professor == null)
					return StatusCode(400, "No professor exists with that id");
				var enrollment = _enrollmentProvider.Enroll(student, course, professor);
				if(enrollment != null)
                {
					return StatusCode(200);
				}
                else
                {
					return StatusCode(400);
				}
			}
			catch(Exception ex)
			{
				return StatusCode(400, ex.Message);
			}
		}
	}
}
