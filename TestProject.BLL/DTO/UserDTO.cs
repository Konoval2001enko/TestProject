using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DAL.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
    }
}
