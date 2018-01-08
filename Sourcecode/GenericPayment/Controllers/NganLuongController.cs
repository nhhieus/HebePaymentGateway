using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using GenericPayment.Database;
using GenericPayment.Models;
using GenericPayment.Utilities;
using Newtonsoft.Json;
using PayPal.Api;
using Item = GenericPayment.Models.Item;
using Payee = GenericPayment.Models.Payee;
using Payer = GenericPayment.Models.Payer;

namespace GenericPayment.Controllers
{
    public enum ReturnType
    {
        Success = 0,
        Cancel = 1,
        Timeout = 2
    }

    public class NganLuongController : Controller
    {
        public async Task<ActionResult> Index(string paykey)
        {
            var marketplaceUrl = GetMarketPlaceUrl();
            var db = new DbContext();
            try
            {
                var details = db.GetDetails(paykey);
                if (details != null)
                {
                    // Update details for the valid record
                    var vm = new PaymentDetails();
                    vm.CashKey = details.PayKey;
                    vm.Currency = details.Currency;

                    decimal total;
                    if (!decimal.TryParse(details.Total, out total))
                    {
                        total = 0m;
                    }
                    vm.Total = total;

                    using (var httpClient = new HttpClient())
                    {
                        var url = marketplaceUrl + "/user/checkout/order-details" + "?gateway=" + details.Gateway + "&invoiceNo=" + details.InvoiceNo + "&paykey=" + paykey + "&hashkey=" + details.Hashkey;
                        HttpResponseMessage tokenResponse = await httpClient.GetAsync(url);
                        tokenResponse.EnsureSuccessStatusCode();
                        string text = await tokenResponse.Content.ReadAsStringAsync();

                        #region DEBUG - Remove dummy code below

                        var response = new GenericPayments();
                        response.InvoiceNo = "ABC";
                        response.PayeeInfos = new List<Payee>();
                        var payee1 = new Payee();
                        response.PayeeInfos.Add(payee1);
                        payee1.Total = 20.45M;
                        payee1.Currency = "VND";
                        payee1.Items = new List<Item>();
                        payee1.Items.Add(new Item()
                                         {
                                             Id = "3704 - 8417",
                                             Name = "Sample item",
                                             Description = "Sample description",
                                             Currency = "USD",
                                             Price = 99.99M,
                                             Quantity = 1
                                         });
                        payee1.Items.Add(new Item()
                                         {
                                             Id = "Admin Fee - Order 3704",
                                             Name = "Fee (deduected)",
                                             Description = "Admin fee",
                                             Currency = "USD",
                                             Price = 9.99M,
                                             Quantity = 1
                                         });

                        var payee2 = new Payee();
                        response.PayeeInfos.Add(payee2);
                        payee2.Total = 9.99M;
                        payee2.Currency = "VND";
                        payee2.Items = new List<Item>();
                        payee2.Items.Add(new Item()
                                         {
                                             Id = "ARCTICK - 1234567",
                                             Name = "Sample item",
                                             Description = "Sample description",
                                             Currency = "USD",
                                             Price = 9.99M,
                                             Quantity = 1
                                         });

                        #endregion

                        // Set the details to db
                        //var response = JsonConvert.DeserializeObject<GenericPayments>(text);
                        details.PayeeInfos = response.PayeeInfos;
                        details.MarketplaceUrl = marketplaceUrl;
                        db.SetDetails(paykey, details);
                    }

                    return View("Index", vm);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                Logger.GetInstance().Write(ex, string.Format("[Key={0}]Exception thrown in NganLuongController.Index", paykey));
            }

            return View("Error");
        }

        private string GetMarketPlaceUrl()
        {
            var marketplaceUrl = ConfigCode.GetInstance().MarketPlaceUrl;

            //DEBUG - Remove comment below 
            //if (Request.UrlReferrer != null)
            //{
            //    marketplaceUrl = Request.UrlReferrer.ToString();
            //}

            //var uri = new Uri(marketplaceUrl);
            //marketplaceUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Authority;

            return marketplaceUrl;
        }

        public JsonResult AgreeToPay(AgreementViewModel vm)
        {
            string redirectUrl = "";
            string errorMessage = "";
            var db = new DbContext();

            try
            {
                //DEBUG
                Logger.GetInstance().Write(JsonConvert.SerializeObject(vm));

                var details = db.GetDetails(vm.CashKey);
                if (details != null)
                {
                    details.AgreedDateTime = DateTime.UtcNow;

                    var info = new RequestInfo();
                    info.Merchant_id = ConfigCode.GetInstance().MerchantID;
                    info.Merchant_password = ConfigCode.GetInstance().MerchantPassword;
                    info.Receiver_email = ConfigCode.GetInstance().ReceiverEmail;

                    info.cur_code = "vnd";
                    info.Payment_method = vm.OptionPayment;
                    info.bank_code = vm.BankCode;
                    info.Order_code = details.InvoiceNo;
                    info.Total_amount = details.Total;
                    info.Buyer_fullname = vm.FullName;
                    info.Buyer_email = vm.Email;
                    info.Buyer_mobile = vm.Phone;

                    //Optional fields
                    info.fee_shipping = "0";
                    info.Discount_amount = "0";
                    info.order_description = "";
                    // End of optional fields

                    //string host = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Authority;
                    string host = "http://localhost/genericpayment";
                    string baseUrl = string.Format("{0}/nganluong/return?", host);
                    info.return_url = baseUrl + "key=" + vm.CashKey;
                    info.cancel_url = baseUrl + "key=" + vm.CashKey + "&type=" + ReturnType.Cancel.GetHashCode();
                    info.time_limit = baseUrl + "key=" + vm.CashKey + "&type=" + ReturnType.Timeout.GetHashCode();

                    //DEBUG
                    Logger.GetInstance().Write("RequestInfo");
                    Logger.GetInstance().Write(JsonConvert.SerializeObject(info));

                    var objApiCheckout = new APICheckoutV3();
                    ResponseInfo checkoutRs = objApiCheckout.GetUrlCheckout(info, vm.OptionPayment);

                    //DEBUG
                    Logger.GetInstance().Write("Check out response:");
                    Logger.GetInstance().Write(JsonConvert.SerializeObject(checkoutRs));

                    if (checkoutRs.Error_code == "00")
                    {
                        details.ProviderTransRefId = checkoutRs.Token;
                        bool result = db.SetDetails(vm.CashKey, details);
                        if (result)
                        {
                            redirectUrl = checkoutRs.Checkout_url;
                        }
                        else
                        {
                            errorMessage = "Something went wrong! Please try again later.";
                            Logger.GetInstance().Write(string.Format("[Key={0}] Check out succeeded but failed to update NL Token", vm.CashKey));
                        }
                    }
                    else
                    {
                        ;
                        errorMessage = string.Format("Error Description: {0}. <br/> Something went wrong! Please try again later.", checkoutRs.Description);
                        Logger.GetInstance().Write(string.Format("[Key={0}] Failed to check out(Code={1};Desc={2})", vm.CashKey, checkoutRs.Error_code, checkoutRs.Description));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.GetInstance().Write(ex, string.Format("[Key={0}]Exception thrown in AgreeToPay", vm.CashKey));
                ViewBag.ErrorMessage = ex.Message;
                redirectUrl = "";
            }

            // Use js to redirect to NL check out URL.
            return Json(new { result = redirectUrl, message = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CancelToPay(AgreementViewModel vm)
        {
            var url = DbContext.CancelUrl(vm.CashKey);
            return Json(new { result = url }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// NganLuong will return to this endpoint.
        /// </summary>
        /// <param name="token">return token if success</param>
        /// <param name="key"></param>
        /// <param name="type">0: success; 1: cancel; 2: timeout</param>
        /// <returns></returns>
        [ActionName("return")]
        public ActionResult PaymentReturn(string token, string key, int type = 0)
        {
            string result;

            //DEBUG
            Logger.GetInstance().Write("Payment Return URL:");
            Logger.GetInstance().Write(Request.Url.AbsoluteUri.ToString());

            if (type == ReturnType.Cancel.GetHashCode() || type == ReturnType.Timeout.GetHashCode() ||
                string.IsNullOrEmpty(key) || string.IsNullOrEmpty(token))
            {
                result = DbContext.CancelUrl(key);
                return Redirect(result);
            }

            var db = new DbContext();
            var details = db.GetDetails(key);
            if (details != null && details.ProviderTransRefId == token)
            {
                var info = new RequestCheckOrder();
                info.Merchant_id = ConfigCode.GetInstance().MerchantID;
                info.Merchant_password = ConfigCode.GetInstance().MerchantPassword;
                info.Token = token;
                var objApiCheckout = new APICheckoutV3();
                ResponseCheckOrder checkOrderRs = objApiCheckout.GetTransactionDetail(info);

                //DEBUG
                Logger.GetInstance().Write("Transaction details:");
                Logger.GetInstance().Write(JsonConvert.SerializeObject(checkOrderRs));

                switch (checkOrderRs.transactionStatus)
                {
                    case "00":
                        details.PaymentStatus = EnumPaymentStatus.Paid.GetHashCode();
                        break;
                    case "01":
                        details.PaymentStatus = EnumPaymentStatus.Pending.GetHashCode();
                        break;
                    case "02":
                        details.PaymentStatus = EnumPaymentStatus.Unpaid.GetHashCode();
                        break;
                }

                var updateRs = db.SetDetails(key, details);

                // Build the success url and redirect back to Arcadier
                result = DbContext.SuccessUrl(key, "");
                return Redirect(result);
            }
            else
            {
                // Build the failure url and redirect back to Arcadier
                result = DbContext.CancelUrl(key);
                return Redirect(result);
            }
        }
    }
}
