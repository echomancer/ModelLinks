using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using ModelLinks.Models;

namespace ModelLinks.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
                    

            builder.Entity<Post>()
                .HasKey(c => c.PostId);

            builder.Entity<Comment>()
                 .HasKey(c => c.CommentId);
            
            builder.Entity<Post>()
                .HasMany<Comment>(p => p.Comments)
                .WithOne(p => p.Post)
                .IsRequired()
                .HasForeignKey(p => p.PostId);
            
            builder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(c => c.Comments)
                .IsRequired();

            builder.Entity<Comment>()
                .HasOne<ApplicationUser>(c => c.User);

            builder.Entity<Post>()
                .HasOne<ApplicationUser>(c => c.User);

            builder.Entity<ApplicationUser>()
                .HasMany<Post>(p => p.Posts)
                .WithOne(p => p.User)
                .IsRequired()
                .HasForeignKey(p => p.ApplicationUserId);

            builder.Entity<ApplicationUser>()
                .HasMany<Comment>(p => p.Comments)
                .WithOne(p => p.User)
                .IsRequired()
                .HasForeignKey(p => p.ApplicationUserId);

            
            
        }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Post> Post { get; set; }
    }
}
