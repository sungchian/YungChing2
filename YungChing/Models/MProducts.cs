using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YungChing.Models
{
    public class MProducts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public string UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
    }
}
