using Newtonsoft.Json;

namespace Reconciliation
{
    internal class PaymentRepository
    {
        private String filePath = Environment.GetEnvironmentVariable("FILESPATH") + "\\Payments.json";
        public PaymentRepository()
        {
            handlePaymentsFile();
        }

        private void handlePaymentsFile()
        {
            List<Payment> payments = new List<Payment>();
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                payments = JsonConvert.DeserializeObject<List<Payment>>(json);
            }
            //payments.ForEach(Console.WriteLine);
        }
    }
}
