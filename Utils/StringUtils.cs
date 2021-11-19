using System;
using System.Linq;

namespace Utils
{
	public static class StringUtils
	{
		private static readonly Random _Random = new Random();

		public static string RandomString(bool letters, bool digits, int length)
		{
			const string ld = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			const string l = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string d = "0123456789";

			if (letters && digits) return RandomString(ld, length);
			if (letters) return RandomString(l, length);
			if (digits) return RandomString(d, length);
			return string.Empty;
		}

		private static string RandomString(string pool, int length)
		{
			return new string(Enumerable.Repeat(pool, length)
				.Select(s => s[_Random.Next(s.Length)]).ToArray());
		}
	}
}