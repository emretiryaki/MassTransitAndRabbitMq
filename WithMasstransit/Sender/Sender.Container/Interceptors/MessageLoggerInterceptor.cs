
using System.Linq;
using Castle.DynamicProxy;
using Core;
using Newtonsoft.Json;
using Sender.Contract;

namespace Sender.Container.Interceptors
{
    public class MessageLoggerInterceptor : IInterceptor
    {
        private readonly ILogger _logger;


        public MessageLoggerInterceptor(ILogger logger)
        {
            this._logger = logger;
        }

        public void Log(string message)
        {
            _logger.Log("SenderServiceLogger", "Thread:" + System.Threading.Thread.CurrentThread.ManagedThreadId + " " + message, LogType.Information);
        }

        public void Intercept(IInvocation invocation)
        {
            RequestBase requestBase = null;
            Log("RequestIncoming");
            try
            {
                if (invocation.Arguments.Any())
                {
                    Log("method:" + invocation.Method.Name + "arg count : " + invocation.Arguments[0].GetType());

                    requestBase = invocation.Arguments[0] as RequestBase;
                    if (requestBase != null)
                    {
                        Log("1");
                        requestBase.TrackId = System.Guid.NewGuid();
                        Log("2");
                        Log(string.Format("TrackId:{0} \n Method:{1}  \n Request : {2}", requestBase.TrackId, invocation.Method.Name, JsonConvert.SerializeObject(invocation.Arguments[0])));
                        Log("3");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log(ex.ToString());
            }
            Log("4");
            invocation.Proceed();
            Log("5");
            try
            {
                Log("6");
                if (requestBase != null && invocation.ReturnValue != null && requestBase.TrackId != null && invocation.Method.Name != null)
                    Log(string.Format("TrackId:{0} \n  Method:{1} \nResponse : {2}", requestBase.TrackId, invocation.Method.Name, JsonConvert.SerializeObject(invocation.ReturnValue)));

                Log("7");
            }
            catch (System.Exception ex)
            {
                Log(ex.ToString());
            }
            Log("RequestEnd");


        }
    }
}