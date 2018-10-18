using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace IRunes.Controllers
{
    public class AlbumsController : BaseController
    {
        private readonly IAlbumService albumService;

        public AlbumsController()
        {
            this.albumService = new AlbumService();
        }

        public IHttpResponse All(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            StringBuilder result = new StringBuilder();

            ICollection<Album> albums = this.albumService.GetAllAlbums();

            if (albums.Count == 0)
            {
                result.Append("<em>There are currently no albums.</em>");
            }

            else
            {
                foreach (var album in albums)
                {
                    result.AppendLine($"<a href=\"/albums/details?id={album.Id}\">{album.Name}</a><br>");
                }
            }

            Dictionary<string, string> viewBag = new Dictionary<string, string> { { "Albums", result.ToString() } };

            return this.View("albums/all", request, viewBag);
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            return this.View("albums/create", request);
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            string albumName = request.FormData["name"].ToString();
            string albumCover = request.FormData["cover"].ToString();

            if (string.IsNullOrWhiteSpace(albumCover) || string.IsNullOrWhiteSpace(albumName))
            {
                return this.Error("Album name and cover cannot be empty!", HttpResponseStatusCode.BadRequest, request);
            }

            this.albumService.CreateAlbum(albumName, albumCover);

            return this.Redirect("/albums/all");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!request.IsLoggedIn())
            {
                return this.Redirect("/users/login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return this.Error("No album id specified", HttpResponseStatusCode.BadRequest, request);
            }

            string albumId = request.QueryData["id"].ToString();

            Album currentAlbum = this.albumService.GetAlbumById(albumId);

            if (currentAlbum == null)
            {
                return this.Error("Album not found", HttpResponseStatusCode.NotFound, request);
            }

            StringBuilder result = new StringBuilder();

            ICollection<Track> tracks = this.albumService.GetAlbumTracks(albumId).ToArray();

            if (tracks.Count == 0)
            {
                result.Append("<em>There are no tracks in this album!</em>");
            }
            else
            {
                result.Append("<ol>");
                foreach (var track in tracks)
                {
                    result.AppendLine($"<li><a href=\"/tracks/details?id={track.Id}\">{track.Name}</a></li>");
                }

                result.Append("</ol>");
            }

            decimal price = this.albumService.GetPrice(albumId);

            Dictionary<string, string> viewBag = new Dictionary<string, string>
                              {
                                  {"CoverUrl", WebUtility.UrlDecode(currentAlbum.Cover)},
                                  {"Name", currentAlbum.Name},
                                  {"Price", price.ToString("F2")},
                                  {"Tracks", result.ToString()},
                                  {"AlbumId", albumId}
                              };

            return this.View("albums/details", request, viewBag);
        }
    }
}
