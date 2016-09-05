using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Emrys.API
{
    /// <summary>
    /// 程序主函数
    /// </summary>
    public class Main
    {
        /// <summary>
        /// 执行函数，获取返回值
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string Process(string parameters)
        {
            // 设置全局的cmd名称，在报错时把cmd值返回
            string globalCmd = string.Empty;
            try
            {
                // 获取json转化的对象 
                APIReqeust req;
                try
                {
                    // 转换成JSON对象
                    req = JsonConvert.DeserializeObject<APIReqeust>(parameters);
                }
                catch
                {
                    // 转换错误
                    throw new APIException(100, "JSON格式不正确。");
                }

                // 判断是否转换成正确的对象
                if (req == null || string.IsNullOrEmpty(req.cmd))
                {
                    throw new APIException(200, "参数格式不正确。");
                }

                globalCmd = req.cmd;

                APIResult res;
                // 获取类名称
                string fullName = string.Format("Emrys.API.APIs.{0}APIService", req.cmd);

                // 根据类名获取实例
                var t = Type.GetType(fullName, false, true);
                if (t == null)
                {
                    throw new APIException(200, string.Format("无接口{0}。", req.cmd));
                }

                // 获取自定义特性，判断是否需要验证Token
                APINotNeedToken nt = (APINotNeedToken)Attribute.GetCustomAttribute(t, typeof(APINotNeedToken));
                if (nt == null || (!nt.NotNeedToken))
                {
                    // 如果需要验证  

                    //  根据token获取用户信息 
                    //var user = GetUser(req.token) ;
                    //if (user == null)
                    //{
                    //    // 判断Token是否失效
                    //    throw new APIException(200, "Token失效。");
                    //}
                    //req.APIWorkContext = new APIWorkContext();
                    //req.APIWorkContext.UserId = Convert.ToInt32(user.user_id);
                    //req.APIWorkContext.UserEmail = user.email;

                }
                // 反射创建对象
                object APIservices = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(fullName, true, System.Reflection.BindingFlags.CreateInstance, null, null, null, null);
                // 转化成类对象
                BaseAPIService baseService = APIservices as BaseAPIService;

                try
                {
                    // 执行函数
                    baseService.Reqeust = req;
                    res = baseService.Process();
                }
                catch (APIException ex)
                {
                    throw ex;
                }
                // 判断是否是直接返回String
                if (res is APIContext)
                {
                    return (res as APIContext).Context;
                }

                // 设置时间格式  
                var jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                // 设置过滤null值为不显示
                jsonSettings.Converters.Add(new UnixDateTimeConverter());

                return JsonConvert.SerializeObject(res, jsonSettings);
            }
            catch (APIException ex)
            {
                // 自定义错误
                return JsonConvert.SerializeObject(new APIResultJson() { cmd = globalCmd, result = ex.Code, resultNote = ex.Message }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception ex)
            {
                // 全局错误
                return JsonConvert.SerializeObject(new APIResultJson() { cmd = globalCmd, result = 500, resultNote = ex.Message }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
        }
    }
}