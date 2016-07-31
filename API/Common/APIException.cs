using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emrys.API
{
    /// <summary>
    /// 自定义APIException
    /// </summary>
    public class APIException : ApplicationException
    {


        private int _code;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code
        {
            get
            {
                return _code == 0 ? 500 : _code;
            }
            set
            {
                _code = value;
            }
        }


        private string _message { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public override string Message
        {
            get
            {
                return string.IsNullOrEmpty(_message) ? base.Message : _message;
            }
        }


        /// <summary>
        /// 默认错误信息
        /// </summary>
        public APIException() { }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <param name="code">代码</param>
        public APIException(int code)
        {
            this.Code = code;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public APIException(string message)
        {
            this._message = message;
        }
        /// <summary>
        /// 代码和信息
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="message">错误信息</param>
        public APIException(int code, string message)
        {
            this.Code = code;
            this._message = message;
        }

    }
}