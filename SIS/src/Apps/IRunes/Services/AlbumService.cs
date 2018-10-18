using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class AlbumService : IAlbumService
    {
        public ICollection<Album> GetAllAlbums()
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Albums.Include(a => a.Tracks).ToArray();
            }
        }

        public void CreateAlbum(string name, string cover)
        {
            Album currentAlbum = new Album
            {
                Name = name,
                Cover = cover
            };

            using (IRunesDbContext db = new IRunesDbContext())
            {
                db.Albums.Add(currentAlbum);
                db.SaveChanges();
            }
        }

        public Album GetAlbumById(string id)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Albums.FirstOrDefault(a => a.Id.ToString() == id);
            }
        }

        public ICollection<Track> GetAlbumTracks(string id)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Tracks.Where(t => t.AlbumId.ToString() == id).ToArray();
            }
        }

        public bool AlbumExists(string id)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Albums.Any(a => a.Id.ToString() == id);
            }
        }

        public decimal GetPrice(string id)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Albums
                    .FirstOrDefault(a => a.Id.ToString() == id)
                    .Tracks
                    .Sum(t => t.Price);
            }
        }
    }
}
