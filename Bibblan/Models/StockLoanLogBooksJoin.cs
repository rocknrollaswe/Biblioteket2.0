using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class StockLoanLogBooksJoin
    {
        public int? Pending { get; set; }
        public int Stockid { get; set; }
        public int Userid { get; set; }
        public DateTime Loandate { get; set; }
        public DateTime Returndate { get; set; }
        public int Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
    }
}
