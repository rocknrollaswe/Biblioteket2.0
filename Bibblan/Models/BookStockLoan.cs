using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class BookStockLoan
    {
        public int Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int Edition { get; set; }
        public int Ddk { get; set; }
        public string Sab { get; set; }
        public string Publisher { get; set; }
        public int Category { get; set; }
        public int Stockid { get; set; }
        public string Comment { get; set; }
        public string Condition { get; set; }
        public int? Discarded { get; set; }
        public int? Available { get; set; }
    }
}
