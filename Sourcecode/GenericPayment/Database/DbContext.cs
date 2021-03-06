﻿using System;
using System.IO;
using GenericPayment.Models;
using GenericPayment.Utilities;
using Newtonsoft.Json;

namespace GenericPayment.Database
{
    public class DbContext
    {
        public GenericPayments GetDetails(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            try
            {
                string dbpath = string.Format("{0}/{1}.json", ConfigCode.GetInstance().DatabasePath, name);
                if (File.Exists(dbpath))
                {
                    string settingsValue = File.ReadAllText(dbpath);

                    return JsonConvert.DeserializeObject<GenericPayments>(settingsValue);
                }
            }
            catch (Exception exception)
            {
                Logger.GetInstance().Write(exception, "Exception thrown in GetDetails");
            }

            return null;

        }

        public bool SetDetails(string name, GenericPayments details)
        {
            if (string.IsNullOrEmpty(name)) return false;
            try
            {
                string dbpath = string.Format("{0}/{1}.json", ConfigCode.GetInstance().DatabasePath, name);
                string output = JsonConvert.SerializeObject(details, Newtonsoft.Json.Formatting.None);
                File.WriteAllText(dbpath, output);
                return true;
            }
            catch(Exception exception)
            {
                Logger.GetInstance().Write(exception, "Exception thrown in SetDetails");
            }

            return false;
        }

        public static string SuccessUrl(string key, string note)
        {
            var db = new DbContext();
            try
            {
                var details = db.GetDetails(key);
                if (details != null)
                {
                    details.AgreedDateTime = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(note))
                    {
                        details.Note = note;
                    }

                    bool result = db.SetDetails(key, details);
                    string url = details.MarketplaceUrl + "/user/checkout/payment-failure" +
                        "?gateway=" + details.Gateway +
                        "&invoiceNo=" + details.InvoiceNo +
                        "&paykey=" + details.PayKey +
                        "&hashkey=" + details.Hashkey;
                    if (result)
                    {
                        url = details.MarketplaceUrl + "/user/checkout/payment-success" +
                        "?gateway=" + details.Gateway +
                        "&invoiceNo=" + details.InvoiceNo +
                        "&paykey=" + details.PayKey +
                        "&hashkey=" + details.Hashkey;
                    }
                    return url;
                }
            }
            catch(Exception exception)
            {
                Logger.GetInstance().Write(exception, "Exception thrown in SuccessUrl");
            }
            
            return "";
        }
   
        public static string CancelUrl(string key)
        {
            var db = new DbContext();
            try
            {
                var details = db.GetDetails(key);
                if (details != null)
                {
                    string url = details.MarketplaceUrl + "/user/checkout/payment-failure" +
                        "?gateway=" + details.Gateway +
                        "&invoiceNo=" + details.InvoiceNo +
                        "&paykey=" + details.PayKey +
                        "&hashkey=" + details.Hashkey;
                    return url;
                }
            }
            catch(Exception exception)
            {
                Logger.GetInstance().Write(exception, "Exception thrown in CancelUrl");
            }

            return "";
        }
    }
}