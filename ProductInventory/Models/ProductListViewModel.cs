﻿namespace ProductInventory.Models
{
    public class ProductListViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
