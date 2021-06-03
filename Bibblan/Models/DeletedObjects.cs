using System;
using System.Collections.Generic;
using System.Text;

namespace Bibblan.Models
{
    public partial class DeletedObjects
    {
        public int StockId { get; set; }
        public int Isbn { get; set; }
        public string Title { get; set; }
        public int Edition { get; set; }
        public string Condition { get; set; }
        public int? Discarded { get; set; }
        public int? Available { get; set; }
    }
}
