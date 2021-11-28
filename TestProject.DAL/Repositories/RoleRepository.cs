using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.DAL.Context;
using TestProject.DAL.Entities;
using TestProject.DAL.Interfaces;

namespace TestProject.DAL.Repositories
{
    class RoleRepository : IRepository<Role>
    {
        private CatalogContext db;

        public RoleRepository(CatalogContext context)
        {
            this.db = context;
        }

        public IEnumerable<Role> GetAll()
        {
            return db.Roles;
        }

        public Role Get(int id)
        {
            return db.Roles.Find(id);
        }

        public void Create(Role role)
        {
            db.Roles.Add(role);
        }

        public void Update(Role role)
        {
            db.Entry(role).State = EntityState.Modified;
        }

        public IEnumerable<Role> Find(Func<Role, Boolean> predicate)
        {
            return db.Roles.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Role role = db.Roles.Find(id);
            if (role != null)
                db.Roles.Remove(role);
        }
    }
}