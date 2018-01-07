using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace GenericPayment.TestApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult OrderDetails(string invoiceno, string paykey, string hashkey, string gateway)
        {
            string result = "";

            var sampleData = new GenericPayments();
            sampleData.InvoiceNo = invoiceno;
            sampleData.PayeeInfos = new List<Payee>();
            var payee1 = new Payee();
            sampleData.PayeeInfos.Add(payee1);
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
            sampleData.PayeeInfos.Add(payee2);
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

            result = JsonConvert.SerializeObject(sampleData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }

    public class GenericPayments
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MarketplaceUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Payee> PayeeInfos { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string InvoiceNo { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PayKey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Total { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Gateway { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Hashkey { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? AgreedDateTime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Note { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ProviderTransRefId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PaymentStatus { get; set; }
    }

    public class Payee
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Total { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Item> Items { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }
    }

    public class Item
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal? Price { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Quantity { get; set; }
    }
}
