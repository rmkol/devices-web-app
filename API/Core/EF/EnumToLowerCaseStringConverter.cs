using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Core.EF
{
	public class EnumToLowerCaseStringConverter<T> : EnumToStringConverter<T> where T : struct
	{
		public override Func<object, object> ConvertToProvider => o => o.ToString().ToLower();
	}
}