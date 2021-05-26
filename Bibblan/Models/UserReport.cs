using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class UserReport
    {
        public string Title { get; set; }
        public string Email { get; set; }
        public DateTime Returndate { get; set; }
        public int StockId { get; set; }
    }
}
