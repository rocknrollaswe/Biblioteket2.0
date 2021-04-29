using System;
using System.Collections.Generic;

#nullable disable

namespace Bibblan.Models
{
    public partial class Category
    {
        public Category()
        {
            Books = new HashSet<Book>();
        }

        public int CategoryId { get; set; }
        public string CategoryDescription { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
