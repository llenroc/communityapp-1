using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReady.Helpers.ServiceCallers
{
    public class ServiceResponse
    {
        public ServiceResponse() { }
        public bool IsSuccess { get; set; }
        public string response { get; set; }
        public ServiceError Error { get; set; }
    }
    public class ServiceError
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
    public class ServiceProxy
    {
        public static async Task<ServiceResponse> CallService(string serviceMethod, string inputData)
        {
            if (TechReady.Helpers.NetworkHelper.NetworkHelper.IsNetworkAvailable())
            {
                ServiceCaller sc = new ServiceCaller();
                string serviceUrl = string.Format(CommonSettings.ServicePath, serviceMethod);

                string response = string.Empty;

                var result = await sc.CallService(serviceUrl, inputData);

                if (result.IsSuccess)
                {
                    ServiceResponse sr = new ServiceResponse();
                    sr.IsSuccess = true;
                    sr.response = result.Message;
                    sr.Error = null;
                    return sr;
                }
                else
                {
                    ServiceResponse sr = new ServiceResponse();
                    sr.IsSuccess = false;
                    sr.response = null;
                    sr.Error = new ServiceError() { Message = result.Message };
                    return sr;
                }
            }
            else
            {
                ServiceResponse sr = new ServiceResponse();
                sr.IsSuccess = false;
                sr.response = null;
                sr.Error = new ServiceError() { Message = "No Network available" };
                return sr;
            }
        }
    }
}
