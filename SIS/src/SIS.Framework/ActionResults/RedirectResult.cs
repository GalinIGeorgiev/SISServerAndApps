using SIS.Framework.ActionResults.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResults
{
    public class RedirectResult : IRedirectable
    {
        public string RedirectUrl { get; }
        public RedirectResult(string redirectUrl)
        {
            this.RedirectUrl = redirectUrl;
        }


        public string Invoke() => this.RedirectUrl;
    }
}
