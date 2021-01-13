using System.Collections.Generic;
using Data.Entities;

namespace Data.Providers
{
	/// <summary>
	///		Represents a professor provider.
	/// </summary>
	public interface IProfessorProvider
	{
		/// <summary>
		///		Creates a new professor. 
		/// </summary>
		/// <param name="professor"> The professor object to create. </param>
		void Create(Professor professor);

		/// <summary>
		///		Gets all professors. 
		/// </summary>
		/// <returns></returns>
		IEnumerable<Professor> GetAll();

		Professor GetById(int id);
	}
}
