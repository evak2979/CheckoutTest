using System.Linq;
using Checkout.Repository.Models;
using LiteDB;

namespace Checkout.Repository.LiteDb
{
    public class LiteDatabaseWrapper : ILiteDatabaseWrapper
    {
        public void Insert(Payment payment)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var payments = db.GetCollection<Payment>("payments");
                payments.Insert(payment);

                payments.EnsureIndex(x => x.Id);
            }
        }

        public Payment Get(RetrievePaymentRequest paymentRequest)
        {
            using (var db = new LiteDatabase(@"MyData.db"))
            {
                var payments = db.GetCollection<Payment>("payments");
                
                return payments.Find(x => x.Id ==paymentRequest.PaymentId && x.MerchantDetails.Id == paymentRequest.VendorId).FirstOrDefault();
            }
        }
    }
}