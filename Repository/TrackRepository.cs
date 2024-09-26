using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class TrackRepository : GenericRepository<Track>
    {
        public TrackRepository(ApplicationContext appContext) : base(appContext)
        {
            // добавляем связанные данные
            // или делать это только при вызове Get()?
            dbSet.AddRange(context.Set<Track>()
                .Include(track => track.Album)
                .Include(track => track.TrackArtists)
                .ThenInclude(trackArtist => trackArtist.Artist).ToList());
        }

        public new virtual IEnumerable<Track> Get(Expression<Func<Track, bool>> filter = null)
        {
            IQueryable<Track> result = dbSet;

            if (filter != null)
            {
                result = result.Where(filter);
            }

            return result.ToList();

        }


    }
}
