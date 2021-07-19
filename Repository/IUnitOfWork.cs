using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public ApplicationContext Context { get; set; }

        public void Save();

        public new void Dispose();
    }
}
