using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Data;
using Alpha.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alpha
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (AlphaDbContext db = new AlphaDbContext())
            {
                if (!db.Roles.Any())
                {
                    db.Roles.AddRange(
                        new Role () { Name = "Manager" },
                        new Role () { Name = "Employee" }
                    );
                    db.SaveChanges();
                }
                int managerRoleId = db.Roles.FirstOrDefault(e => e.Name == "Manager").Id;
                int employeeRoleId = db.Roles.FirstOrDefault(e => e.Name == "Employee").Id;

                if (!db.Users.Any())
                {
                    db.Users.AddRange(
                        new User()
                        {
                            Name = "Jerry Goldsmith",
                            Email = "goldsmith@qwerty1234.ru",
                            RoleId = managerRoleId,
                            Password = "goldsmith1234"
                        },
                        new User()
                        {
                            Name = "Thomas Newman",
                            Email = "newman@qwerty1234.ru",
                            RoleId = employeeRoleId,
                            Password = "newman1234"
                        }
                    );
                    db.SaveChanges();
                }

                if (!db.Rooms.Any())
                {
                    db.Rooms.AddRange(
                        new Room()
                        {
                            Name = "Room #1",
                            Descrtiption = "Conference room B is located on the second floor on the historic side of the property. The Carpender family's living room was reinvented for today's medium-sized groups. The room features traditional Tudor-styling, large picture windows, and a sun porch which can function as a separate break out area for smaller group sessions or casual conversation.",
                            Seats = 10,
                            Projector = true,
                            Board = true
                        },
                        new Room()
                        {
                            Name = "Room #2",
                            Descrtiption = "This conference room, located on the second floor of the historic side of the property, features a charming original fireplace, floor to ceiling windows, and a flat-screen monitor for projecting right from a laptop or other mobile device. This room is well-suited for meetings, executive conferences, and social events alike.",
                            Seats = 20,
                            Projector = true,
                            Board = false
                        },
                        new Room()
                        {
                            Name = "Room #3",
                            Descrtiption = "Located on the third floor, this room features a flat-screen monitor for projecting materials from your laptop or mobile device. Conference room D is a bright and airy space perfect for mid-sized meetings. imilar to Conference Room D, this light and bright third-floor room features a flat-screen monitor and is great for mid-sized meetings.",
                            Seats = 30,
                            Projector = false,
                            Board = false
                        },
                        new Room()
                        {
                            Name = "Room #4",
                            Descrtiption = "Tucked away on the first floor of the historic side of the property, this room is a quiet space that invites brainstorming in a more flexible setting. Large windows brighten the finely appointed space, offering a panorama of lush woods and greenery, and creating a pleasingly open and relaxing environment. The 100-seat room is also home to the Carpender Family baby grand piano, played during social events by professional pianists and students of the Mason Gross School of the Arts.",
                            Seats = 40,
                            Projector = false,
                            Board = true
                        },
                        new Room()
                        {
                            Name = "Room #5",
                            Descrtiption = "Appointed with soft seating and a conference table for 4, this room is great for unstructured sessions or smaller, intimate groups looking for flexible space in which to create. A flat screen monitor is available for presentations, web-based training, or brainstorming. This newly renovated room features a sleek conference table and plush  seating for 8. With its very own flat screen monitor, whiteboard  and wireless printer/scanner, this room is perfect for board meetings and executive retreats.",
                            Seats = 50,
                            Projector = true,
                            Board = false
                        }
                    );
                    db.SaveChanges();
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
