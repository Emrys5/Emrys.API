using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http; 

namespace Emrys.API
{
    /// <summary>
    /// 用API的模式
    /// </summary>
    public class HomeAPIController : ApiController
    {

        public HttpResponseMessage Post()
        {
            // 设置返回对象
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK); 
            HttpRequestBase request = ((HttpContextBase)Request.Properties["MS_HttpContext"]).Request;
            // 获取Json参数
            byte[] byts = new byte[request.InputStream.Length];
            request.InputStream.Read(byts, 0, byts.Length);
            string parameters = System.Text.Encoding.UTF8.GetString(byts); 
            // 获取返回值
            string returnString = Main.Process(parameters);
            httpResponseMessage.Content = new StringContent(returnString, Encoding.UTF8); 
            return httpResponseMessage; 
        }




        public HttpResponseMessage Get()
        { 
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponseMessage.Content = new StringContent("不支持GET请求，请用POST请求。", Encoding.UTF8);
            return httpResponseMessage;
        }


    }
}

