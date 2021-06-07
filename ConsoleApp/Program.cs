using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using Repository.DTO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {


            PlaylistTypeDto somePlaylistType = Playlists.GetPlaylistTypes().FirstOrDefault();

            UserDto user = Users.GetUsers("dartungar").FirstOrDefault();
            PlaylistDto playlist = Playlists.GetPlaylists("My Awesome Playlist").FirstOrDefault();
            TrackDto track = Tracks.GetTracks("Mein Land").FirstOrDefault();
            if (playlist != null && track != null)
            {
                Playlists.AddTrackToPlaylist(track.Id, playlist.Id);
            }
            else throw new Exception("Не найдено");

        }

    }
}
