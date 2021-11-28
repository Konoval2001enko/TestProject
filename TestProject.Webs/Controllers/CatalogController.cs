using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestProject.BLL.DTO;
using TestProject.BLL.Infrastructure;
using TestProject.BLL.Interfaces;
using TestProject.DAL.Entities;
using TestProject.Webs.Models;

namespace TestProject.Webs.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ILogger<CatalogController> _logger;

        private readonly ICatalogService _catalogService;
        public CatalogController(ILogger<CatalogController> logger,ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }
        
        public IActionResult Products()
        {
            IEnumerable<ProductDTO> productDTOs = _catalogService.GetProducts();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();
            ViewBag.Products = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(productDTOs);
            ViewBag.User = _catalogService.GetUserRole(User.Identity.Name);
            return View();
        }
        [Authorize(Roles = "Administrator, AdvanceUser")]
        public IActionResult AddProduct()
        {
            IEnumerable<CategoryDTO> categoryDTOs = _catalogService.GetCategories();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, CategoryViewModel>()).CreateMapper();
            ViewBag.Categories = mapper.Map<IEnumerable<CategoryDTO>, List<CategoryViewModel>>(categoryDTOs);
            return View();
        }

        [Authorize(Roles = "Administrator, AdvanceUser")]
        [HttpPost]
        public IActionResult AddNewProduct(string name,int categoryId,string discription, String price, string generalNote,string specialNote )
        {
            try
            {
                var category = _catalogService.GetCategory(categoryId);
                if (price.Contains("."))
                {
                    price=price.Replace(".", ",");
                }
      
                var productDTO = new ProductDTO { Name = name, Category = category, Discription=discription, Price= float.Parse(price), GeneralNote=generalNote, SpecialNote=specialNote };
                _catalogService.AddProduct(productDTO);
                return Redirect("/Catalog/Products");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator, AdvanceUser")]
        public IActionResult UpdateProduct(int id)
        {
            IEnumerable<CategoryDTO> categoryDTOs = _catalogService.GetCategories();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, CategoryViewModel>())
            .CreateMapper();
            ViewBag.Categories = mapper.Map<IEnumerable<CategoryDTO>, List<CategoryViewModel>>(categoryDTOs);


            ProductDTO productDTO = _catalogService.GetProduct(id);
            mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()
            .ForMember(m=>m.CategoryId, opt => opt.MapFrom(src =>src.Category.Id))).CreateMapper();
            ViewBag.Product = mapper.Map<ProductDTO, ProductViewModel>(productDTO);
            return View();
        }

        [Authorize(Roles = "Administrator, AdvanceUser")]
        [HttpPost]
        public IActionResult UpdateNewProduct(int id,string name, int categoryId, string discription, String price, string generalNote, string specialNote)
        {
            try
            {
                var category = _catalogService.GetCategory(categoryId);
                var productDTO = new ProductDTO {Id=id, Name = name, Category = category, Discription = discription, Price = float.Parse(price.Replace(".", ",")), GeneralNote = generalNote, SpecialNote = specialNote };
                _catalogService.UpdateProduct(productDTO);
                return Redirect("/Catalog/Products");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator, AdvanceUser")]
        public IActionResult DeleteProduct(int id)
        {

            ProductDTO productDTO = _catalogService.GetProduct(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()
            .ForMember(m => m.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
            .ForMember(m => m.CategoryName, opt => opt.MapFrom(src => src.Category.Name))).CreateMapper();
            ViewBag.Product = mapper.Map<ProductDTO, ProductViewModel>(productDTO);
            return View();
        }

        [Authorize(Roles = "Administrator, AdvanceUser")]
        [HttpPost]
        public IActionResult DeleteProductPost(int id)
        {
            try
            {
                _catalogService.DeleteProduct(id);
                return Redirect("/Catalog/Products");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public IActionResult Categories()
        {
            IEnumerable<CategoryDTO> categoryDTOs = _catalogService.GetCategories();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, CategoryViewModel>()).CreateMapper();
            ViewBag.Categories = mapper.Map<IEnumerable<CategoryDTO>, List<CategoryViewModel>>(categoryDTOs);
            ViewBag.User = _catalogService.GetUserRole(User.Identity.Name);

            return View();
        }
        [Authorize(Roles ="Administrator")]
        public IActionResult AddCategory()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AddNewCategory(string name)
        {
            try
            {
                var categoryDTO = new CategoryDTO { Name = name };
                _catalogService.AddCategory(categoryDTO);
                return Redirect("/Catalog/Categories");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult UpdateCategory(int id)
        {
            CategoryDTO categoryDTO = _catalogService.GetCategory(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, CategoryViewModel>()).CreateMapper();
            ViewBag.Category = mapper.Map<CategoryDTO, CategoryViewModel>(categoryDTO);
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult UpdateNewCategory(int id, string name)
        {
            try
            {
                var categoryDTO = new CategoryDTO
                {
                    Id = id,
                    Name = name
                };
                _catalogService.UpdateCategory(categoryDTO);
                return Redirect("/Catalog/Categories");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteCategory(int id)
        {

            CategoryDTO categoryDTO = _catalogService.GetCategory(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<CategoryDTO, CategoryViewModel>()).CreateMapper();
            ViewBag.Category = mapper.Map<CategoryDTO, CategoryViewModel>(categoryDTO);
            return View();
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult DeleteCategoryPost(int id)
        {
            try
            {
                _catalogService.DeleteCategory(id);
                return Redirect("/Catalog/Categories");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (FormatException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult SpecificCategory(int id)
        {
            IEnumerable<ProductDTO> productDTOs = _catalogService.GetSpecificProducts(id);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();
            ViewBag.Products = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(productDTOs);
            ViewBag.Category = _catalogService.GetCategory(id).Name;
            ViewBag.User = _catalogService.GetUserRole(User.Identity.Name);
            return View();
        }

        public IActionResult SearchProducts(string name = "")
        {
            IEnumerable<ProductDTO> productDTOs = _catalogService.SearchProducts(name);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();
            ViewBag.Products = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(productDTOs);
            ViewBag.Name = name;
            ViewBag.User = _catalogService.GetUserRole(User.Identity.Name);
            return View();
        }

        public IActionResult FilterProducts(string minValue, string maxValue)
        {
            IEnumerable<ProductDTO> productDTOs = _catalogService.FilterProducts(minValue,maxValue);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ProductDTO, ProductViewModel>()).CreateMapper();
            ViewBag.Products = mapper.Map<IEnumerable<ProductDTO>, List<ProductViewModel>>(productDTOs);
            ViewBag.MinValue = minValue;
            ViewBag.MaxValue = maxValue;
            ViewBag.User = _catalogService.GetUserRole(User.Identity.Name);
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Logining(String login, String password)
        {
            try
            {
                await _catalogService.Login(HttpContext,login, password);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Redirect("/Catalog/Products");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _catalogService.Logout(HttpContext);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Redirect("/Catalog/Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
