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

        public async Task<IActionResult> QueryProdById(int id)
        {
            string connectionString = getAppSetting.getDataBseStr();

            var sqlString = @"
                SELECT [ProductID]
                              ,[ProductName]
                              ,[SupplierID]
                              ,[CategoryID]
                              ,[QuantityPerUnit]
                              ,[UnitPrice]
                              ,[UnitsInStock]
                              ,[UnitsOnOrder]
                FROM [Northwind].[dbo].[Products]
                WHERE [ProductID] = @id";

            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString, connect))
                {
                    connect.Open();

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    SqlDataReader test = cmd.ExecuteReader();

                    var maintainProd = new MProducts();

                    while (test.Read())
                    {
                        maintainProd.Id = Convert.ToInt32(test["ProductID"]);
                        maintainProd.Name = Convert.ToString(test["ProductName"]);
                        maintainProd.SupplierID = Convert.ToInt32(test["SupplierID"]);
                        maintainProd.CategoryID = Convert.ToInt32(test["CategoryID"]);
                        maintainProd.QuantityPerUnit = Convert.ToString(test["QuantityPerUnit"]);
                        maintainProd.UnitPrice = Convert.ToString(test["UnitPrice"]);
                        maintainProd.UnitsInStock = Convert.ToInt32(test["UnitsInStock"]);
                        maintainProd.UnitsOnOrder = Convert.ToInt32(test["UnitsOnOrder"]);

                    }
                    return Json(maintainProd);
                }
            }
        }


        //新增功能
        public async Task<string> InsertProd(MProducts prod)
        {
            try
            {
                string connectionString = getAppSetting.getDataBseStr();

                var sqlString = @"INSERT INTO [Northwind].[dbo].[Products]([ProductName], [SupplierID], [CategoryID], [QuantityPerUnit], [UnitPrice], [UnitsInStock], [UnitsOnOrder]) 
                                                    values(@name, @supplierId, @categoryId, @quantityPerUnit,  @unitPrice, @unitsInStock, @unitsOnOrder);SELECT SCOPE_IDENTITY();";

                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlString, connect))
                    {
                        connect.Open();

                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = prod.Name;

                        cmd.Parameters.Add("@supplierId", SqlDbType.Int).Value = prod.SupplierID;

                        cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = prod.CategoryID;

                        cmd.Parameters.Add("@quantityPerUnit", SqlDbType.NVarChar).Value = prod.QuantityPerUnit;

                        cmd.Parameters.Add("@unitPrice", SqlDbType.Money).Value = prod.UnitPrice;

                        cmd.Parameters.Add("@unitsInStock", SqlDbType.SmallInt).Value = prod.UnitsInStock;

                        cmd.Parameters.Add("@unitsOnOrder", SqlDbType.SmallInt).Value = prod.UnitsOnOrder;

                        //會傳遞一個字串，代表要插入資料表中的新值
                        int test = decimal.ToInt32((decimal)cmd.ExecuteScalar());

                        if (connect.State == ConnectionState.Open)
                        {
                            connect.Close();
                        }

                        if (test != 0)
                        {
                            return "success";
                        }
                        else
                        {
                            return "failed";
                        }
                        //dataReader ["欄位名稱"].ToString()    資料庫的資料
                    }
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        //編輯功能
        public async Task<bool> UpdateProd(int Id, string name, string supplierId, string categoryId, string quantityPerUnit, string unitPrice, string unitsInStock, string unitsOnOrder)
        {
            try
            {
                string connectionString = getAppSetting.getDataBseStr();
                
                var sqlStr = "UPDATE [Northwind].[dbo].[Products]  SET [ProductName] = @name, [SupplierID] = @supplierId, [CategoryID] = @categoryId, [QuantityPerUnit] = @quantityPerUnit, [UnitPrice] = @unitPrice, [UnitsInStock] = @unitsInStock, [UnitsOnOrder] = @unitsOnOrder  WHERE [ProductID] = @Id ";
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlStr, connect))
                    {
                        connect.Open();

                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(name) ? DBNull.Value : (object)name;
                        cmd.Parameters.Add("@supplierId", SqlDbType.Int).Value = string.IsNullOrEmpty(supplierId) ? DBNull.Value : (object)supplierId;
                        cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = string.IsNullOrEmpty(categoryId) ? DBNull.Value : (object)categoryId;
                        cmd.Parameters.Add("@quantityPerUnit", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(quantityPerUnit) ? DBNull.Value : (object)quantityPerUnit;
                        cmd.Parameters.Add("@unitPrice", SqlDbType.Money).Value = string.IsNullOrEmpty(unitPrice) ? DBNull.Value : (object)unitPrice;
                        cmd.Parameters.Add("@unitsInStock", SqlDbType.SmallInt).Value = string.IsNullOrEmpty(unitsInStock) ? DBNull.Value : (object)unitsInStock;
                        cmd.Parameters.Add("@unitsOnOrder", SqlDbType.SmallInt).Value = string.IsNullOrEmpty(unitsOnOrder) ? DBNull.Value : (object)unitsOnOrder;
                        int row = cmd.ExecuteNonQuery();
                        if (row > 0)

                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }


        //刪除功能
        public async Task<string> DeleteProd(int Id)
        {
            try
            {
                string connectionString = getAppSetting.getDataBseStr();

                var sqlString = @"DELETE FROM [Northwind].[dbo].[Products] WHERE [ProductID] = @id";

                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlString, connect))
                    {
                        connect.Open();

                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;

                        int row = cmd.ExecuteNonQuery();

                        if (row > 0)
                        {
                            return "success";
                        }
                        else
                        {
                            return "fail";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}
