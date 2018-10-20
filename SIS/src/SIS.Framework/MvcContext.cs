﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SIS.Framework
{
   public class MvcContext
    {
        private static MvcContext Instance;

        private MvcContext() { }
        public static MvcContext Get => Instance == null ? (Instance = new MvcContext()) : Instance;

        public string AssemblyName { get; set; }

        public string ControllerSuffix { get; set; } = "Controller";
        public string ControllerFolder { get; set; } = "Controllers";
        
        public string ViewsFolder { get; set; } = "Views"; 
        public string ModelsFolder { get; set; } = "Models";
    }
}
