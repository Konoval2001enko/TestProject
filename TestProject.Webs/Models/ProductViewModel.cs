using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Webs.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public string Discription { get; set; }
        public decimal Price { get; set; }
        public string GeneralNote { get; set; }
        public string SpecialNote { get; set; }
    }
}
