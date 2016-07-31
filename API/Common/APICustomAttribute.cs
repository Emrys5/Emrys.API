using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emrys.API
{

    /// <summary>
    /// 设置参数是否可以为空
    /// </summary>
    public class APIRequired : Attribute
    {
        public bool Required { get; set; }
        public APIRequired()
        {
            Required = true;
        }
        public APIRequired(bool req)
        {
            this.Required = req;
        }
    }

    /// <summary>
    /// 判断权限是否需要Token
    /// </summary>
    public class APINotNeedToken : Attribute
    {
        public APINotNeedToken()
        {
            NotNeedToken = true;
        }

        public APINotNeedToken(bool notNeedToken)
        {
            NotNeedToken = notNeedToken;
        }


        public bool NotNeedToken { get; set; }
    }

}