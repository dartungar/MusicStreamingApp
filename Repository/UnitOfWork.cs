using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;

namespace Repository
{
    /// <summary>
    /// Contains repositories
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private ApplicationContext context = new ApplicationContext();
        private bool disposed = false;

        private GenericRepository<Address> addressRepository;
        private GenericRepository<AddressElement> addressElementRepository;
        private GenericRepository<AddressElementType> addressElementTypeRepository;
        private GenericRepository<Album> albumRepository;

        private GenericRepository<Artist> artistRepository;
        private GenericRepository<Track> trackRepository;
        private GenericRepository<TrackArtist> trackArtistRepository;
        private GenericRepository<Playlist> playlistRepository;
        private GenericRepository<User> userRepository;

        public GenericRepository<Address> AddressRepository
        {
            get
            {
                if (addressRepository == null)
                {
                    addressRepository = new GenericRepository<Address>(context);
                }

                return addressRepository;
            }
        }

        public GenericRepository<AddressElement> AddressElementRepository
        {
            get
            {
                if (addressElementRepository == null)
                {
                    addressElementRepository = new GenericRepository<AddressElement>(context);
                }

                return addressElementRepository;
            }
        }

        public GenericRepository<AddressElementType> AddressElementTypeRepository
        {
            get
            {
                if (addressElementTypeRepository == null)
                {
                    addressElementTypeRepository = new GenericRepository<AddressElementType>(context);
                }

                return addressElementTypeRepository;
            }
        }

        public GenericRepository<Album> AlbumRepository
        {
            get
            {
                if (albumRepository == null)
                {
                    albumRepository = new GenericRepository<Album>(context);
                }

                return albumRepository;
            }
        }

        public GenericRepository<Artist> ArtistRepository
        {
            get
            {
                if (artistRepository == null)
                {
                    artistRepository = new GenericRepository<Artist>(context);
                }

                return artistRepository;
            }
        }

        public GenericRepository<Playlist> PlaylistRepository
        {
            get
            {
                if (playlistRepository == null)
                {
                    playlistRepository = new GenericRepository<Playlist>(context);
                }

                return playlistRepository;
            }
        }

        public GenericRepository<Track> TrackRepository
        {
            get
            {
                if (trackRepository == null)
                {
                    trackRepository = new GenericRepository<Track>(context);
                }

                return trackRepository;
            }
        }

        public GenericRepository<TrackArtist> TrackArtistRepository
        {
            get
            {
                if (trackArtistRepository == null)
                {
                    trackArtistRepository = new GenericRepository<TrackArtist>(context);
                }

                return trackArtistRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new GenericRepository<User>(context);
                }

                return userRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
