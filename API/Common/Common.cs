using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Emrys.API
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class Common
    { 
        /// <summary>
        /// 时间戳转化成时间格式
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string strValue, string propName = "")
        {
            Int64 ticks;
            if ((!Int64.TryParse(strValue, out ticks)))
            {
                throw new APIException(500, string.Format("{0}格式不是DateTime时间戳类型！", propName));
            }

            var date = new DateTime(1970, 1, 1);
            date = date.AddMilliseconds(ticks);
            return date;
        }

    }

    /// <summary>
    /// 设置时间和JSON转化格式格式
    /// </summary>
    public class UnixDateTimeConverter : DateTimeConverterBase
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Common.ConvertToDateTime(Convert.ToString(reader.Value), reader.Path);

        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("时间格式错误.1");
                }
                ticks = Convert.ToInt64(delta.TotalMilliseconds);
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(ticks);
        }
    }
}