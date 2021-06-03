using System;
using Service;
using System.Collections.Generic;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Albums albumService = new Albums();
            Artists artistsService = new Artists();
            artistsService.AddArtist("Rammstein", "fb.com/rammstein", "This is Rammstein", true, "https://static.dw.com/image/49947376_303.jpg");
            artistsService.AddArtist("Nadezhda Babkind", "ok.ru/BabkinaOfficial", "This is Nadezhda Babkina", true, "https://static.dw.com/image/49947376_303.jpg");
            List<ITrackData> tracks = new List<ITrackData>();
            var artists = artistsService.GetArtists();
            List<Guid> artistsIds = new List<Guid>();
            foreach (var artist in artists) artistsIds.Add(artist.Id);
            tracks.Add(new TrackData("Despacito", 185, artistsIds));
            tracks.Add(new TrackData("Mein Land", 231, artistsIds));
            tracks.Add(new TrackData("Malo Polovin", 156, artistsIds));

            try
            {
                albumService.AddAlbumWithTracks("The Very Best", "EP", new DateTime(2021, 6, 3), tracks, "https://pbs.twimg.com/media/ETzsjosWkA4cEew.jpg");
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка при добавлении альбома!");
                throw;
            }
            var albums = albumService.GetAlbums("Best");
            foreach (var album in albums)
            {
                Console.WriteLine($"{album.Name}");
            }
        }

        struct TrackData : ITrackData
        {
            public string Name { get; set; }
            public int Length { get; set; }
            public List<Guid> ArtistIds { get; set; }
            public Guid AlbumId { get; set; }

            public TrackData(string name, int length, List<Guid> artistIds)
            {
                Name = name;
                Length = length;
                ArtistIds = artistIds;
                AlbumId = Guid.Empty;
            }
        }
    }
}
