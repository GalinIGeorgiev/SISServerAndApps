using System.Collections.Generic;

namespace SIS.HTTP.Common
{
    public static class GlobalConstants
    {
        public const string HttpOneProtocolFragment = "HTTP/1.1";

        public const string HostHeaderKey = "Host";

        public const string HttpNewLine = "\r\n";

        public static List<string> ResourceExtentions =new List<string> { ".css", ".js" };
    }
}
