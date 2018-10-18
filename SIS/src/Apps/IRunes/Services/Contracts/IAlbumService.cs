using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services.Contracts
{
    public interface IAlbumService
    {
        void CreateAlbum(string name, string cover);

        decimal GetPrice(string id);

        bool AlbumExists(string id);

        ICollection<Album> GetAllAlbums();

        Album GetAlbumById(string id);

        ICollection<Track> GetAlbumTracks(string id);
    }
}
