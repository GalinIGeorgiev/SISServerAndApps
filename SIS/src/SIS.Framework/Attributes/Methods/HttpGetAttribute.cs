using SIS.Framework.Attributes.Methods.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpGetAttribute : HttpMethodAttribute
    {

        public override bool isValid(string requestMethod)
        {
            if (requestMethod.ToLower() == "get")
            {
                return true;
            }
            return false;
        }
    }
}
