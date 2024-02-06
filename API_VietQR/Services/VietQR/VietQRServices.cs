using API_VietQR.Databases.MUADI_LOG;
using API_VietQR.Databases.UnitOfWork;
using API_VietQR.Request;
using API_VietQR.Response;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace API_VietQR.Services.VietQR
{
    public interface IVietQRServices
    {
        Task<TransactionSyncReponse> TransactionSync(int agentId, TransactionSyncRequest request, CancellationToken cancellationToken);

        Task<TransactionCallbackTest> TransactionTest(int agentId, TransactionTestRequest request, CancellationToken cancellationToken);

	}
    public class VietQRServices : IVietQRServices
    {
        private readonly IUnitOfWork _unitOfWork;
        public VietQRServices(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}
        public async Task<TransactionSyncReponse> TransactionSync(int agentId, TransactionSyncRequest request, CancellationToken cancellationToken)
        {
			string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/LogRequest/Tran_" + Guid.NewGuid() + ".txt";
			//using (StreamWriter writer = new StreamWriter(fullPath))
			//{
			//	//writer.WriteLine(JsonConvert.SerializeObject(request));
			//}
			using var db = _unitOfWork.Connection_LOG();
            Agent_VietQR_CallBack objAgentCallBack = new Agent_VietQR_CallBack();
            objAgentCallBack.AgentID = agentId;
            objAgentCallBack.RefTransactionId = Guid.NewGuid().ToString();
            objAgentCallBack.TransactionId = request.transactionid;
            objAgentCallBack.TransactionTime = request.transactiontime;
            objAgentCallBack.ReferenceNumber = request.referencenumber;
            objAgentCallBack.Amount = request.amount;
            objAgentCallBack.MessageContent = request.content;
            objAgentCallBack.BankAccount = request.bankaccount;
            objAgentCallBack.TransType = request.transType;
            objAgentCallBack.OrderId = request.orderId;
            objAgentCallBack.CreateDate = DateTime.Now;
            objAgentCallBack.Status = 0;
            db.Insert<Agent_VietQR_CallBack>(objAgentCallBack);




            TransactionSyncReponse objResult = new TransactionSyncReponse();
            objResult.error = false;
            objResult.errorReason = "000";
            objResult.toastMessage = "";
            objResult.@object = new Response.Object();
			objResult.@object.reftransactionid = objAgentCallBack.RefTransactionId; 
			return objResult;
		}
        public async Task<TransactionCallbackTest> TransactionTest(int agentId, TransactionTestRequest request, CancellationToken cancellationToken)
        {
			//string fullPath = AppDomain.CurrentDomain.BaseDirectory + "/LogRequest/Tran_Test_" + Guid.NewGuid() + ".txt";
			//using (StreamWriter writer = new StreamWriter(fullPath))
			//{
			//	writer.WriteLine(JsonConvert.SerializeObject(request));
			//}
			TransactionCallbackTest objResult = new TransactionCallbackTest();
            objResult.Status = "SUCCESS";
            objResult.Message = "";
            return objResult;

		}
	}
}
