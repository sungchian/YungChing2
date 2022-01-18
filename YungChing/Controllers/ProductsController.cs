using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using YungChing.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using YungChing.API;

namespace YungChing.Controllers
{
    public class ProductsController : Controller
    {
        GetAppSetting getAppSetting = new GetAppSetting();
        public IActionResult Index()
        {
            return View();
        }

        public List<MProducts> getAllProducts()
        {
            string connectionString = getAppSetting.getDataBseStr();

            var sqlString = @"SELECT * FROM dbo.Products";

            var ProductList = new List<MProducts>();

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connect))
                {
                    connect.Open();

                    SqlDataReader test = cmd.ExecuteReader();

                    while (test.Read())
                    {
                        var prod = new MProducts();
                        //dataReader ["欄位名稱"].ToString()    資料庫的資料
                        prod.Id = Convert.ToInt32(test["ProductID"]);
                        prod.Name = Convert.ToString(test["ProductName"]);
                        prod.SupplierID = Convert.ToInt32(test["SupplierID"]);
                        prod.CategoryID = Convert.ToInt32(test["CategoryID"]);
                        prod.QuantityPerUnit = Convert.ToString(test["QuantityPerUnit"]);
                        prod.UnitPrice = Convert.ToString(test["UnitPrice"]);
                        prod.UnitsInStock = Convert.ToInt32(test["UnitsInStock"]);
                        prod.UnitsOnOrder = Convert.ToInt32(test["UnitsOnOrder"]);
                        ProductList.Add(prod);

                    }

                    return ProductList;
                }
            }
        }

    }
}
