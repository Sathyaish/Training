namespace DeferredObjectMaterialization.BusinessObjects
{
    public class PaymentResult
    {
        public bool Successful { get; internal set; }

        public string FailureReason { get; internal set; }
    }
}
