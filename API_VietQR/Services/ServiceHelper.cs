using API_VietQR.Response;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using API_VietQR.Request;

namespace API_VietQR.Services
{
	public interface IServiceHelper
	{
		Task<BaseResponse<string>> SendToOtherService(HttpMethod httpMethod, string path, string contentType, string requestBodyStr, CancellationToken cancellationToken,
			string bearerToken = "");
		Task<BaseResponse<string>> SendToOtherService(HttpMethod httpMethod, string path, string contentType, string requestBodyStr, List<HeaderAPIRequest> headerAPIs, CancellationToken cancellationToken,
	string bearerToken = "");
	}
	public class ServiceHelper : IServiceHelper
	{
		public ServiceHelper()
		{
		}
		public async Task<BaseResponse<string>> SendToOtherService(HttpMethod httpMethod, string path, string contentType, string requestBodyStr, CancellationToken cancellationToken,
		  string bearerToken = "")
		{
			try
			{
				using HttpClient client = new HttpClient();

				if (!string.IsNullOrEmpty(bearerToken))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
				}

				var content = new StringContent(requestBodyStr, Encoding.UTF8, contentType);
				var uri = path;
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
				httpRequestMessage.Content = content;

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
				HttpResponseMessage httpResponse;
				var stopwatch = Stopwatch.StartNew();
				try
				{
					httpResponse = await client.SendAsync(httpRequestMessage, cancellationToken);
					stopwatch.Stop();
					string responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

					if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.NoContent ||
						httpResponse.StatusCode == HttpStatusCode.Created || httpResponse.StatusCode == HttpStatusCode.NotAcceptable
						|| httpResponse.StatusCode == HttpStatusCode.NotFound)
					{
						return new BaseResponse<string> { Success = true, Code = "00", Data = responseBody };
					}
					return new BaseResponse<string> { Success = false, Data = responseBody };
				}
				catch (Exception ex)
				{
					return new BaseResponse<string> { Success = false, Code = "-1", Message = ex.Message };
				}
			}
			catch (Exception ex)
			{
				return new BaseResponse<string> { Success = false, Code = "-1", Message = ex.Message };
			}

		}
		public async Task<BaseResponse<string>> SendToOtherService(HttpMethod httpMethod, string path, string contentType, string requestBodyStr, List<HeaderAPIRequest> headerAPIs, CancellationToken cancellationToken,
		string bearerToken = "")
		{
			try
			{
				using HttpClient client = new HttpClient();

				if (!string.IsNullOrEmpty(bearerToken))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
				}
				foreach (var item in headerAPIs)
				{
					client.DefaultRequestHeaders.Add(item.Key, item.Value);
				}

				var content = new StringContent(requestBodyStr, Encoding.UTF8, contentType);
				var uri = path;
				HttpRequestMessage httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
				httpRequestMessage.Content = content;

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
				HttpResponseMessage httpResponse;
				var stopwatch = Stopwatch.StartNew();
				try
				{
					httpResponse = await client.SendAsync(httpRequestMessage, cancellationToken);
					stopwatch.Stop();
					string responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

					if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.NoContent ||
						httpResponse.StatusCode == HttpStatusCode.Created || httpResponse.StatusCode == HttpStatusCode.NotAcceptable
						|| httpResponse.StatusCode == HttpStatusCode.NotFound)
					{
						return new BaseResponse<string> { Success = true, Code = "00", Data = responseBody };
					}
					return new BaseResponse<string> { Success = false, Data = responseBody };
				}
				catch (Exception ex)
				{
					return new BaseResponse<string> { Success = false, Code = "-1", Message = ex.Message };
				}
			}
			catch (Exception ex)
			{
				return new BaseResponse<string> { Success = false, Code = "-1", Message = ex.Message };
			}
		}
	}
}
