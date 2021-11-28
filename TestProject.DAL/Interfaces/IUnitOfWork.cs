using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.DAL.Entities;

namespace TestProject.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        void Save();
    }
}
