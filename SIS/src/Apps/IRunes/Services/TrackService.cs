using IRunes.Data;
using IRunes.Models;
using IRunes.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class TrackService : ITrackService
    {
        public void CreateTrack(string name, string albumId, string link, decimal price)
        {
            Track currentTrack = new Track
            {
                AlbumId = Guid.Parse(albumId),
                Name = name,
                Link = link,
                Price = price
            };

            using (IRunesDbContext db = new IRunesDbContext())
            {
                db.Tracks.Add(currentTrack);
                db.SaveChanges();
            }
        }

        public Track GetTrackById(string id)
        {
            using (IRunesDbContext db = new IRunesDbContext())
            {
                return db.Tracks.FirstOrDefault(t => t.Id.ToString() == id);
            }
        }
    }
}
