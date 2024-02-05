using BraintreePaymentGateway_DotNet5.Models;

namespace BraintreePaymentGateway_DotNet5.ViewModel
{
    public class BookPurchaseViewModel : Book
    {
        public string Nonce { get; set; }
    }
}
