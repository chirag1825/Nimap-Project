using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nimap.Models
{
    public class PagedProductCategoryViewModel
    {
        public List<ProductCategoryViewModel> Products { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}