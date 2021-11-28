using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestProject.BLL.DTO;
using TestProject.BLL.Infrastructure;
using TestProject.BLL.Interfaces;
using TestProject.DAL.Context;
using TestProject.DAL.Entities;
using TestProject.DAL.Interfaces;

namespace TestProject.BLL.Services
{
    public class CatalogService : ICatalogService
    {
        IUnitOfWork Database { get; set; }

        public CatalogService(IUnitOfWork uow)
        {
            Database = uow;
        }

        private void ValidateProduct(ProductDTO productDTO, Category category)
        {
            if (category == null)
                throw new ValidationException("Категория не найдена", "");
            string validate = "";
            if (productDTO.Name == null)
                validate += "Имя не может быть пустым\n";
            if (productDTO.Discription == null)
                validate += "Описание не может быть пустым\n";
            if (productDTO.SpecialNote == null)
                validate += "Специальное примечание не может быть пустым\n";
            if (productDTO.GeneralNote == null)
                validate += "Общее примечание не может быть пустым\n";
            if (!validate.Equals(""))
                throw new ValidationException(validate, "");
        }

        public void ValidateCategory(CategoryDTO categoryDTO)
        {
            if (categoryDTO.Name == null)
                throw new ValidationException("Имя не может быть пустым", "");
            if (Database.Categories.Find(x => x.Name == categoryDTO.Name && x.Id != categoryDTO.Id).Count() > 0)
                throw new ValidationException("Категория с таким именем уже существует", "");
        }
        public void AddProduct(ProductDTO productDTO)
        {

            Category category = Database.Categories.Get(productDTO.Category.Id);
            ValidateProduct(productDTO, category);
            Product product = new Product
            {
                Name = productDTO.Name,
                Category = category,
                Discription = productDTO.Discription,
                Price = productDTO.Price,
                GeneralNote = productDTO.GeneralNote,
                SpecialNote = productDTO.SpecialNote
            };
            Database.Products.Create(product);
            Database.Save();
        }
        public void UpdateProduct(ProductDTO productDTO)
        {
            Category category = Database.Categories.Get(productDTO.Category.Id);
            ValidateProduct(productDTO, category);
            Product product = new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Category = category,
                Discription = productDTO.Discription,
                Price = productDTO.Price,
                GeneralNote = productDTO.GeneralNote,
                SpecialNote = productDTO.SpecialNote
            };
            Database.Products.Update(product);
            Database.Save();
        }
        public void DeleteProduct(int id)
        {
            Database.Products.Delete(id);
            Database.Save();
        }
        public IEnumerable<ProductDTO> GetProducts()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
            }
            ).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.GetAll());
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(Database.Categories.GetAll());
        }
        public IEnumerable<ProductDTO> GetSpecificProducts(int id)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Category, CategoryDTO>();
            }
            ).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.Find(x => x.CategoryId == id));
        }

        public IEnumerable<ProductDTO> SearchProducts(string name)
        {
            if (name == null) name = "";
            var mapper = new MapperConfiguration(cfg =>

                {
                    cfg.CreateMap<Product, ProductDTO>();
                    cfg.CreateMap<Category, CategoryDTO>();
                }
            ).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.Find(x => x.Name.ToLower().Contains(name.ToLower())));
        }

        public IEnumerable<ProductDTO> FilterProducts(string minValue, string maxValue)
        {
            try
            {
                double _minValue = minValue != null ? double.Parse(minValue) : 0.0;
                double _maxValue = maxValue != null ? double.Parse(maxValue) : double.MaxValue;


                var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDTO>();
                    cfg.CreateMap<Category, CategoryDTO>();
                }
            ).CreateMapper();
                return mapper.Map<IEnumerable<Product>, List<ProductDTO>>(Database.Products.Find(x => x.Price >= _minValue && x.Price <= _maxValue));
            }
            catch (FormatException ex)
            {
                throw new ValidationException("Не корректное значение для фильтрации", "");
            }
        }

        public ProductDTO GetProduct(int id)
        {
            var product = Database.Products.Get(id);
            if (product == null)
                throw new ValidationException("Продукт не найден", "");

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Category = new CategoryDTO() { Name = product.Category.Name, Id = product.Category.Id },
                Discription = product.Discription,
                Price = product.Price,
                GeneralNote = product.GeneralNote,
                SpecialNote = product.SpecialNote
            };
        }

        public CategoryDTO GetCategory(int id)
        {
            var category = Database.Categories.Get(id);
            if (category == null)
                throw new ValidationException("Категория не найдена", "");

            return new CategoryDTO
            {
                Name = category.Name,
                Id = category.Id
            };
        }

        public void AddCategory(CategoryDTO categoryDTO)
        {
            ValidateCategory(categoryDTO);
            Category category = new Category
            {
                Name = categoryDTO.Name
            };
            Database.Categories.Create(category);
            Database.Save();
        }
        public void UpdateCategory(CategoryDTO categoryDTO)
        {
            ValidateCategory(categoryDTO);
            Category categoryUpdate = Database.Categories.Get(categoryDTO.Id);
            categoryUpdate.Id = categoryDTO.Id;
            categoryUpdate.Name = categoryDTO.Name;
            Database.Categories.Update(categoryUpdate);
            Database.Save();
        }
        public void DeleteCategory(int id)
        {
            Database.Categories.Delete(id);
            Database.Save();
        }
        public async Task Login(Microsoft.AspNetCore.Http.HttpContext context, String login, String password)
        {
            var users = Database.Users.Find(x => x.Login.Equals(login) && x.Password.Equals(password));

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<Role, RoleDTO>();
            }
            ).CreateMapper();
            if (users.Count() != 1)
                throw new ValidationException("Неверный логин или пароль", "");
            var userDTO = mapper.Map<User, UserDTO>(users.First());

            if (users.Count() == 1)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userDTO.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userDTO.Role.Name)
            };
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                await AuthenticationHttpContextExtensions.SignInAsync(context, new ClaimsPrincipal(id));

            }
            else
                throw new ValidationException("Неверный логин или пароль", "");
        }
        public async Task Logout(Microsoft.AspNetCore.Http.HttpContext context)
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(context);
        }
        public String GetUserRole(String name)
        {
            return Database.Users.Find(x => x.Login.Equals(name)).FirstOrDefault().Role.Name;
        }
    }
}
