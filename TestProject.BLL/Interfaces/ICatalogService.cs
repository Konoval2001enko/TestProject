using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.BLL.DTO;
using TestProject.DAL.Entities;

namespace TestProject.BLL.Interfaces
{
    public interface ICatalogService
    {
        void AddProduct(ProductDTO productDTO);
        void UpdateProduct(ProductDTO productDTO);
        void DeleteProduct(int? id);
        void AddCategory(CategoryDTO categoryDTO);
        void UpdateCategory(CategoryDTO categoryDTO);
        void DeleteCategory(int? id);
        ProductDTO GetProduct(int? id);
        IEnumerable<ProductDTO> GetSpecificProducts(int? id);
        IEnumerable<ProductDTO> SearchProducts(string name);
        IEnumerable<ProductDTO> FilterProducts(string minValue,string maxValue );
        CategoryDTO GetCategory(int? id);
        void ValidateProduct(ProductDTO productDTO, Category category);
        IEnumerable<ProductDTO> GetProducts();
        IEnumerable<CategoryDTO> GetCategories();
        void Dispose();
        Task Login(Microsoft.AspNetCore.Http.HttpContext context, String login, String password);
        Task Logout(Microsoft.AspNetCore.Http.HttpContext context);
        String GetUserRole(String name);
    }
}
