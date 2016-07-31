using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Emrys.API.APIs;
using System.Reflection;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;

namespace Emrys.API
{
    /// <summary>
    /// 用MVC的模式
    /// </summary>
    public class HomeController : Controller
    {
        [HttpGet]
        public string Index(string t)
        {
            return "不支持GET请求，请用POST请求。";
        }



        /// <summary>
        /// API入口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string Index()
        {
            // 解析 Post Json 参数 
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string parameters = System.Text.Encoding.UTF8.GetString(byts);
            return Main.Process(parameters);
        }
    }
}
