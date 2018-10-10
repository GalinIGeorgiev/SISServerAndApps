namespace IRunesWebApp.Model
{
    public class TrackAlbum : BaseEntity<int>
    {
        public string AlbumId { get; set; }

        public virtual Album Album {get;set;}
        public string TrackId { get; set; }

        public virtual Track Track { get; set; }
    }
}