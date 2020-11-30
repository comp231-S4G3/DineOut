using DineOut.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;

namespace DineOut.Controllers
{
    public class PaymentController : Controller
    {

        double TotalPrice { get; set; }
        public object ClientScript { get; private set; }

        string error;

        public IActionResult Index(double totalPrice)
        {

            ViewBag.Greeting = (totalPrice.ToString("C"));

            TempData["totalCost"] = Convert.ToString(totalPrice);

            return View();
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


                Stripe.StripeConfiguration.SetApiKey("sk_test_51HqI05B1UsJ4lZg1agboQSE7i0fWn98619xc2FP5NhREH4igqo1AlKTQO8VWMfsQBUs1OlXNBzBkOqORRQP6ZlPf00E2l0QVhL");



                Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();
                card.Name = cardInfo.CardOwnerFirstName + " " + cardInfo.CardOwnerLastName;
                card.Number = cardInfo.CardNumber;
                card.ExpYear = cardInfo.ExpirationYear;
                card.ExpMonth = cardInfo.ExpirationMonth;
                card.Cvc = cardInfo.CVV2;

                Console.WriteLine(TotalPrice.ToString());


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
               

                int t1 = (int)Math.Round(Convert.ToDouble(t)) - 1;
                string total;
                string input_decimal_number = t.ToString();

                var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                if (regex.IsMatch(input_decimal_number))
                {
                    string decimal_places = regex.Match(input_decimal_number).Value;
                    total = t1.ToString() + decimal_places;
                }
                else
                {
                    total = t1 + "00";
                }


                System.Diagnostics.Trace.WriteLine(t1.ToString());


                var options = new Stripe.ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(total),
                    Currency = "USD",
                    ReceiptEmail = cardInfo.Buyer_Email,
                    CustomerId = stripeCustomer.Id,

                };



                var service = new Stripe.ChargeService();
                Stripe.Charge charge = service.Create(options);


                return View("Thanks");
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":

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
        public IActionResult CancelOrder()
        {
            /// the action that cancels the current order and returns the customer to the customer index page
            /// @param int orderId



            return RedirectToAction("Index", "Customer");
        }

    }
}