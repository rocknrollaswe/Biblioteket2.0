using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class BookStockLoan
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Ddk { get; set; }
        public int Category { get; set; }
        public int? Available { get; set; }
    }
}
