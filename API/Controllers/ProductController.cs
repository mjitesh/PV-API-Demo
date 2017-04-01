using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using API.Models;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class ProductController : ApiController
    {
        //[Authorize]
        public List<ProductDTO> GetProducts()
        {
            using (var ctx = new Models.demodbEntities1())
            {
                var _list = ctx.Products.Select(x => new ProductDTO() { Id = x.Id, Name = x.Name, Cost = x.Cost,IsInOffer = x.IsInOffer,CategoryName = x.ProductCategory.Name,Stock = x.Stock});
                return _list.ToList();

            }
        }

        [HttpPost]
        public ProductDTO AddProduct(ProductDTO product)
        {
            ProductDTO _productDto;
            using (var ctx = new Models.demodbEntities1())
            {
                if (product.Id == 0)
                {
                    ctx.Products.Add(new Product()
                    {
                        Id = 1,
                        CategoryId = product.CategoryId,
                        Stock = product.Stock,
                        Name = product.Name,
                        IsInOffer = product.IsInOffer,
                        Cost = product.Cost
                    });
                    ctx.SaveChanges();
                }

                else
                {
                    var selpro = ctx.Products.FirstOrDefault(x => x.Id == product.Id);
                    selpro.Name = product.Name;
                    selpro.IsInOffer = product.IsInOffer;
                    selpro.Cost = product.Cost;
                    selpro.Stock = product.Stock;
                    selpro.CategoryId = product.CategoryId;
                    ctx.SaveChanges();
                }
            }
            return null;


        }

        [HttpGet]
        public List<CategoryList> GetCategoriesList()
        {
            using (var ctx = new Models.demodbEntities1())
            {
                var res = ctx.ProductCategories.Select(x => new CategoryList() { Text = x.Name, Value = x.Id.ToString() });
                return res.ToList();

            }

        }

        [HttpGet]
        public bool DeleteProuct(int productid)
        {
            using (var ctx = new Models.demodbEntities1())
            {
                var prod = ctx.Products.Where(x => x.Id == productid).FirstOrDefault();
                ctx.Products.Remove(prod);
                ctx.SaveChanges();
            }
            return true;
        }

        public class  ProductDTO:Product
        {
            public string CategoryName { get; set; }
           
        }

        public class CategoryList
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}
