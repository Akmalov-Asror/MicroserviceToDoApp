﻿using Micro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Micro.Domain.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }
    public DbSet<User> Users { get; set; }
    public DbSet<Micro.Domain.Entities.Task> Tasks { get; set; }
}