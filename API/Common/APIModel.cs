using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emrys.API
{



    public class APIResult
    {
    }


    /// <summary>
    /// 返回string类型
    /// </summary>
    public class APIContext : APIResult
    {
        public APIContext(string _context)
        {
            this.Context = _context;
        }
        public string Context { get; set; }
    }


    /// <summary>
    /// 返回Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResultJson : APIResult
    {
        private APIReqeust Request { get; set; }
        public APIResultJson()
        {
        }
        public APIResultJson(APIReqeust req)
        {
            Request = req;
        }

        public APIResultJson(APIReqeust req, dynamic _detail)
        {
            Request = req;
            detail = _detail;
        }

        public APIResultJson(APIReqeust req, int _result, string _resultNote, dynamic _detail)
        {
            Request = req;
            result = _result;
            resultNote = _resultNote;
            detail = _detail;
        }

        public APIResultJson(APIReqeust req, int _result, string _resultNote)
        {
            Request = req;
            result = _result;
            resultNote = _resultNote;
        }

        private string _cmd;
        public string cmd
        {
            get
            {
                if (string.IsNullOrEmpty(_cmd))
                {
                    if (Request == null || string.IsNullOrEmpty(Request.cmd))
                    {
                        return "";
                    }
                    return Request.cmd; 
                }
                return _cmd;
                
            }
            set
            {
                _cmd = value;
            }
        }
        public int result { get; set; }
        private string _resultNote;
        public string resultNote
        {
            get
            {
                return string.IsNullOrEmpty(_resultNote) ? "Success" : _resultNote;
            }
            set
            {
                _resultNote = value;
            }
        }
        public int totalRecordNum { get; set; }
        public int pages { get; set; }
        public int pageNo { get; set; }
        public dynamic detail { get; set; }
    }





    /// <summary>
    /// 接收参数Model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIReqeust
    {
        public string cmd { get; set; }
        public string token { get; set; }
        public string version { get; set; }
        public int pageNo { get; set; }
        public int onePageNum { get; set; }
        public dynamic Params { get; set; }
        public APIWorkContext APIWorkContext { get; set; }
    }


    /// <summary>
    /// API上下文信息
    /// </summary>
    public class APIWorkContext
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }
}