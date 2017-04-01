using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MVC.Models
{
    
    public class ProductModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public double? Cost { get; set; }
        public int? CategoryId { get; set; }
        public string productImage { get; set; }
        public int? Stock { get; set; }
        public bool IsInOffer { get; set; }
        public string CategoryName { get; set; }

        public static List<SelectListItem> GetCategoryList()
        {
            HttpClient _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettings.Token);
            var response = _client.GetAsync(AppSettings.ApiUrl + "/product/GetCategoriesList");
            var content = response.Result.Content.ReadAsStringAsync().Result;

            var myList = JsonConvert.DeserializeObject<List<SelectListItem>>(content).AsEnumerable();
            return myList.ToList();
        } 
    }
}