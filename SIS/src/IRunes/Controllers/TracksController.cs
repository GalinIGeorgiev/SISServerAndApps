using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace IRunes.Controllers
{
    public class TracksController : BaseController
    {
        private readonly IAlbumService albumService;

        private readonly ITrackService trackservice;

        public TracksController()
        {
            this.albumService = new AlbumService();
            this.trackservice = new TrackService();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var albumId = request.QueryData["albumId"].ToString();

            if (!this.albumService.AlbumExists(albumId))
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }


            var viewBag = new Dictionary<string, string> { { "AlbumId", albumId } };

            return this.View("tracks/create", request, viewBag);
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            var name = request.FormData["name"].ToString();
            var link = request.FormData["link"].ToString();
            var priceString = request.FormData["price"].ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(link)
                                                || string.IsNullOrWhiteSpace(priceString))
            {
                return this.Error("Name, link and price cannot be empty!", HttpResponseStatusCode.BadRequest, request);
            }

            if (!decimal.TryParse(priceString, out decimal price))
            {
                return this.Error("Invalid price", HttpResponseStatusCode.BadRequest, request);
            }

            var albumId = request.QueryData["albumId"].ToString();

            var album = this.albumService.GetAlbumById(albumId);

            if (album == null)
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }

            this.trackservice.CreateTrack(name, albumId, link, price);

            return this.Redirect("/albums/details?id=" + albumId);
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return this.Error("No track id specified", HttpResponseStatusCode.BadRequest, request);
            }

            string trackId = (string)request.QueryData["id"];

            Track currentTrack = this.trackservice.GetTrackById(trackId);

            if (currentTrack == null)
            {
                return this.Error("Track not found", HttpResponseStatusCode.NotFound, request);
            }
            currentTrack.Link = WebUtility.UrlDecode(currentTrack.Link);

            string pattern = @"(?:https?:\/\/)?(?:www\.)?(?:(?:(?:youtube.com\/watch\?[^?]*v=|youtu.be\/)([\w\-]+))(?:[^\s?]+)?)";
            string replacement = @"<iframe title='YouTube video player' width='480' height='390' src='http://www.youtube.com/embed/$1' frameborder='0' allowfullscreen='1'></iframe>";

            Regex regex = new Regex(pattern);
            var result = regex.Replace(currentTrack.Link, replacement);

            Dictionary<string, string> viewBag = new Dictionary<string, string>
                              {
                                  {"Name", currentTrack.Name},
                                  {"Link", result},
                                  {"AlbumId", currentTrack.AlbumId.ToString()},
                                  {"Price", currentTrack.Price.ToString("F2")}
                              };

            return this.View("tracks/details", request, viewBag);
        }
    }
}

