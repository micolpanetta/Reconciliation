using Newtonsoft.Json;
using System.Linq;

namespace Reconciliation
{
    internal class PaymentRepository
    {
        private String filePath = Environment.GetEnvironmentVariable("FILESPATH") + "\\Payments.json";
        private List<Payment> payments = new List<Payment>();

        public PaymentRepository()
        {
            handlePaymentsFile();//TODO rename
        }

        private void handlePaymentsFile()
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                payments = JsonConvert.DeserializeObject<List<Payment>>(json);
            }
            //payments.ForEach(Console.WriteLine);
        }

        internal decimal GetAmountByCustomerYearMonth(string Customer, int Year, int Month)
        {
            //Console.WriteLine(Customer + " " + Year + " " + Month);

            Payment payment = payments.Find(payment => payment.CustomerId == Customer && payment.Year == Year && payment.Month == Month);

            return payment != null ? payment.Amount : 0;

        }

        internal List<string> GetAllCustomersIds()
        {
           return payments.Select(payment => payment.CustomerId).ToList();  
        }
    }
}
