using DeferredObjectMaterialization.Models;
using System;
using System.Linq;

namespace DeferredObjectMaterialization.BusinessObjects
{
    public static class PaymentGatewayFactory
    {
        public static PaymentGateway Create(int paymentGatewayId)
        {
            // read payment gateway info from the db

            using (var context = new DOMEntities())
            {
                var paymentGateway = context.PaymentGateways.SingleOrDefault(g => g.Id == paymentGatewayId);

                // some logic to return the right type of payment gateway
                // get payment gateway class name
                var paymentGatewayTypeName = GetPaymentGatewayTypeName(paymentGateway);

                var paymentGatewayType = Type.GetType(paymentGatewayTypeName);

                var gateway = Activator.CreateInstance(paymentGatewayType) as PaymentGateway;

                gateway.Name = paymentGateway.Name;
                gateway.Uri = new Uri(paymentGateway.Uri);
                gateway.AppId = paymentGateway.AppId;
                gateway.AppKey = paymentGateway.AppKey;

                return gateway;
            }
        }

        private static string GetPaymentGatewayTypeName(Models.PaymentGateway gateway)
        {
            return string.Concat("DeferredObjectMaterialization.BusinessObjects.", gateway.Name.Replace(" ", string.Empty));
        }
    }
}