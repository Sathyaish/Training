using System.Collections.Specialized;

namespace DeferredObjectMaterialization.BusinessObjects
{
    public class PayPal : PaymentGateway
    {
        protected override NameValueCollection PrepareRequest()
        {
            return null;
        }
    }
}