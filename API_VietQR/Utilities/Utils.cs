using System.Text.Json;

namespace API_VietQR.Utilities
{
	public class Utils
	{
		public static JsonSerializerOptions JSON_OPTIONS = new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
	}
}
