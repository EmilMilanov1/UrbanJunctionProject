using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
    public class PostImage
    {
        public int Id { get; set; }

        public string ImagePath { get; set; } = null!;

        public int PostId { get; set; }

        public Post Post { get; set; } = null!;
    }
}

