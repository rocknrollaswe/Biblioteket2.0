using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class Loanlog
    {
        public int UserId { get; set; }
        public int StockId { get; set; }
        public DateTime Loandate { get; set; }
        public DateTime Returndate { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual User User { get; set; }
    }
}
