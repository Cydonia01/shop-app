// Purpose: Contains the ProductListViewModel class which is used to display the products in the view.

using System;
using System.Collections.Generic;
using shopapp.entity;

namespace shopapp.webui.Models {

    public class PageInfo {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

        public int TotalPages() {
            return (int) Math.Ceiling((decimal)TotalItems/ItemsPerPage);
        }
    }

    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
    }
}