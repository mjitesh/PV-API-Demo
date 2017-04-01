using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class ProductController : Controller
    {
          // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductList()
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.Token);
            var response = _client.GetAsync(AppSettings.ApiUrl+"/product/GetProducts");
            var content = response.Result.Content.ReadAsStringAsync().Result;

            var myList = JsonConvert.DeserializeObject<List<ProductModel>>(content).AsEnumerable();
            return View(myList);
        }
        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductModel _newproduct)
        {
          

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.ApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(JsonConvert.SerializeObject(_newproduct), Encoding.UTF8, "application/json");

                // HTTP POST
                HttpResponseMessage response = await client.PostAsync("/api/Product/AddProduct", content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    _newproduct = JsonConvert.DeserializeObject<ProductModel>(data);
                }


                return RedirectToAction("ProductList");
            }
           
        }

        public ActionResult Edit(int id)
        {
            ProductModel _selectdProductModel=new ProductModel();
          
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.Token);
            var response = _client.GetAsync(AppSettings.ApiUrl+"/product/GetProducts");
            var content = response.Result.Content.ReadAsStringAsync().Result;

            var myList = JsonConvert.DeserializeObject<List<ProductModel>>(content).AsEnumerable();
            _selectdProductModel=myList.FirstOrDefault(x => x.Id == id);

            return View("AddProduct",_selectdProductModel);
        }

        public ActionResult DeleteProduct(int productid)
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.Token);
            var response = _client.GetAsync(AppSettings.ApiUrl + "/product/DeleteProuct?productid="+productid);
            var content = response.Result.Content.ReadAsStringAsync().Result;

            var myList = JsonConvert.DeserializeObject<bool>(content);
            return RedirectToAction("ProductList");
            // DeleteProuct
        }

        public ActionResult AddNewProduct()
        {
            return View("AddProduct", new ProductModel(){Id = 0});
        }
        

    }
}