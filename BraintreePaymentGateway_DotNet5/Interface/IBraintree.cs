using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraintreePaymentGateway_DotNet5.Interface
{
    public interface IBraintree
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
    }
}
