using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.IO;
using System.Text;
using API_NganLuong;

namespace checkoutv3
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string payment_method = Request.Form["option_payment"];
            string str_bankcode = Request.Form["bankcode"];
           
            	
            RequestInfo info = new RequestInfo();
            info.Merchant_id = "36680";
            info.Merchant_password = "matkhauketnoi";
            info.Receiver_email = "demo@nganluong.vn";

            

            info.cur_code = "vnd";
            info.bank_code = str_bankcode;
            
            info.Order_code = "ma_don_hang01";
            info.Total_amount = "10000";
            info.fee_shipping = "0";
            info.Discount_amount = "0";
            info.order_description = "Thanh toan tes thu dong hang";
            info.return_url = "http://localhost";
            info.cancel_url = "http://localhost";

            info.Buyer_fullname = buyer_fullname.Value;
            info.Buyer_email = buyer_email.Value;
            info.Buyer_mobile = buyer_mobile.Value;
            
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseInfo result = objNLChecout.GetUrlCheckout(info, payment_method);

            if (result.Error_code == "00")
            {
                Response.Redirect(result.Checkout_url);
            }else
                txtserverkt.InnerHtml = result.Description;


        }
    }
}