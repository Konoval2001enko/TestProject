using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.BLL.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryDTO Category { get; set; }
        public string Discription { get; set; }
        public float Price { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
