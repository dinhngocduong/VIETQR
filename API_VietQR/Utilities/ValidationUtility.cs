using System.Text.RegularExpressions;

namespace API_VietQR.Utilities
{
	public class ValidationUtility
	{
		private string SizeFormat(float size, string formatString)
		{
			if (size < 1024)
				return size.ToString(formatString) + " bytes";
			// Math.Pow nhan 2 so thuc
			if (size < Math.Pow(1024, 2))
				return (size / 1024).ToString(formatString) + " kb";

			if (size < Math.Pow(1024, 3))
				return (size / Math.Pow(1024, 2)).ToString(formatString) + " mb";

			if (size < Math.Pow(1024, 4))
				return (size / Math.Pow(1024, 3)).ToString(formatString) + " gb";

			return size.ToString(formatString);
		}

		// Kiem tra dung luong cua file
		private bool SizeFormat(double size, float valsize, FileSize filesize)
		{
			switch (filesize)
			{
				case FileSize.Bytes:
					if (size > valsize)
						return false;
					break;
				case FileSize.KB:
					size = size / 1024;
					if (size > valsize)
						return false;
					break;
				case FileSize.MB:
					size = size / Math.Pow(1024, 2);
					if (size > valsize)
						return false;
					break;
				case FileSize.GB:
					size = size / Math.Pow(1024, 3);
					if (size > valsize)
						return false;
					break;
			}
			return true;
		}

		// Regular expression to validate e-mail addresses
		public static string EmailRegex = @"^(\w[-._\w]*@\w[-._\w]*\w\.\w{2,6})$";

		// Regular expression to validate urls
		public static string UrlRegex = @"^((((http|https):\/\/)|^)[\w-_]+(\.[\w-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)$";

		public static bool AreEmails(object value)
		{
			return false;
		}

		public static byte[] GetBinary(object value, byte[] defaultValue)
		{
			return null;
		}

		public static bool GetBoolean(object value, bool defaultValue)
		{
			return false;
		}

		public static DateTime GetDateTime(object value, DateTime defaultValue)
		{
			return DateTime.Now;
		}

		public static DateTime GetDateTime(object value, DateTime defaultValue, IFormatProvider format)
		{
			return DateTime.Now;
		}

		public static double GetDouble(object value, double defaultValue)
		{
			return 0;
		}

		public static Guid GetGuid(object value, Guid defaultValue)
		{
			return Guid.NewGuid();
		}

		public static int GetInteger(object value, int defaultValue)
		{
			return 0;
		}

		public static string GetString(object value, string defaultValue)
		{
			return "";
		}

		public static bool IsBoolean(object value)
		{
			try
			{
				bool rep = Convert.ToBoolean(value);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool IsCodeName(object value)
		{
			return false;
		}

		public static bool IsDouble(object value)
		{
			return false;
		}

		public static bool IsEmail(object value)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				return false;
			}

			string strRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
			Regex re = new Regex(strRegex, RegexOptions.Compiled);
			if (re.IsMatch(value.ToString()))
				return true;
			else
				return false;

		}

		public static bool IsNonUnicode(string value)
		{
			if (string.IsNullOrEmpty(value.ToString()))
				return false;
			else
			{
				string strRegex = @"^[a-zA-Z\s]+$";
				Regex re = new Regex(strRegex, RegexOptions.Compiled);
				if (re.IsMatch(value.ToString()))
					return true;
				else
					return false;
			}
		}

		public static bool IsIdentificator(object value)
		{
			return false;
		}

		public static bool IsInteger(object value)
		{
			return false;
		}

		public static bool IsIntervalValid(DateTime fromDate, DateTime toDate, bool nullFriendly)
		{
			return false;
		}

		public static bool IsPositiveNumber(object value)
		{
			return false;
		}

		private static Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

		public static bool IsGuid(object candidate)
		{
			if (candidate.ToString() != null)
			{
				if (isGuid.IsMatch(candidate.ToString()))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsNumeric(string strTextEntry)
		{
			try
			{
				double tmp = Convert.ToDouble(strTextEntry);
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}

		public static bool IsDate(string strTextEntry)
		{
			try
			{
				DateTime checkTime;
				checkTime = Convert.ToDateTime(strTextEntry);
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}
	}
	public enum FileSize
	{
		Bytes = 0,
		KB = 1,
		MB = 2,
		GB = 3
	}
}
