using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Service
{
    class Service
    {
        private UnitOfWork UnitOfWork { get; set; }
        public AddressService AddressService { get; set; }
        public AlbumService AlbumService { get; set; }

        public ArtistService ArtistService { get; set; }

        public PlaylistService PlaylistService { get; set; }
        public TrackService TrackService { get; set; }
        public UserService UserService { get; set; }

        public Service()
        {
            UnitOfWork = new UnitOfWork();
        }
    }
}
