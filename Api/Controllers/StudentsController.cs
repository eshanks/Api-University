using Api.Models;
using Data.Providers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    /// <summary>
    ///		An API controller used to manage students. 
    /// </summary>
    [Route("api/students")]
    public class StudentsController
        : Controller
    {

        private readonly IStudentProvider _studentProvider;

        /// <summary>
        ///		Creates a new instance of a <see cref="StudentsController"/>. 
        /// </summary>
        /// <param name="courseProvider"> Provider for student information. </param>
        public StudentsController(IStudentProvider studentProvider)
        {
            _studentProvider = studentProvider;
        }

        [HttpGet]
        [Route("{studentId}/courses")]
        public IEnumerable<CourseModel> Courses(int studentId)
        {
            var student = _studentProvider.GetById(studentId);
            var courses = student.Enrollments.Select(enrollment => new CourseModel()
            {
                Id = enrollment.CourseId,
                Name = enrollment.Course.Name,
                Professors = enrollment.Course.CourseProfessors.Select(p => new ProfessorModel() { Id = p.Professor.Id, Name = p.Professor.Name })
            });
            return courses;
        }
    }
}
