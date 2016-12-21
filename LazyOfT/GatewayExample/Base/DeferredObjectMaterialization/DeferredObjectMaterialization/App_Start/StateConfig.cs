using b = DeferredObjectMaterialization.BusinessObjects;
using DeferredObjectMaterialization.Models;
using System.Collections.Generic;
using DeferredObjectMaterialization.BusinessObjects;
using System.Web;
using System;

namespace DeferredObjectMaterialization
{
    public class StateConfig
    {
        public static void LoadState()
        {
            LoadPaymentGateways();
        }

        private static void LoadPaymentGateways()
        {
            using (var profiler = new Profiler())
            {
                var dict = new Lazy<Dictionary<int, Lazy<b::PaymentGateway>>>(CreateProductPaymentGatewayDictionary);

                HttpContext.Current.Application["ProductPaymentGatewayDictionary"] = dict;
            }
        }

        private static Dictionary<int, Lazy<b::PaymentGateway>> CreateProductPaymentGatewayDictionary()
        {
            var dict = new Dictionary<int, Lazy<b::PaymentGateway>>();

            using (var context = new DOMEntities())
            {
                foreach (var ppg in context.ProductPaymentGateways)
                {
                    Func<b::PaymentGateway> func = () => PaymentGatewayFactory.Create(ppg.PaymentGatewayId);

                    dict.Add(ppg.ProductId, new Lazy<b::PaymentGateway>(func));
                }
            }

            return dict;
        }
    }
}