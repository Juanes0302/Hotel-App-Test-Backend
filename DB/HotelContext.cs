﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DB
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options)
            : base(options)
        {
        }
        public DbSet<rooms> Rooms { get; set; }
        public DbSet<guest> Guest { get; set; }
        public DbSet<records> Records { get; set; }
        public DbSet<login> Login { get; set; }
       
    }
}
