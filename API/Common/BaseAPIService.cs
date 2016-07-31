using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Emrys.API
{
    /// <summary>
    /// API实现类的父类
    /// </summary>
    public abstract class BaseAPIService
    {

        /// <summary>
        /// POST数据对象
        /// </summary>
        public APIReqeust Reqeust { get; set; }

        /// <summary>
        /// 获取Post数据的参数
        /// </summary>
        public JObject Params
        {
            get
            {
                return Reqeust.Params as JObject;
            }
        }

        /// <summary>
        /// 获取参数的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">参数名称</param>
        /// <param name="isRequired">是否需要验证为空</param>
        /// <returns></returns>
        public T GetParams<T>(string name, bool isRequired = true)
        {
            // 转成JObject
            JObject j = Reqeust.Params as JObject;
            // 获取值
            var v = j[name];
            var strValue = Convert.ToString(v);

            // 判断是否可为空 
            if (string.IsNullOrEmpty(strValue) && (isRequired))
            {
                throw new APIException(500, string.Format("参数{0}不存在！", name));
            }

            // string类型
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(strValue, typeof(T));
            }
            // int类型
            if (typeof(T) == typeof(int))
            {
                // 如果可为空而且值也为空，则直接返回0
                if ((!isRequired) && (string.IsNullOrEmpty(strValue)))
                {
                    return (T)Convert.ChangeType(0, typeof(T));
                }

                int intValue;
                if ((!int.TryParse(strValue, out intValue)))
                {
                    throw new APIException(500, string.Format("参数{0}不是Int类型！", name));
                }
                return (T)Convert.ChangeType(intValue, typeof(T));
            }
            // 时间类型
            else if (typeof(T) == typeof(DateTime))
            {
                DateTime date = Common.ConvertToDateTime(strValue, name);
                return (T)Convert.ChangeType(date, typeof(T));
            }
            throw new Exception(string.Format("获取参数只支持String、Int和DateTime类型！"));

        }

        /// <summary>
        /// API上下文信息
        /// </summary>
        public APIWorkContext WorkContext
        {
            get
            {
                return Reqeust.APIWorkContext;
            }
        }

        /// <summary>
        /// 把Post数据参数转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ConvertToModel<T>() where T : class
        {
            T t;
            try
            {
                UnixDateTimeConverter utc = new UnixDateTimeConverter();
                t = JsonConvert.DeserializeObject<T>(Reqeust.Params.ToString(), utc);
            }
            catch (Exception ex)
            {

                throw new APIException(1, ex.Message);
            }

            // 判断参数是否可以为空
            Type objType = typeof(T);

            // 首先获取类上的特性 
            var classAttribute = objType.GetCustomAttribute(typeof(APIRequired), true) as APIRequired;


            // 取属性上的自定义特性
            foreach (PropertyInfo propInfo in objType.GetProperties())
            {
                // 获取属性的验证特性，如果属性设置了，则忽略类设置的验证特性
                object[] objAttrs = propInfo.GetCustomAttributes(typeof(APIRequired), true);

                if (objAttrs.Length > 0)
                {
                    if (!(objAttrs[0] as APIRequired).Required)
                    {
                        continue;
                    }
                }
                // 属性没有设置验证特性，则需要判断类中的验证特性
                else
                {
                    if (classAttribute == null || !classAttribute.Required)
                    {
                        continue;
                    }
                }

                object value = propInfo.GetValue(t);
                if (propInfo.PropertyType == typeof(string) || propInfo.PropertyType == typeof(DateTime))
                {
                    if (string.IsNullOrEmpty(Convert.ToString(value)))
                    {
                        throw new APIException(propInfo.Name + "参数不能为空");
                    }
                    if (propInfo.PropertyType == typeof(DateTime) && Convert.ToDateTime(Convert.ToString(value)) == DateTime.MinValue)
                    {
                        throw new APIException(propInfo.Name + "参数不能为空");
                    }
                }


            }
            return t;
        }
        /// <summary>
        /// 函数执行方法
        /// </summary>
        /// <returns></returns>
        public abstract APIResult Process();


        /// <summary>
        /// 返回值Json，默认成功
        /// </summary>
        /// <returns></returns>
        public APIResultJson APIJson()
        {
            return new APIResultJson(Reqeust);
        }

        /// <summary>
        /// 返回值Json，设置一个实体类
        /// </summary>
        /// <param name="_detail">实体类</param>
        /// <returns></returns>
        public APIResultJson APIJson(dynamic _detail)
        {
            return new APIResultJson(Reqeust, _detail);
        }

        /// <summary>
        /// 返回值Json，设置状态码和状态信息
        /// </summary>
        /// <param name="_result">状态</param>
        /// <param name="_resultNote">状态信息</param>
        /// <returns></returns>
        public APIResultJson APIJson(int _result, string _resultNote)
        {
            return new APIResultJson(Reqeust, _result, _resultNote);
        }

        /// <summary>
        /// 返回值Json，设置状态码、状态信息和实体类
        /// </summary>
        /// <param name="_result">状态</param>
        /// <param name="_resultNote">状态信息</param>
        /// <param name="_detail">实体对象</param>
        /// <returns></returns>
        public APIResultJson APIJson(int _result, string _resultNote, dynamic _detail)
        {
            return new APIResultJson(Reqeust, _result, _resultNote, _detail);
        }

        /// <summary>
        /// 直接返回string类型
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIContext APIContext(string context)
        {
            return new APIContext(context);
        }



        
    }
}