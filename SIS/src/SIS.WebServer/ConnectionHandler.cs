﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
namespace SIS.WebServer
{
    using HTTP.Common;
    using HTTP.Exceptions;
    using HTTP.Requests;
    using HTTP.Responses;
    using HTTP.Sessions;
    using Results;
    using Routing;
    using SIS.WebServer.Api;
    using SIS.WebServer.Api.Contracts;
    using System.IO;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly IHttpHandler httpHandler;

        private const string RootDirectoryRelativePath = "../../..";

        public ConnectionHandler(Socket client, IHttpHandler handler)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(handler, nameof(handler));

            this.client = client;
            this.httpHandler = handler;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data.Array, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        //private IHttpResponse HandleRequest(IHttpRequest httpRequest)
        //{
        //    var isResourceRequest = this.isResourceRequest(httpRequest);
        //    if (isResourceRequest)
        //    {
        //        return this.HandleRequestResponse(httpRequest.Path);
        //    }
        //    else
        //    {
        //        if (!this.serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
        //        || !this.serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path.ToLower()))
        //        {
        //            return new HttpResponse(HttpResponseStatusCode.NotFound);
        //        }
        //    }    
        //    return this.serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
        //}

        //private IHttpResponse HandleRequestResponse(string httpRequestPath)
        //{
        //    var indexStartOfExtention = httpRequestPath.LastIndexOf(".");
        //    var indexOfStartOfNameOfResource=httpRequestPath.LastIndexOf("/");
        //    var requestPathExtention = httpRequestPath.Substring(indexStartOfExtention);

        //    var resourceName = httpRequestPath
        //        .Substring(indexOfStartOfNameOfResource);

        //    var resourcePath=RootDirectoryRelativePath+"/Resources"+
        //        $"/{requestPathExtention.Substring(1)}"+
        //        resourceName;

        //    if (!File.Exists(resourcePath))
        //    {
        //        return new HttpResponse(HttpResponseStatusCode.NotFound);
        //    }
        //    var fileContent = File.ReadAllBytes(resourcePath);

        //    return new InlineResourceResult(fileContent, HttpResponseStatusCode.Ok);
        //}

        //private bool isResourceRequest(IHttpRequest httpRequest)
        //{
        //    var requestPath = httpRequest.Path;

        //    if (requestPath.Contains("."))
        //    {
        //        var requestPathExtention = requestPath.Substring(requestPath.LastIndexOf("."));
        //        return  GlobalConstants.ResourceExtentions.Contains(requestPathExtention);
        //    }


        //    return false;
        //}

        private async Task PrepareResponse(IHttpResponse httpResponse)
        {
            byte[] byteSegments = httpResponse.GetBytes();

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId = null;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
                httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            }

            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                httpResponse
                    .AddCookie(new HttpCookie(HttpSessionStorage.SessionCookieKey
                        , sessionId));
            }
        }

        public async Task ProcessRequestAsync()
        {
            try
            {
                var httpRequest = await this.ReadRequest();

                if (httpRequest != null)
                {
                    string sessionId = this.SetRequestSession(httpRequest);

                    var httpResponse = this.httpHandler.Handle(httpRequest);

                    this.SetResponseSession(httpResponse, sessionId);

                    await this.PrepareResponse(httpResponse);
                }
            }
            catch (BadRequestException e)
            {
                await this.PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.BadRequest));
            }
            catch (Exception e)
            {
                await this.PrepareResponse(new TextResult(e.Message, HttpResponseStatusCode.InternalServerError));
            }

            this.client.Shutdown(SocketShutdown.Both);
        }
    }
}