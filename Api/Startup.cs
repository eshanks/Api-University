using Data;
using Data.Entities;
using Data.Providers;
using Data.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using System.Collections.Generic;

namespace Api
{
	public class Startup
	{
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			return ConfigureIoC(services);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			
			app.UseMvc();
		}
		
		/// <summary>
		///		Configures the IoC container for the application. 
		/// </summary>
		private static IServiceProvider ConfigureIoC(IServiceCollection services)
		{
			var container = new Container(config =>
			{
				config.For<DbContext>().Use<UniversityContext>().Singleton();
				config.For<IStudentProvider>().Use<StudentProvider>();
				config.For<ICourseProvider>().Use<CourseProvider>();
				config.For<IProfessorProvider>().Use<ProfessorProvider>();
				config.For<IEnrollmentProvider>().Use<EnrollmentProvider>();
				config.For<IValidator<Student>>().Use<StudentValidator>();
				config.For<IValidator<Course>>().Use<CourseValidator>();
				config.For<IValidator<Professor>>().Use<ProfessorValidator>();
				config.For<IEnumerable<IValidationRule<Course>>>().Use(new List<IValidationRule<Course>> { new CourseNameRequiredRule() });
				config.For<IEnumerable<IValidationRule<Professor>>>().Use(new List<IValidationRule<Professor>> { new ProfessorNameRequiredRule() });
				config.For<IValidator<Enrollment>>().Use<EnrollmentValidator>()
					.Ctor<IEnumerable<IValidationRule<Enrollment>>>()
						.Is(ctx => new List<IValidationRule<Enrollment>> { ctx.GetInstance<EnrollmentCountRule>() });

				config.Populate(services);
			});

			Seed(container);

			return container.GetInstance<IServiceProvider>();
		}

		/// <summary>
		///		Seeds the in-memory database with data.
		/// </summary>
		/// <param name="container"> The IoC container. </param>
		private static void Seed(IContainer container)
		{
			var courseProvider = container.GetInstance<ICourseProvider>();
			var courseEntity = new Course { Name = "Software Development I" };
			courseProvider.Create(courseEntity);
			Console.WriteLine($"Course: {courseEntity.Id} - {courseEntity.Name} added!");
			courseEntity = new Course { Name = "Software Development II" };
			courseProvider.Create(courseEntity);
			Console.WriteLine($"Course: {courseEntity.Id} - {courseEntity.Name} added!");
			courseEntity = new Course { Name = "Software Development I" };
			courseProvider.Create(courseEntity);
			Console.WriteLine($"Course: {courseEntity.Id} - {courseEntity.Name} added!");
			courseEntity = new Course { Name = "SQL Development I" };
			courseProvider.Create(courseEntity);
			Console.WriteLine($"Course: {courseEntity.Id} - {courseEntity.Name} added!");

			Console.WriteLine();

			var professorProvider = container.GetInstance<IProfessorProvider>();
			var professorEntity = new Professor { Name = "Dr. Hall" };
			var courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(1), Professor = professorEntity }; 
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(3), Professor = professorEntity };
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			professorProvider.Create(professorEntity);
			Console.WriteLine($"Professor: {professorEntity.Id} - {professorEntity.Name} added!");

			professorEntity = new Professor { Name = "Dr. Jones" };
			courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(2), Professor = professorEntity };
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			professorProvider.Create(professorEntity);
			Console.WriteLine($"Professor: {professorEntity.Id} - {professorEntity.Name} added!");

			professorEntity = new Professor { Name = "Dr. James" };
			courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(1), Professor = professorEntity };
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(2), Professor = professorEntity };
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			courseProfessorEntity = new CourseProfessor { Course = courseProvider.GetById(3), Professor = professorEntity };
			professorEntity.CourseProfessors.Add(courseProfessorEntity);
			professorProvider.Create(professorEntity);
			Console.WriteLine($"Professor: {professorEntity.Id} - {professorEntity.Name} added!");

			Console.WriteLine();

			var studentProvider = container.GetInstance<IStudentProvider>();
			var studentEntity = new Student { Name = "Heather Barton" };
			studentProvider.Create(studentEntity);
			Console.WriteLine($"Student: {studentEntity.Id} - {studentEntity.Name} added!");
			studentEntity = new Student { Name = "Michelle Brewer" };
			studentProvider.Create(studentEntity);
			Console.WriteLine($"Student: {studentEntity.Id} - {studentEntity.Name} added!");
			studentEntity = new Student { Name = "Kimberly Macias" };
			studentProvider.Create(studentEntity);
			Console.WriteLine($"Student: {studentEntity.Id} - {studentEntity.Name} added!");
			studentEntity = new Student { Name = "Lisa Tapia" };
			studentProvider.Create(studentEntity);
			Console.WriteLine($"Student: {studentEntity.Id} - {studentEntity.Name} added!");
			studentEntity = new Student { Name = "Jennifer Schmidt" };
			studentProvider.Create(studentEntity);
			Console.WriteLine($"Student: {studentEntity.Id} - {studentEntity.Name} added!");

			Console.WriteLine();

			var enrollmentProvider = container.GetInstance<IEnrollmentProvider>();
			var enrollment = enrollmentProvider.Enroll(new Student { Id = 1 }, new Course { Id = 1 }, new Professor { Id = 2 });
			Console.WriteLine($"Student: {enrollment.Student.Name} enrolled into {enrollment.Course.Name}!");

			// BUG: This line fails, need to find the cause and correct the issue. 
			Console.WriteLine($"Student: {enrollment.Student.Name} enrolled into {enrollment.Course.Name} instructed by {enrollment.Professor.Name}!");

			Console.WriteLine();
		}
	}
}
