using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public int Isbn { get; set; }

        public virtual Book IsbnNavigation { get; set; }
    }
}
