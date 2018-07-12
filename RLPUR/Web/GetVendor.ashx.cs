using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RLPUR.Models;

namespace RLPUR.Web
{
    /// <summary>
    /// 获取供应商信息
    /// </summary>
    public class GetVendor : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            context.Response.ContentType = "application/json";

            using (PurProvider purProvider = new PurProvider())
            {
                string venNo = context.Request.Form["VendorNo"];

                var vendor = purProvider.GetVendor(venNo);
                if (vendor == null)
                {
                    context.Response.Write("{\"success\":false,\"message\":\"" + "厂商代码不存在，或者该厂商已失效!" + "\"}");
                }
                else
                {
                    context.Response.Write("{\"success\":true,\"vendorName\":\"" + vendor["avnam"].ToString() + "\",\"curr\":\"" + vendor["avcur"].ToString() + "\"}");
                }

                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}