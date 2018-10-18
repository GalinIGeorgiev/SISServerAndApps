using SIS.Framework.ActionResults.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.ActionResults
{
    public class ViewResult : IViewable
    {
        public ViewResult(IRenderable view)
        {
            this.View = view;
        }
        public IRenderable View { get; set; }

        public string Invoke() => this.View.Render();       
    }
}
