using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Services.Contracts
{
    public interface ITrackService
    {
        Track GetTrackById(string id);

        void CreateTrack(string name, string albumId, string link, decimal price);
    }
}
