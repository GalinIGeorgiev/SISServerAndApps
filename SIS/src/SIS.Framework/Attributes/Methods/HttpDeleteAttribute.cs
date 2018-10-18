using SIS.Framework.Attributes.Methods.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpDeleteAttribute : HttpMethodAttribute
    {

        public override bool isValid(string requestMethod)
        {
            if (requestMethod.ToLower() == "delete")
            {
                return true;
            }
            return false;
        }
    }
}
