using System;

namespace API.Exceptions
{
	public class EntityNotFoundException : Exception
	{
		public string ErrorMessage { get; set; }

		public EntityNotFoundException(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}
	}
}