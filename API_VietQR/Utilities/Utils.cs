using System.Text.Json;

namespace API_VietQR.Utilities
{
	public class Utils
	{
		public static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
		public static string SplitSubString(string sStart, string sEnd, string sContent)
		{

			int iStart = 0;
			if (string.IsNullOrEmpty(sStart))
			{
				return string.Empty;
			}
			iStart = sContent.IndexOf(sStart);
			if (iStart < 0 && string.IsNullOrEmpty(sEnd))
			{
				return string.Empty;
			}
			int iEnd = sContent.IndexOf(sEnd, iStart + sStart.Length);
			if (iStart >= 0 && iEnd >= 0 && iStart < iEnd)
			{
				sContent = sContent.Substring(iStart + sStart.Length, iEnd - iStart - sStart.Length);
				return sContent;
			}
			return string.Empty;
		}

	}
}
