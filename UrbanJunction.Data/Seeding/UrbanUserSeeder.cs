using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanJunction.Data.Models;

namespace UrbanJunction.Data.Seeding
{
    public class UrbanUserSeeder
    {
        public static IEnumerable<UrbanUser> SeedUsers()
        {
            var hasher = new PasswordHasher<UrbanUser>();

            return new List<UrbanUser>()
            {
                new UrbanUser
                {
                    Id = "93e5df7b-fb35-46d7-bd8c-7b88546ac77e",
                    UserName = "Emo",
                    NormalizedUserName = "EMO",
                    Email = "artlover@urban.com",
                    NormalizedEmail = "ARTLOVER@URBAN.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "ArtLover123!")
                },
                new UrbanUser
                {
                    Id = "3ad674e3-3797-41ba-b980-9b2e85c32a51",
                    UserName = "Valio",
                    NormalizedUserName = "VALIO",
                    Email = "musicfan@urban.com",
                    NormalizedEmail = "MUSICFAN@URBAN.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "MusicFan123!")
                },
                new UrbanUser
                {
                    Id = "c5859895-19f2-47da-ae19-569400ee20d5",
                    UserName = "Mr.Yanev",
                    NormalizedUserName = "MR.YANEV",
                    Email = "fashionguru@urban.com",
                    NormalizedEmail = "FASHIONGURU@URBAN.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Fashion123!")
                }
            };
        }
    }
}
