using System;

namespace API.Exceptions
{
	public class BadCredentialsException : Exception
	{
		public string ErrorMessage { get; set; }

		public BadCredentialsException(string errorMessage)
		{
			ErrorMessage = errorMessage;
		}
	}
}