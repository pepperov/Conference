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

            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var context = services.GetRequiredService<AlphaDbContext>();
            //        AlphaDbInit.Initialize(context);
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while seeding the database.");
            //    }
            //}

            using (AlphaDbContext db = new AlphaDbContext())
            {
                if (!db.Users.Any())
                {
                    db.Users.AddRange(
                        new User()
                        {
                            Name = "Jerry Goldsmith",
                            Email = "admin@qwerty1234.ru",
                            Role = Role.Manager
                        },
                        new User()
                        {
                            Name = "Thomas Newman",
                            Email = "Newman@qwerty1234.ru",
                            Role = Role.Employee
                        },
                        new User()
                        {
                            Name = "Hans Zimmer",
                            Email = "Zimmer@qwerty1234.ru",
                            Role = Role.Employee
                        },
                        new User()
                        {
                            Name = "John Barry",
                            Email = "Barry@qwerty1234.ru",
                            Role = Role.Employee
                        },
                        new User()
                        {
                            Name = "Trent Reznor",
                            Email = "Reznor@qwerty1234.ru",
                            Role = Role.Employee
                        }
                    );
                    db.SaveChanges();
                    int UserId1 = db.Users.FirstOrDefault(e => e.Name == "Jerry Goldsmith").Id;
                    int UserId2 = db.Users.FirstOrDefault(e => e.Name == "Thomas Newman").Id;
                    int UserId3 = db.Users.FirstOrDefault(e => e.Name == "Hans Zimmer").Id;
                    int UserId4 = db.Users.FirstOrDefault(e => e.Name == "John Barry").Id;
                    int UserId5 = db.Users.FirstOrDefault(e => e.Name == "Trent Reznor").Id;

                    if (!db.Rooms.Any())
                    {
                        db.Rooms.AddRange(
                            new Room()
                            {
                                Name = "Room #1",
                                Descrtiption = "Room #1",
                                Seats = 10,
                                Projector = true,
                                Board = true
                            },
                            new Room()
                            {
                                Name = "Room #2",
                                Descrtiption = "Room #2",
                                Seats = 20,
                                Projector = true,
                                Board = false
                            },
                            new Room()
                            {
                                Name = "Room #3",
                                Descrtiption = "Room #3",
                                Seats = 30,
                                Projector = false,
                                Board = false
                            },
                            new Room()
                            {
                                Name = "Room #4",
                                Descrtiption = "Room #4",
                                Seats = 40,
                                Projector = false,
                                Board = true
                            },
                            new Room()
                            {
                                Name = "Room #5",
                                Descrtiption = "Room #5",
                                Seats = 50,
                                Projector = true,
                                Board = false
                            }
                        );
                        db.SaveChanges();
                    }
                    int RoomId1 = db.Rooms.FirstOrDefault(e => e.Name == "Room #1").Id;
                    int RoomId2 = db.Rooms.FirstOrDefault(e => e.Name == "Room #2").Id;
                    int RoomId3 = db.Rooms.FirstOrDefault(e => e.Name == "Room #3").Id;
                    int RoomId4 = db.Rooms.FirstOrDefault(e => e.Name == "Room #4").Id;
                    int RoomId5 = db.Rooms.FirstOrDefault(e => e.Name == "Room #5").Id;

                    if (!db.Reservations.Any())
                    {
                        db.Reservations.AddRange(
                            new Reservation()
                            {
                                Start = new DateTime(2019, 8, 1, 10, 0, 0),
                                End = new DateTime(2019, 8, 1, 11, 0, 0),
                                RoomId = RoomId1,
                                UserId = UserId2
                            },
                            new Reservation()
                            {
                                Start = new DateTime(2019, 8, 1, 12, 0, 0),
                                End = new DateTime(2019, 8, 1, 14, 0, 0),
                                RoomId = RoomId1,
                                UserId = UserId3
                            },
                            new Reservation()
                            {
                                Start = new DateTime(2019, 8, 2, 9, 0, 0),
                                End = new DateTime(2019, 8, 2, 15, 0, 0),
                                RoomId = RoomId3,
                                UserId = UserId4
                            },
                            new Reservation()
                            {
                                Start = new DateTime(2019, 8, 4, 16, 0, 0),
                                End = new DateTime(2019, 8, 4, 18, 0, 0),
                                RoomId = RoomId4,
                                UserId = UserId4
                            }
                        );
                        db.SaveChanges();
                    }
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
