using System;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
namespace UserManagement.Data
{
	public class ApiContext : DbContext
	{
		public DbSet<UserModel> Users { get; set; }

		public ApiContext(DbContextOptions<ApiContext> options)
			:base(options)
		{
		}
	}
}

