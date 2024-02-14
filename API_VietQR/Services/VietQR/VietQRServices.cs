using API_VietQR.Databases.Booking;
using API_VietQR.Databases.MUADI_LOG;
using API_VietQR.Databases.UnitOfWork;
using API_VietQR.Request;
using API_VietQR.Response;
using Dapper;
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
          

            if (request.content.ToUpper().IndexOf("TOPUP") >=0)
            {
                var array = request.content.ToUpper().Split(' ');
                if (array.Length > 0)
                {
                    var objMemberID = db.QueryFirst<long>("select ID from tbl_ThanhVien where Username=@Username and AgentID=@AgentID", new
                    {
                        AgentID = agentId,
                        Username = "AutoDeposit"
                    });
                    if (objMemberID  > 0)
                    {
                        var objSubAgent = db.QueryFirst<SubAgents>("select * from tbl_SubAgent where ID=@ID and ParentAgent=@AgentID",
                        new { AgentID = agentId, ID = array[0] });
                        if (objSubAgent != null)
                        {
                            var DocType = "T";
                            var objDocNo = db.QueryFirstOrDefault<string>("Select [dbo].[GET_DOC_NO](@DocType,@AgentID,@SubAgentID)",
                                new { DocType = DocType, AgentID = agentId, SubAgentID = objSubAgent.ID });
                            if (!String.IsNullOrEmpty(objDocNo))
                            {
                                CN_NhatKys objNhatKy = new CN_NhatKys();
                                objNhatKy.AgentID = agentId;
                                objNhatKy.Amount_NT = request.amount;
                                objNhatKy.Amount_VND = request.amount;
                                objNhatKy.Doc_Date = DateTime.Now;
                                objNhatKy.Doc_No = objDocNo;
                                objNhatKy.Doc_Type = DocType;
                                objNhatKy.Doc_Title = objDocNo;
                                objNhatKy.MemberID = objMemberID;
                                objNhatKy.ROE = 1;
                                objNhatKy.SubAgent = objSubAgent.ID;
                                objNhatKy.OtherFee = 0;
                                objNhatKy.AccType = "";
                                db.Insert<CN_NhatKys>(objNhatKy);
                                objAgentCallBack.Status = 1;

                            }

                        }
                    }
                    
                }
                
            }
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
