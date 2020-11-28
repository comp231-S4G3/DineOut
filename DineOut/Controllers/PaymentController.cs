using DineOut.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;

namespace DineOut.Controllers
{
    public class PaymentController : Controller
    {

        int TotalPrice { get; set; }
        public object ClientScript { get; private set; }

        string error;

        public IActionResult Index(int totalPrice)
        {

            ViewBag.Greeting = (totalPrice.ToString("C"));

            TempData["totalCost"] = totalPrice;

            return View();
        }

        private int getAmount()
        {
            return TotalPrice;
        }

        [HttpGet]
        public ViewResult makePayment()
        {
            return View();
        }
        [HttpPost]

        public ViewResult makePayment(PaymentInformation cardInfo)
        {
            try
            {
                //cardInfo.Amount = totalPrice.ToString();

                Stripe.StripeConfiguration.SetApiKey("sk_test_51HqI05B1UsJ4lZg1agboQSE7i0fWn98619xc2FP5NhREH4igqo1AlKTQO8VWMfsQBUs1OlXNBzBkOqORRQP6ZlPf00E2l0QVhL");


                //Create Card Object to create Token  
                Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();
                card.Name = cardInfo.CardOwnerFirstName + " " + cardInfo.CardOwnerLastName;
                card.Number = cardInfo.CardNumber;
                card.ExpYear = cardInfo.ExpirationYear;
                card.ExpMonth = cardInfo.ExpirationMonth;
                card.Cvc = cardInfo.CVV2;

                Console.WriteLine(TotalPrice.ToString());

                //Assign Card to Token Object and create Token  
                Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
                token.Card = card;
                Stripe.TokenService serviceToken = new Stripe.TokenService();
                Stripe.Token newToken = serviceToken.Create(token);

                Stripe.CustomerCreateOptions myCustomer = new Stripe.CustomerCreateOptions();
                myCustomer.Email = cardInfo.Buyer_Email;
                myCustomer.SourceToken = newToken.Id;
                var customerService = new Stripe.CustomerService();
                Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

                var t = TempData["totalCost"];
                string s = t.ToString() + "00";
                //Create Charge Object with details of Charge  
                // System.Diagnostics.Debug.WriteLine(s.ToString());
                var options = new Stripe.ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(s),
                    Currency = "USD",
                    ReceiptEmail = cardInfo.Buyer_Email,
                    CustomerId = stripeCustomer.Id,

                };

                // System.Diagnostics.Debug.WriteLine(50);
                //and Create Method of this object is doing the payment execution.  
                var service = new Stripe.ChargeService();
                Stripe.Charge charge = service.Create(options); // This will do the Payment  


                return View("Thanks");
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        //error = ("Code: " + e.StripeError.Code + "; ");
                        error = (" Error Message: " + e.StripeError.Message);
                        break;
                    case "api_connection_error":
                        break;
                    case "api_error":
                        break;
                    case "authentication_error":
                        break;
                    case "invalid_request_error":
                        break;
                    case "rate_limit_error":
                        break;
                    case "validation_error":
                        break;
                    default:
                        // Unknown Error Type
                        break;
                }
                ViewBag.Greeting = error;
                return View("Error");

            }
        }

        public ViewResult Error()
        {




            return View();
        }


    }
}