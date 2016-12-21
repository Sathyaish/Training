using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeferredObjectMaterialization.BusinessObjects
{
    public class MarineCommerce : PaymentGateway
    {
        protected override System.Collections.Specialized.NameValueCollection PrepareRequest()
        {
            return null;
        }
    }
}