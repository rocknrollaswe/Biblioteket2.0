using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class Book
    {
        public Book()
        {
            Stocks = new HashSet<Stock>();
        }

        public int Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime Edition { get; set; }
        public int Ddk { get; set; }
        public string Sab { get; set; }
        public string Publisher { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
