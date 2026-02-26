using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
    public class UserDTO
    {
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UrbanUserSeeder
    {
        public static IEnumerable<UserDTO> GetUsers()
        {
            return new List<UserDTO>()
            {
                new UserDTO
                {
                    UserName = "Emo",
                    Email = "artlover@urban.com",
                    Password = "ArtLover123!"
                },
                new UserDTO
                {
                    UserName = "Admin",
                    Email = "admin@urban.com",
                    Password = "Admin123!"
                },
                new UserDTO
                {
                    UserName = "Valio",
                    Email = "musicfan@urban.com",
                    Password = "MusicFan123!"
                },
                new UserDTO
                {
                    UserName = "Mr.Yanev",
                    Email = "fashionguru@urban.com",
                    Password = "Fashion123!"
                }
            };
        }
    }
}
