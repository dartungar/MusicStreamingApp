/*using System;
using System.Collections.Generic;
using System.Linq;
using Service;
using Service.DTO;

namespace ConsoleApp
{
    /// <summary>
    /// Взаимодействие с пользователем и state клиентской части
    /// </summary>
    class Application
    {
        /// <summary>
        /// Индикатор активности приложения для main loop
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Авторизованный пользователь. Если null - пользователь не авторизован
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// Рендер меню для неавторизованного пользователя, считывание нажатой клавиши
        /// </summary>
        /// <returns>int - код нажатой клавиши (1-3) </returns>
        internal int PaintWelcomeScreen()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(@"                        Welcome to the world's simplest music streaming application!
                                _   _ _ _         _             __       
                                | \ | (_) | _____ | | __ _  ___ / _|_   _ 
                                |  \| | | |/ / _ \| |/ _` |/ _ \ |_| | | |
                                | |\  | |   < (_) | | (_| |  __/  _| |_| |
                                |_| \_|_|_|\_\___/|_|\__,_|\___|_|  \__, |
                                                                    |___/ 

                                                                                ");
                Console.WriteLine("Выберите действие и нажмите соответствующую клавишу на клавиатуре:");
                Console.WriteLine("1. Зарегистрироваться");
                Console.WriteLine("2. Авторизоваться");
                Console.WriteLine("3. Выйти");
                int output;
                bool parseSuccessful = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out output);
                if (parseSuccessful)
                {
                    if (output > 0 && output < 4) return output;
                }
            }
        }

        /// <summary>
        /// Рендер меню для авторизованного пользователя и считывание нажатой клавиши
        /// </summary>
        /// <returns>int - Код нажатой клавиши (1-9, 0)</returns>
        internal int PaintMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Nikolaefy: world's simplest music streaming application \n version 0.0.5 \n");
                Console.WriteLine("Выберите действие и нажмите соответствующую клавишу на клавиатуре:");
                Console.WriteLine("1. Поиск песен                   4. Поиск пользователей        7. Изменить описание исполнителя ");
                Console.WriteLine("2. Поиск исполнителей            5. Добавить альбом            8. Добавить плейлист  ");
                Console.WriteLine("3. Поиск плейлистов              6. Добавить исполнителя       9. Удалить плейлист");
                Console.WriteLine("0. Выйти");
                int output;
                bool parseSuccessful = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out output);
                if (parseSuccessful && output < 10)
                    return output;
            }
        }
  

        /// <summary>
        /// "Зарегистрировать" пользователя
        /// </summary>
        internal void RegisterNewUser()
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();

            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();

            Console.WriteLine("Введите e-mail:");
            string email = Console.ReadLine();

            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();

            Console.WriteLine("Введите страну проживания:");
            string country = Console.ReadLine();

            Console.WriteLine("Введите регион (область) проживания:");
            string region = Console.ReadLine();

            Console.WriteLine("Введите город проживания:");
            string city = Console.ReadLine();

            Console.WriteLine("Введите улицу проживания:");
            string street = Console.ReadLine();

            Console.WriteLine("Введите номер дома:");
            string house = Console.ReadLine();

            try
            {
                UserDto user = UserRepository.AddOrGetUser(name, login, password, email, country, region, city, street, house);
                User = user;
                Console.WriteLine("Добро пожаловать!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка регистрации: {ex.Message}. Попробуйте заново");
                WaitTillUserPressesEnter();
            }
        }

        /// <summary>
        /// "Авторизовать" пользователя
        /// </summary>
        public void AuthenticateUser()
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();
            UserDto user = UserRepository.GetUser(login);
            if (user != null)
            {
                Console.WriteLine("Введите пароль:");
                string password = Console.ReadLine();
                UserDto authenticatedUser = UserRepository.CheckUserPassword(login, password);
                if (authenticatedUser != null)
                {
                    User = authenticatedUser;
                    Console.WriteLine("Добро пожаловать!");
                    ShowLoader(1500, 500);
                } else
                {
                    Console.WriteLine("Неправильный пароль");
                    WaitTillUserPressesEnter();
                }
            } else
            {
                Console.WriteLine("Пользователь не найден. Зарегистрируйтесь.");
            }
        }

        /// <summary>
        /// Найти пользователей по запросу и вывести список на экран
        /// </summary>
        public void SearchAndDisplayUsers() 
        {
            Console.WriteLine("Введите запрос для поиска пользователя по логину, имени, e-mail:");
            string query = Console.ReadLine();
            List<UserDto> users = UserRepository.SearchUsers(query);
            if (users.Count > 0)
            {
                foreach (UserDto user in users)
                {
                    Console.WriteLine($"{user.Login} {user.Name} {user.Email}");
                }
            } else
            {
                Console.WriteLine($"По запросу {query} пользователей не найдено");
                
            }
            WaitTillUserPressesEnter();
        }

        /// <summary>
        /// Найти треки по запросу и вывести список на экран
        /// </summary>
        public void SearchAndDisplayTracks() 
        {
            Console.WriteLine("Введите название трека для поиска:");
            string query = Console.ReadLine();
            List<TrackDto> tracks = TrackRepository.SearchTracks(query);
            if (tracks.Count > 0)
            {
                foreach (TrackDto track in tracks)
                {
                    List<string> artistNames = new List<string>();
                    foreach (Guid artistId in track.ArtistsIds)
                    {
                        artistNames.Add(ArtistRepository.GetArtist(artistId).Name);
                    }
                    string artists = artistNames.Count > 1 ? string.Join(", ", artistNames) : artistNames.FirstOrDefault();
                    Console.WriteLine($"{track.Length / 60}:{(track.Length % 60).ToString().PadLeft(2, '0')}  {track.Name} by {artists}");
                }
            }
            else
            {
                Console.WriteLine($"По запросу {query} треков не найдено");
            }
            WaitTillUserPressesEnter();
        }

        /// <summary>
        /// Выяснить у пользователя запрос, найти исполнителей по запросу, вывести список на экран
        /// </summary>
        /// <returns>List<ArtistDto></returns>
        public List<ArtistDto> SearchArtists()
        {
            Console.WriteLine("Введите название исполнителя для поиска:");
            string query = Console.ReadLine();
            List<ArtistDto> artists = ArtistRepository.SearchArtists(query);
            if (artists.Count > 0)
            {
                Console.WriteLine("Найдены исполнители:");
                foreach (ArtistDto artist in artists)
                {
                    Console.WriteLine(artist.Name);
                }
            }
            else Console.WriteLine("Исполнители не найдены");
            WaitTillUserPressesEnter();
            return artists;
        }

        /// <summary>
        /// Найти исполнителей по запросу, вывести список на экран
        /// </summary>
        /// <returns>List<ArtistDto></returns>
        public List<ArtistDto> SearchArtists(string query)
        {
            List<ArtistDto> artists = ArtistRepository.SearchArtists(query);
            if (artists.Count > 0)
            {
                Console.WriteLine("Найдены исполнители:");
                foreach (ArtistDto artist in artists)
                {
                    Console.WriteLine(artist.Name);
                }
            }
            else Console.WriteLine("Исполнители не найдены");
            WaitTillUserPressesEnter();
            return artists;
        }

        /// <summary>
        /// Найти плейлисты по запросу и вывести список на экран
        /// </summary>
        public void SearchAndDisplayPlaylists() 
        {
            Console.WriteLine("Введите название плейлиста для поиска:");
            string query = Console.ReadLine();
            List<PlaylistDto> playlists = PlaylistRepository.SearchPlaylists(query);
            if (playlists.Count > 0)
            {
                foreach (PlaylistDto playlist in playlists)
                {
                    string msg = $"{playlist.Name} - " + (playlist.TracksIds?.Count > 0 ? $"{playlist.TracksIds.Count} songs" : "empty");
                    Console.WriteLine(msg);
                }
            }
            else
            {
                Console.WriteLine($"По запросу {query} плейлистов не найдено");
            }
            WaitTillUserPressesEnter();
        }

        /// <summary>
        /// Добавить альбом вместе с треками, а если нужно - то и исполнителем
        /// </summary>
        public void AddAlbum() {
            Guid albumId = Guid.NewGuid();

            Console.WriteLine("Введите название альбома:");
            string albumName = Console.ReadLine();

            Console.WriteLine("Введите тип альбома (Album / EP / Single):");
            string albumType = Console.ReadLine();

            Console.WriteLine("Введите дату выхода альбома в формате 01.01.2001:");
            DateTime albumDateReleased;
            bool parseDateSuccessful = DateTime.TryParse(Console.ReadLine(), out albumDateReleased);
            if (!parseDateSuccessful) throw new Exception("Неправильный формат даты");

            Console.WriteLine("Вставьте URL картинки для альбома:");
            string albumImageUrl = Console.ReadLine();

            List<TrackDto> tracks = new List<TrackDto>();
            bool addingTracks = true;
            while (addingTracks)
            {
                Console.WriteLine("Введите название трека. Если вы закончили, введите пробел и нажмите Enter.");
                string trackName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(trackName))
                {
                    addingTracks = false;
                    break;
                }

                Console.WriteLine("Введите продолжительность трека в секундах:");
                int trackLength;
                bool parseTrackLengthSuccessful = int.TryParse(Console.ReadLine(), out trackLength);
                if (!parseTrackLengthSuccessful) 
                    throw new Exception("Продолжительность трека должна быть положительным целым числом");


                // TODO: человечный поиск
                List<Guid> artistIds = new List<Guid>();
                bool addingArtists = true;
                while (addingArtists)
                {
                    Console.WriteLine("Введите название исполнителя трека. Исполнителей может быть больше одного. Чтобы прекратить добавление, введите пробел и нажмите Enter.");

                    string query = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        addingTracks = false;
                        break;
                    }
                    // TODO: что делать, если исполнитель не найден
                    ArtistDto artist = ArtistRepository.GetArtistsByName(query).FirstOrDefault(); // TODO - выбор артиста из списка найденных
                    if (artist == null)
                    {
                        Console.WriteLine($"Исполнитель {query} не найден. Добавить? Введите да / нет:");
                        if (Console.ReadLine().Equals("да"))
                            artist = AddArtist();
                    }
                    artistIds.Add(artist.Id);
                    Console.WriteLine($"Исполнитель {artist.Name} добавлен в список исполнителей трека.");
                }
                tracks.Add(new TrackDto { Id = Guid.NewGuid(), Name = trackName, Length = trackLength, AlbumId = albumId, ArtistsIds = artistIds });
            }

            try
            {
                AlbumRepository.AddAlbumWithTracks(albumName, albumType, albumDateReleased, tracks, albumImageUrl);
                Console.WriteLine($"Альбом {albumName} с {tracks.Count} треками добавлен");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении альбома: {ex.Message}. Попробуйте заново");
            }
            WaitTillUserPressesEnter();

        }

        /// <summary>
        /// Добавить исполнителя
        /// </summary>
        /// <returns>ArtistDto</returns>
        public ArtistDto AddArtist() 
        {
            Console.WriteLine("Введите название исполнителя:");
            string artistName = Console.ReadLine();
            List <ArtistDto> existingArtists = ArtistRepository.GetArtistsByName(artistName);
            if (existingArtists.Count > 0)
            {
                Console.WriteLine($"Исполнитель с названием {artistName} уже существует");
                WaitTillUserPressesEnter();
                return existingArtists.First();
            } else
            {
                string artistDescription = "";
                Console.WriteLine("Введите описание / биографию исполнителя. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredArtistDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredArtistDescription))
                    artistDescription = enteredArtistDescription;

                string artistFacebookLink = "";
                Console.WriteLine("Введите ссылку на Facebook исполнителя. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredFacebookLink = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredFacebookLink))
                    artistFacebookLink = enteredFacebookLink;

                string artistImageUrl = "https://corgicare.com/wp-content/uploads/corgi-puppies.jpg";
                Console.WriteLine("Введите ссылку на фотографию исполнителя. Чтобы использовать изображение по умолчанию, введите пробел и нажмите Enter");
                string enteredImageUrl = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredImageUrl))
                    artistImageUrl = enteredImageUrl;

                try
                {
                    ArtistDto artist = ArtistRepository.AddArtist(artistName, artistFacebookLink, artistDescription, false, artistImageUrl);
                    Console.WriteLine("Исполнитель добавлен");
                    ShowLoader(1500, 500);
                    return artist;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении исполнителя: {ex.Message}");
                    WaitTillUserPressesEnter();
                    return null;
                }
            }
        }

        /// <summary>
        /// Найти исполнителя и отредактировать информацию о нём
        /// </summary>
        public void EditArtist()
        {
            Console.WriteLine("Введите название исполнителя, которого нужно изменить:");
            string artistName = Console.ReadLine();
            List<ArtistDto> existingArtists = ArtistRepository.GetArtistsByName(artistName);
            ArtistDto artist;
            if (existingArtists.Count == 0)
            {
                Console.WriteLine($"Исполнителя с названием {artistName} не найдено");
                ShowLoader(1500, 500);
            }
            else if (existingArtists.Count > 1)
            {
                Console.WriteLine("Найдено несколько исполнителей. Повторите поиск, уточнив запрос");
                foreach (ArtistDto artistDto in existingArtists)
                {
                    Console.WriteLine(artistDto.Name);
                }
                WaitTillUserPressesEnter();
            } else
            {
                artist = existingArtists.First();
                
                string artistDescription = artist.Description;
                Console.WriteLine("Введите новое описание / биографию исполнителя. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredArtistDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredArtistDescription))
                    artistDescription = enteredArtistDescription;

                string artistFacebookLink = artist.FacebookLink;
                Console.WriteLine("Введите новую ссылку на Facebook исполнителя. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredFacebookLink = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredFacebookLink))
                    artistFacebookLink = enteredFacebookLink;

                Console.WriteLine("Введите ссылку на фотографию исполнителя. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredImageUrl = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredImageUrl))
                    ArtistRepository.AddArtistImage(artist.Id, enteredImageUrl);

                try
                {
                    ArtistRepository.UpdateArtist(artist.Id, artist.Name, artistDescription, artistFacebookLink);
                    Console.WriteLine("Исполнитель изменён");
                    ShowLoader(1500, 500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при редактировании исполнителя: {ex.Message}");
                    WaitTillUserPressesEnter();
                }
            }
        }

        /// <summary>
        /// Добавить плейлист
        /// </summary>
        public void AddPlaylist() 
        {
            Console.WriteLine("Введите название плейлиста:");
            string playlistName = Console.ReadLine();
            List<PlaylistDto> existingPlaylists = PlaylistRepository.GetUserPlaylists(User.Id);
            if (existingPlaylists.Any(pl => pl.Name == playlistName)) 
            {
                Console.WriteLine("У вас уже есть плейлист с таким названием");
                WaitTillUserPressesEnter();
            }
            else
            {
                string playlistDescription = "";
                Console.WriteLine("Введите описание плейлиста. Чтобы пропустить, введите пробел и нажмите Enter");
                string enteredPlaylistDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredPlaylistDescription))
                    playlistDescription = enteredPlaylistDescription;

                string playlistImageUrl = "https://s3.amazonaws.com/cdn-origin-etr.akc.org/wp-content/uploads/2017/11/15175802/Airedale-Terrier-mother-and-puppy1.jpg";
                Console.WriteLine("Введите ссылку на изображение плейлиста. Чтобы использовать изображение по умолчанию, введите пробел и нажмите Enter");
                string enteredImageUrl = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enteredImageUrl))
                    playlistImageUrl = enteredImageUrl;

                try
                {
                    PlaylistRepository.AddOrGetPlaylist(playlistName, playlistDescription, User.Id, playlistImageUrl);
                    Console.WriteLine("Плейлист добавлен");
                    ShowLoader(1500, 500);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении плейлиста: {ex.Message}");
                    WaitTillUserPressesEnter();
                }
            }
        }

        public void RemovePlaylist()
        {
            Console.WriteLine("Введите название одного из ваших плейлистов:");
            string playlistName = Console.ReadLine();
            List<PlaylistDto> existingPlaylists = PlaylistRepository.GetUserPlaylists(User.Id);
            if (existingPlaylists.Count == 0)
            {
                Console.WriteLine("У вас нет плейлистов");
                WaitTillUserPressesEnter();
                return;
            }

            List<PlaylistDto> matchingUserPlaylists = existingPlaylists.Where(pl => pl.Name.Contains(playlistName)).ToList();
            if (matchingUserPlaylists.Count > 1)
            {
                Console.WriteLine($"По запросу {playlistName} найдено более 1 плейлиста. Повторите попытку, уточнив запрос");
            } else if (matchingUserPlaylists.Count == 1)
            {
                try
                {
                    PlaylistRepository.RemovePLaylist(matchingUserPlaylists.First().Id);
                    Console.WriteLine("Плейлист удален");
                    WaitTillUserPressesEnter();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при удалении плейлиста: {ex.Message}");
                    WaitTillUserPressesEnter();
                }
            } 
            else
            {
                Console.WriteLine("У вас нет плейлистов с таким названием");
                WaitTillUserPressesEnter();
            }
        }

        public void WaitTillUserPressesEnter()
        {
            string key = "";
            Console.WriteLine("Нажмите Enter, чтобы продолжить...");
            while (key != "Enter")
            {
                key = Console.ReadKey().Key.ToString();
            }
        }

        public void ShowLoader(int lengthMs, int intervalMs)
        {
            int remaining = lengthMs;
            if (remaining > 1000)
            {
                while (remaining >= intervalMs)
                {
                    Console.Write("» ");
                    remaining -= intervalMs;
                    System.Threading.Thread.Sleep(intervalMs);
                }
            }
        }
    }
}
*/