using SIS.Framework.ActionResults.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResults.Contracts
{
    public interface IRedirectable:IActionResult
    {
        string RedirectUrl { get; }
    }
}
