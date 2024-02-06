using System.Globalization;
using System.Net;

namespace API_VietQR.Modal.Exceptions
{
	[Serializable]
	public class AppException : Exception
	{
		public string Code;

		public HttpStatusCode StatusCode;

		protected AppException() : base() { }

		public AppException(string message) : base(message) { }

		public AppException(HttpStatusCode statusCode, string code, String message) : base(message)
		{
			this.StatusCode = statusCode;
			this.Code = code;
		}

		public AppException(string message, params object[] args)
			: base(String.Format(CultureInfo.CurrentCulture, message, args))
		{
		}
	}
}
