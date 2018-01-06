using System;
using System.Web.Mvc;
using GenericPayment.Database;
using GenericPayment.Models;
using GenericPayment.Utilities;
using PayPal.Api;
using Payer = GenericPayment.Models.Payer;

namespace GenericPayment.Controllers
{
    public class NganLuongController : Controller
    {
        public ActionResult Index(string paykey)
        {
            var marketplaceUrl = GetMarketPlaceUrl();
            var db = new DbContext();
            try
            {
                var details = db.GetDetails(paykey);
                details = new GenericPayments();
                details.Currency = "USD";
                details.Gateway = "GATEWAY";
                details.Hashkey = "HashKey";
                details.InvoiceNo = "Invoice Number";
                details.PayKey = "123456";
                details.Total = "100.45";
                if (details != null)
                {
                    // Update details for the valid record
                    PaymentDetails vm = new PaymentDetails();
                    vm.CashKey = details.PayKey;
                    vm.Currency = details.Currency;
                    
                    decimal total;
                    if (!decimal.TryParse(details.Total, out total))
                    {
                        total = 0m;
                    }
                    vm.Total = total;
                    vm.Note = "";

                    // Call Arcadier api to get the details 
                    //using (var httpClient = new HttpClient())
                    //{
                    //    var url = marketplaceUrl + "/user/checkout/order-details" + "?gateway=" + details.Gateway + "&invoiceNo=" + details.InvoiceNo + "&paykey=" + paykey + "&hashkey=" + details.Hashkey;
                    //    HttpResponseMessage tokenResponse = await httpClient.GetAsync(url);
                    //    tokenResponse.EnsureSuccessStatusCode();
                    //    string text = await tokenResponse.Content.ReadAsStringAsync();

                    //    // Set the details to db
                    //    GenericPayments response = JsonConvert.DeserializeObject<GenericPayments>(text);
                    //    details.PayeeInfos = response.PayeeInfos;
                    //    details.MarketplaceUrl = marketplaceUrl;
                    //    db.SetDetails(paykey, details);
                    //}

                    return View("Index", vm);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View("Error");
        }

        private string GetMarketPlaceUrl()
        {
            var marketplaceUrl = ConfigCode.GetInstance().MarketPlaceUrl;
            if (Request.UrlReferrer != null)
            {
                marketplaceUrl = Request.UrlReferrer.ToString();
            }

            var uri = new Uri(marketplaceUrl);
            marketplaceUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Authority;

            return marketplaceUrl;
        }

        public JsonResult AgreeToPay(AgreementViewModel vm)
        {
            string ppurl = "";
            var db = new DbContext();
            try
            {
                var details = db.GetDetails(vm.CashKey);
                if (details != null)
                {
                    details.AgreedDateTime = DateTime.UtcNow;
                    details.Note = vm.Note;

                    var info = new RequestInfo();
                    info.Merchant_id = ConfigCode.GetInstance().MerchantID;
                    info.Merchant_password = ConfigCode.GetInstance().MerchantPassword;
                    info.Receiver_email = ConfigCode.GetInstance().ReceiverEmail;
                    
                    info.cur_code = "vnd";
                    info.bank_code = vm.BankCode;

                    info.Order_code = "ma_don_hang01";
                    info.Total_amount = "10000";
                    info.fee_shipping = "0";
                    info.Discount_amount = "0";
                    info.order_description = "Thanh toan tes thu dong hang";
                    info.return_url = "http://localhost";
                    info.cancel_url = "http://localhost";

                    info.Buyer_fullname = "";
                    info.Buyer_email = "";
                    info.Buyer_mobile = "";

                    var objNLChecout = new APICheckoutV3();
                    ResponseInfo result = objNLChecout.GetUrlCheckout(info, vm.OptionPayment);

                    if (result.Error_code == "00")
                    {
                        Response.Redirect(result.Checkout_url);
                    }
                    else
                    {
                        //txtserverkt.InnerHtml = result.Description;
                    }

                }

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                ppurl = "";
            }

            // Use js to redirect to PayPal with the received approval_url above
            return Json(new { result = ppurl }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelToPay(AgreementViewModel vm)
        {
            var url = DbContext.CancelUrl(vm.CashKey);
            return Json(new { result = url }, JsonRequestBehavior.AllowGet);
        }


    }
}
