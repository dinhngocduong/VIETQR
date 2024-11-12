using API_VietQR.Databases.Booking;
using API_VietQR.Databases.MUADI_LOG;
using API_VietQR.Databases.UnitOfWork;
using API_VietQR.Request;
using API_VietQR.Response;
using API_VietQR.Utilities;
using Dapper;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pipelines.Sockets.Unofficial.Arenas;
using System.IO;

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
		private readonly IServiceHelper _serviceHelper;
		private readonly IConfiguration _configuration;
		private readonly string URL_PAYMENT_CALLBACK = "";
        public VietQRServices(IUnitOfWork unitOfWork, IServiceHelper serviceHelper, IConfiguration configuration)
        {
			_unitOfWork = unitOfWork;
			_serviceHelper = serviceHelper;
			_configuration = configuration;
			URL_PAYMENT_CALLBACK = _configuration.GetValue<string>("URL_PAYMENT_CALLBACK");
		}
        public async Task<TransactionSyncReponse> TransactionSync(int agentId, TransactionSyncRequest request, CancellationToken cancellationToken)
        {		
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
          

            if (request.content.ToUpper().IndexOf("TOP UP") >= 0 || request.content.ToUpper().IndexOf("TOPUP") >= 0)
            {				
				long SubAgentID = 0;
				var SubAgentCode = "";
				decimal TotalAmountUseFee = 0;
				using var dbBooking = _unitOfWork.ConnectionBooking();
				var objAgentVietQR = dbBooking.QueryFirstOrDefault<Agent_VietQR>("select * from tbl_Agent_VietQR where BankAccount =@BankAccount", new
				{
					BankAccount = request.bankaccount
				});
				if (objAgentVietQR != null)
				{
					TotalAmountUseFee = objAgentVietQR.AmountUseFee;
					agentId = Convert.ToInt32(objAgentVietQR.AgentID);
					var objThanhVien = dbBooking.QueryFirstOrDefault<ThanhViens>("select ID from tbl_ThanhVien where Username=@Username and AgentID=@AgentID", new
					{
						AgentID = agentId,
						Username = "AutoDeposit"
					});
					if (objThanhVien.ID > 0)
					{
						string sSQL = "";
						var dataContent = request.content.ToUpper();
						dataContent = dataContent.Replace("MB "+ request.bankaccount, "");
						if (dataContent.IndexOf("ENDTOPUP") >=0)
						{
							var TmpSubAgent = Utils.SplitSubString("TOPUP", "ENDTOPUP", dataContent).Trim().TrimStart().TrimEnd();
							var array = TmpSubAgent.Split(' ');
							if (array.Length >= 1)
							{
								var TmpSubAgentID = array[0];
								if (Utilities.ValidationUtility.IsNumeric(TmpSubAgentID))
								{
									SubAgentID = Convert.ToInt64(TmpSubAgentID);
									sSQL = "select * from tbl_SubAgent where ID=@ID and ParentAgent=@AgentID";
								}
							}
							if (String.IsNullOrEmpty(sSQL))
							{
								var TmpSubAgentID = "";
								char[] charArr = TmpSubAgent.ToCharArray();
								foreach (var item in charArr)
								{
									if (Utilities.ValidationUtility.IsNumeric(item.ToString()))
									{
										TmpSubAgentID += item.ToString();
									}
									else
									{
										break;
									}
								}
								if (Utilities.ValidationUtility.IsNumeric(TmpSubAgentID))
								{
									SubAgentID = Convert.ToInt64(TmpSubAgentID);
									sSQL = "select * from tbl_SubAgent where ID=@ID and ParentAgent=@AgentID";
								}
							}
						}
						else
						{
							dataContent = dataContent.Replace("TOP UP", "!").Split('!')[0];
							if (dataContent.IndexOf(".") >= 0)
							{
								dataContent = dataContent.Substring(dataContent.LastIndexOf(".") + 1);
							}
							var array = dataContent.Split(' ');
							if (array.Length >= 2)
							{
								if (Utilities.ValidationUtility.IsNumeric(array[0]))
								{
									SubAgentID = Convert.ToInt64(array[0]);
									sSQL = "select * from tbl_SubAgent where ID=@ID and ParentAgent=@AgentID";
								}
								else
								{
									SubAgentCode = array[1];
									sSQL = "select * from tbl_SubAgent where AgentCode=@SubAgentCode and ParentAgent=@AgentID";
								}
							}
						}
						
						if (!String.IsNullOrEmpty(sSQL))
						{
							var objSubAgent = dbBooking.QueryFirstOrDefault<SubAgents>(sSQL,
						new { AgentID = agentId, ID = SubAgentID, SubAgentCode = SubAgentCode });
							if (objSubAgent != null)
							{
								var objAgentCallBackCheck = db.QueryFirstOrDefault<Agent_VietQR_CallBack>("select * from tbl_Agent_VietQR_CallBack where ReferenceNumber=@ReferenceNumber", new { ReferenceNumber = objAgentCallBack.ReferenceNumber });
								if (objAgentCallBackCheck == null)
								{
									var DocType = "T";
									var objDocNo = dbBooking.QueryFirstOrDefault<string>("Select [dbo].[GET_DOC_NO](@DocType,@AgentID,@SubAgentID)",
										new { DocType = DocType, AgentID = agentId, SubAgentID = objSubAgent.ID });
									if (!String.IsNullOrEmpty(objDocNo))
									{
										decimal Fee = 0;
										if (request.amount < TotalAmountUseFee)
										{
											Fee = objAgentVietQR.Fee;
										}
										CN_NhatKys objNhatKy = new CN_NhatKys();
										objNhatKy.AgentID = agentId;
										objNhatKy.Amount_NT = request.amount - Fee;
										objNhatKy.Amount_VND = request.amount - Fee;
										objNhatKy.Doc_Date = DateTime.Now;
										objNhatKy.Doc_No = objDocNo;
										objNhatKy.Doc_Type = DocType;
										objNhatKy.Doc_Title = objDocNo;
										objNhatKy.MemberID = objThanhVien.ID;
										objNhatKy.ROE = 1;
										objNhatKy.SubAgent = objSubAgent.ID;
										objNhatKy.OtherFee = 0;
										objNhatKy.AccType = "VN-BL-VJ";
										dbBooking.Insert<CN_NhatKys>(objNhatKy);
										objAgentCallBack.Status = 1;

									}
								}


							}
						}
						
						
					}	
					
				}	
            }
			else
			{
				#region Process Pay B2C
				using var dbBooking = _unitOfWork.ConnectionBooking();
				var objAgentVietQR = dbBooking.QueryFirstOrDefault<Agent_VietQR>("select * from tbl_Agent_VietQR where BankAccount =@BankAccount", new
				{
					BankAccount = request.bankaccount
				});
				if (objAgentVietQR != null)
				{
					agentId = Convert.ToInt32(objAgentVietQR.AgentID);
				}	
				var payCode = request.content.ToUpper();
				var objPayment = db.QueryFirstOrDefault<Agent_Payment_B2C>("select * from tbl_Agent_Payment_B2C where PayCode = @PayCode and AgentID=@AgentID", new
				{
					PayCode = payCode,
					AgentID = agentId
				});
				if (objPayment != null && objPayment.PayStatus_R != "PAID")
				{
					objPayment.PayStatus_R = "PAID";
					objPayment.PayCreateDate_R = DateTime.Now;
					objPayment.PayTotal_R = request.amount;
					objPayment.PayTransaction_R = request.transactionid;
					db.Update(objPayment);

					var listHeader = new List<HeaderAPIRequest>();
					listHeader.Add(new HeaderAPIRequest { Key = "AgentID", Value = agentId.ToString() });
					listHeader.Add(new HeaderAPIRequest { Key = "MemberID", Value = objPayment.MemberID.ToString() });
					var url = $"{URL_PAYMENT_CALLBACK}/embedded/{objPayment.ID.ToString()}";
					_ =	Task.Run( () => _serviceHelper.SendToOtherService(HttpMethod.Get, url, "application/json", "", listHeader, cancellationToken));
				}
				
				#endregion
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
