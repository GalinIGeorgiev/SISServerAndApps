using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.Framework
{
    public class HttpHandler : IHttpHandler
    {
        private readonly string RootDirectoryRelativePath = "../../..";

        private ServerRoutingTable serverRoutingTable;

        public HttpHandler(ServerRoutingTable routingTable)
        {
            this.serverRoutingTable = routingTable;
        }

        public IHttpResponse Handle(IHttpRequest httpRequest)
        {
            var isResourceRequest = this.isResourceRequest(httpRequest);
            if (isResourceRequest)
            {
                return this.HandleRequestResponse(httpRequest.Path);
            }
            else
            {
                if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
                || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
                {
                    return new HttpResponse(HttpResponseStatusCode.NotFound);
                }
            }
            return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);

        }

        private bool isResourceRequest(IHttpRequest httpRequest)
        {
            var requestPath = httpRequest.Path;

            if (requestPath.Contains("."))
            {
                var requestPathExtention = requestPath.Substring(requestPath.LastIndexOf("."));
                return GlobalConstants.ResourceExtentions.Contains(requestPathExtention);
            }


            return false;
        }

        private IHttpResponse HandleRequestResponse(string httpRequestPath)
        {
            var indexStartOfExtention = httpRequestPath.LastIndexOf(".");
            var indexOfStartOfNameOfResource = httpRequestPath.LastIndexOf("/");
            var requestPathExtention = httpRequestPath.Substring(indexStartOfExtention);

            var resourceName = httpRequestPath
                .Substring(indexOfStartOfNameOfResource);

            var resourcePath = RootDirectoryRelativePath + "/Resources" +
                $"/{requestPathExtention.Substring(1)}" +
                resourceName;

            if (!File.Exists(resourcePath))
            {
                return new HttpResponse(HttpResponseStatusCode.NotFound);
            }
            var fileContent = File.ReadAllBytes(resourcePath);

            return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        }

    }
}