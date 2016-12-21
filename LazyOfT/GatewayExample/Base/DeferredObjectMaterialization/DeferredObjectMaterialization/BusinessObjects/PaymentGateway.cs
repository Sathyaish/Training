using DeferredObjectMaterialization.ViewModels;
using System;
using System.Collections.Specialized;
using System.Net;

namespace DeferredObjectMaterialization.BusinessObjects
{
    public abstract class PaymentGateway : IDisposable
    {
        protected WebClient _client;

        public PaymentGateway()
        {
            // set up HTTP Client
            _client = new WebClient();
        }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        public string AppId { get; set; }

        public string AppKey { get; set; }

        public PaymentResult Pay(OrderInfo orderInfo)
        {
            PaymentResult result = new PaymentResult();

            try
            {
                var data = PrepareRequest();

                if (data != null) _client.UploadValues(Uri, "POST", data);

                result.Successful = true;
            }
            catch(Exception ex)
            {
                result.FailureReason = ex.Message;
            }

            return result;
        }

        protected abstract NameValueCollection PrepareRequest();

        public virtual void Dispose()
        {
            _client.Dispose();
        }
    }
}