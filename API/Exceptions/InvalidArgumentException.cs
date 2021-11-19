using System;

namespace API.Exceptions
{
	public class InvalidArgumentException : Exception
	{
		public string ErrorMessage { get; set; }

		public InvalidArgumentException(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}
	}
}