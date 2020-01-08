using System.Linq;
using Checkout.Repository.Models;
using LiteDB;

namespace Checkout.Repository.LiteDb
{
    public class LiteDatabaseWrapper : ILiteDatabaseWrapper
    {
        public void Insert(PaymentInformation payment)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var payments = db.GetCollection<PaymentInformation>("payments");
                payments.Insert(payment);

                payments.EnsureIndex(x => x.Id);
            }
        }

        public PaymentInformation Get(RetrievePaymentRequest paymentRequest)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var payments = db.GetCollection<PaymentInformation>("payments");
                
                return payments.Find(x => x.Id ==paymentRequest.PaymentId && x.MerchantDetails.MerchantId == paymentRequest.MerchantId).FirstOrDefault();
            }
        }
    }
}