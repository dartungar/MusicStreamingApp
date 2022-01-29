using System;

namespace DAL.EF
{
    /// <summary>
    /// Contains repositories
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationContext Context { get; set; } = new ApplicationContext();
        private bool _disposed = false;

        public void Save()
        {
            Context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
