using Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Data.Validation
{
	/// <summary>
	///		Represents an enrollment validator. 
	/// </summary>
	public class EnrollmentValidator
		: IValidator<Enrollment>
	{
		private readonly IEnumerable<IValidationRule<Enrollment>> _enrollmentValidationRules;
		/// <summary>
		///		Creates a new instance of a <see cref="EnrollmentValidator"/>.
		/// </summary>
		public EnrollmentValidator(IEnumerable<IValidationRule<Enrollment>> rules)
		{
			_enrollmentValidationRules = rules;
		}

		/// <summary>
		///		Determines if the target object is valid. 
		/// </summary>
		/// <param name="target"> The object to be validated. </param>
		/// <param name="errors"> The key/value pairs indicating the collection of errors. </param>
		/// <returns> The value indicating whether the object is valid. </returns>
		public bool IsValid(Enrollment target, out Dictionary<string, string> errors)
		{
			errors = new Dictionary<string, string>();
			foreach (var rule in _enrollmentValidationRules)
				rule.Enforce(target, errors);
			return !errors.Any();
		}
	}
}
