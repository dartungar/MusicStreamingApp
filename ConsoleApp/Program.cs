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
            Application app = new Application();
            while (app.IsActive)
            {
                if (app.User == null)
                {
                    int key = app.PaintWelcomeScreen();
                    switch (key)
                    {
                        case 1:
                            app.RegisterNewUser();
                            break;
                        case 2:
                            app.AuthenticateUser();
                            break;
                        case 3:
                            app.IsActive = false;
                            break;
                        default:
                            break;
                    }
                } else
                {
                    int key = app.PaintMainMenu();
                    switch (key)
                    {
                        case 1:
                            app.SearchAndDisplayTracks();
                            break;
                        case 2:
                            app.SearchArtists();
                            break;
                        case 3:
                            app.SearchAndDisplayPlaylists();
                            break;
                        case 4:
                            app.SearchAndDisplayUsers();
                            break;
                        case 5:
                            app.AddAlbum();
                            break;
                        case 6:
                            app.AddArtist();
                            break;
                        case 7:
                            app.EditArtist();
                            break;
                        case 8:
                            app.AddPlaylist();
                            break;
                        case 9:
                            app.RemovePlaylist();
                            break;
                        case 0:
                            app.IsActive = false;
                            break;
                        default:
                            break;
                    }
                }

            }


            

        }

    }
}
