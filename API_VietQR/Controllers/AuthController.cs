using API_VietQR.Services.Auth;
using API_VietQR.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API_VietQR.Controllers
{
	
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthServices _authServices;
		private readonly IConfiguration _configuration;
		private readonly string _Username = String.Empty;
		private readonly string _Password = String.Empty;
		public AuthController(IAuthServices authServices, IConfiguration configuration)
		{
			_authServices = authServices;
			_configuration = configuration;
			_Username = _configuration.GetValue<string>("Username_VIETQR");
			_Password = _configuration.GetValue<string>("Password_VIETQR");
		}

		[HttpPost]
		[Route("api/token_generate")]
		public async Task<IActionResult> Login()
		{
			var result = await _authServices.Auth(_Username, _Password);
			return Ok(result);
		}
		[HttpPost]
		[Route("vqr/api/token_generate")]
		public async Task<IActionResult> LoginVQR()
		{
			var result = await _authServices.Auth(_Username, _Password);
			return Ok(result);
		}

	}
}
