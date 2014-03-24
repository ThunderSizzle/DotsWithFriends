using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DotsWithFriends.Models
{
	public class MyUser : IdentityUser
	{
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public Profile Profile { get; set; }
		[EmailAddress]
		public String Email { get; set; }
		public ICollection<Connection> Connections { get; set; }

		public MyUser()
			: base()
		{
			this.Connections = new Collection<Connection>();
		}
	}

	public class Context : IdentityDbContext<MyUser>
	{
		public DbSet<Box> Boxes { get; set; }
		public DbSet<Coordinate> Coordinates { get; set; }
		public DbSet<DatabaseObject> DatabaseObject { get; set; }
		public DbSet<Game> Games { get; set; }
		public DbSet<Line> Lines { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Turn> Turns { get; set; }

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );

			modelBuilder.Entity<IdentityUser>()
				.ToTable( "Users" );
			modelBuilder.Entity<MyUser>()
				.ToTable( "Users" );

			modelBuilder.Entity<DatabaseObject>().ToTable( "DatabaseObjects" ).
				Map<Game>( m =>
				{
					m.ToTable( "Games" );
				} ).
				Map<Grid>( m =>
				{
					m.ToTable( "Grids" );
				} ).
				Map<Line>( m =>
				{
					m.ToTable( "Lines" );
				} ).
				Map<Player>( m =>
				{
					m.ToTable( "Players" );
				} ).
				Map<Profile>( m =>
				{
					m.ToTable( "Profiles" );
				} ).
				Map<Turn>( m =>
				{
					m.ToTable( "Turn" );
				} ).
				Map<Box>( m =>
				{
					m.ToTable( "Boxes" );
				} ).
				Map<Coordinate>( m =>
				{
					m.ToTable( "Coordinates" );
				} );
			modelBuilder.Entity<IdentityUserLogin>().Property( c => c.ProviderKey).HasMaxLength(1024);
			modelBuilder.Entity<Profile>().HasRequired<MyUser>( c => c.User ).WithOptional( c => c.Profile );
		}
	}
}