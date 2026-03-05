using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanJunction.Data.Models
{
    public class UserFollow
    {
        public string FollowerId { get; set; } = null!;
        public UrbanUser Follower { get; set; } = null!;

        public string FollowingId { get; set; } = null!;
        public UrbanUser Following { get; set; } = null!;
    }
}