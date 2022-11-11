using System;
using System.Collections.Generic;

#nullable disable

namespace MixAz.Models
{
    public partial class Category
    {
        public Category()
        {
            Posts = new HashSet<Post>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
