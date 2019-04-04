using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iGotUp.Api.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols.WsFederation;

namespace iGotUp.Api.Data
{
    public class GotUpSeeder
    {
        private readonly GotUpContext ctx;
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<GotUpUser> userManager;
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public GotUpSeeder(GotUpContext ctx, IHostingEnvironment hosting, UserManager<GotUpUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            this.ctx = ctx;
            this.hosting = hosting;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Seed()
        {
            this.ctx.Database.EnsureCreated();
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                var roleExist = await this.roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    await this.roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            var user = await this.userManager.FindByEmailAsync("aqwright@gmail.com");

            if (user == null)
            {
                user = new GotUpUser()
                {
                    UserName = "aqwright@gmail.com",
                    Email = "aqwright@gmail.com",
                    FirstName = "Anthony",
                    LastName = "Wright"
                };

                var result = await this.userManager.CreateAsync(user, "WarEagle!1");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
                else
                {
                    await this.userManager.AddToRoleAsync(user, "Admin");
                }
            }

            var game_type_count = this.ctx.GameTypes.Count();

            IEnumerable<GameType> types;
            if (game_type_count < 1)
            {
                types = new List<GameType>()
               {
                   new GameType(){description = "Full court four vs four", name = "4v4F"},
                   new GameType(){description = "Half court four vs four", name = "4v4H"},
                   new GameType(){description = "Five vs Five full court", name = "5v5F"}
               };

                this.ctx.GameTypes.AddRange(types);
                this.ctx.SaveChanges();
            }
        }
    }
}