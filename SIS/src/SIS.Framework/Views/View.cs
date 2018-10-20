using SIS.Framework.ActionResults.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;
        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;

        }

        private string ReadFile()
        {
            if (!File.Exists(fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException($"View does not exist as {fullyQualifiedTemplateName}");
            }

            return File.ReadAllText(this.fullyQualifiedTemplateName);
        }
        public string Render()
        {
            var fullHtml = this.ReadFile();
            return fullHtml;
        }
    }
}
