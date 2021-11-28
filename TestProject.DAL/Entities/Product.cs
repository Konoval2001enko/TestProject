using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public virtual Category  Category{ get; set; }
        public string Discription { get; set; }
        public float Price { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
