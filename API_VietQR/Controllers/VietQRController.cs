using API_VietQR.Constant;
using API_VietQR.Request;
using API_VietQR.Services.VietQR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_VietQR.Controllers
{
	
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class VietQRController : ControllerBase
	{
		private readonly IVietQRServices _vietQRServices;
		public VietQRController(IVietQRServices vietQRServices)
		{
			_vietQRServices = vietQRServices;
		}

		[HttpPost]
		[Route("api/bank/api/transaction-sync")]
		public async Task<IActionResult> TransactionSync(TransactionSyncRequest request, CancellationToken cancellationToken)
		{
			var agentIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(Constants.AgentIdClaim));
			if (agentIdClaim == null)
			{
				return Forbid();
			}
			var agentId = int.Parse(agentIdClaim.Value);			
			var result = await _vietQRServices.TransactionSync(agentId, request, cancellationToken);
			return Ok(result);
		}
				
		[HttpPost]
		[Route("vqr/bank/api/test/transaction-callback")]
		public async Task<IActionResult> TransactionTest(TransactionTestRequest request, CancellationToken cancellationToken)
		{
			var agentIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(Constants.AgentIdClaim));
			if (agentIdClaim == null)
			{
				return Forbid();
			}
			var agentId = int.Parse(agentIdClaim.Value);
			var result = await _vietQRServices.TransactionTest(agentId, request, cancellationToken);
			return Ok(result);
		}


		
		[HttpPost]
		[Route("vqr/api/bank/api/transaction-sync")]
		public async Task<IActionResult> TransactionSyncVQR(TransactionSyncRequest request, CancellationToken cancellationToken)
		{
			var agentIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(Constants.AgentIdClaim));
			if (agentIdClaim == null)
			{
				return Forbid();
			}
			var agentId = int.Parse(agentIdClaim.Value);
			var result = await _vietQRServices.TransactionSync(agentId, request, cancellationToken);
			return Ok(result);
		}

		[HttpPost]
		[Route("vqr/bank/api/transaction-sync")]
		public async Task<IActionResult> TransactionSyncVQRTest(TransactionSyncRequest request, CancellationToken cancellationToken)
		{
			var agentIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(Constants.AgentIdClaim));
			if (agentIdClaim == null)
			{
				return Forbid();
			}
			var agentId = int.Parse(agentIdClaim.Value);
			var result = await _vietQRServices.TransactionSync(agentId, request, cancellationToken);
			return Ok(result);
		}

	}
}
