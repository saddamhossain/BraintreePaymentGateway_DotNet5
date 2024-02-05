using Braintree;
using BraintreePaymentGateway_DotNet5.Interface;
using BraintreePaymentGateway_DotNet5.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BraintreePaymentGateway_DotNet5.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBook _bookService;
        private readonly IBraintree _braintreeService;

        public CheckoutController(IBook bookService, IBraintree braintreeService)
        {
            _bookService = bookService;
            _braintreeService = braintreeService;
        }

        [HttpGet]
        public IActionResult Purchase(Guid id)
        {
            var book = _bookService.GetById(id);
            if(book == null)
            {
                return NotFound();
            }

            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();
            ViewBag.clientToken = clientToken;

            var result = new BookPurchaseViewModel
            {
               Id = book.Id,
               Description = book.Description,
               Author = book.Author,
               Thumbnail = book.Thumbnail,
               Title = book.Title,
               Price = book.Price,
               Nonce = ""
            };

            return View(result);
        }

     
        public IActionResult Create(BookPurchaseViewModel model)
        {
            var gateway = _braintreeService.GetGateway();
            var book = _bookService.GetById(model.Id);

            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(book.Price),
                PaymentMethodNonce = model.Nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true,
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            if (result.IsSuccess())
            {
                return View("Success");
            }
            else
            {
                return View("Failure");
            }
        }

   
        public IActionResult BraintreePlans()
        {
            var gateway = _braintreeService.GetGateway();
            var plans = gateway.Plan.All().ToList();
            return View(plans);
        }

        public IActionResult SubscribeToPlan(string id)
        {
            var gateway = _braintreeService.GetGateway();

            var subscriptionRequest = new SubscriptionRequest()
            {
                PaymentMethodToken = "my-payment-token-value",
                PlanId = id
            };

            Result<Subscription> result = gateway.Subscription.Create(subscriptionRequest);
            if (result.IsSuccess())
            {
                return View("Subscribed");
            }
            else
            {
                return View("NotSubscribed");
            }
        }


    }
}
