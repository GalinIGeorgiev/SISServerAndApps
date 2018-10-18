using SIS.Framework.Attributes.Methods.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpPostAttribute : HttpMethodAttribute
    {

        public override bool isValid(string requestMethod)
        {
            if (requestMethod.ToLower() == "post")
            {
                return true;
            }
            return false;
        }
    }
}
