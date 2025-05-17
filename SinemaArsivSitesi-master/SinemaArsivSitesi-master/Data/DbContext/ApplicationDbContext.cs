using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using SinemaArsivSitesi.Controllers;
using SinemaArsivSitesi.Models;

namespace SinemaArsivSitesi.Data.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=yourdatabase.db");
            }

            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFavoriteMovie> UserFavoriteMovies { get; set; }
        public DbSet<RelatedMovie> RelatedMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserFavoriteMovie>()
                .HasKey(ufm => new { ufm.UserId, ufm.MovieId });

            builder.Entity<UserFavoriteMovie>()
                .HasOne(ufm => ufm.User)
                .WithMany(u => u.FavoriteMovies)
                .HasForeignKey(ufm => ufm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFavoriteMovie>()
                .HasOne(ufm => ufm.Movie)
                .WithMany(m => m.UserFavoriteMovies)
                .HasForeignKey(ufm => ufm.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RelatedMovie>()
                .HasKey(rm => new { rm.MovieId, rm.RelatedMovieId });

            builder.Entity<RelatedMovie>()
                .HasOne(rm => rm.Movie)
                .WithMany(m => m.RelatedMovies)
                .HasForeignKey(rm => rm.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RelatedMovie>()
                .HasOne(rm => rm.Related)
                .WithMany(m => m.RelatedToMovies)
                .HasForeignKey(rm => rm.RelatedMovieId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.Entity<Comment>()
                .HasOne(c => c.Movie)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MovieId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Bilim Kurgu" },
                new Category { Id = 2, Name = "Komedi" },
                new Category { Id = 3, Name = "Aksiyon" },
                new Category { Id = 4, Name = "Dram" }
            );

            builder.Entity<Movie>().HasData(
    new Movie
    {
        Id = 1,
        Title = "Tenet",
        PosterUrl = "https://tr.web.img3.acsta.net/pictures/20/08/28/14/59/2855138.jpg",
        Genre = "Aksiyon, Bilim Kurgu, Gerilim",
        Language = "İngilizce",
        Duration = "150",
        IMDBRating = 7.3m,
        Year = 2020,
        Description = "Tenet kelimesiyle silahlanmış bir ajan, küresel casusluk dünyasında görev alıyor.",
        VideoUrl = "https://www.youtube.com/watch?v=L3pk_TBwu3M",
        Director = "Christopher Nolan",
        CategoryId = 1,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 2,
        Title = "Inception",
        PosterUrl = "https://fullhdfilmizlede.net/uploads/filmler/baslangic-inception.jpg",
        Genre = "Bilim Kurgu, Aksiyon, Gerilim",
        Language = "İngilizce",
        Duration = "148",
        IMDBRating = 8.8m,
        Year = 2010,
        Description = "Dom Cobb, insanların rüyalarına girerek bilinçaltlarından sır çalabilen bir hırsızdır.",
        VideoUrl = "https://www.youtube.com/watch?v=YoHD9XEInc0",
        Director = "Christopher Nolan",
        CategoryId = 1,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 3,
        Title = "Monty Python and the Holy Grail",
        PosterUrl = "https://tr.web.img3.acsta.net/medias/nmedia/18/80/37/57/19546984.jpg",
        Genre = "Absürt Komedi, Parodi",
        Language = "İngilizce",
        Duration = "91",
        IMDBRating = 8.2m,
        Year = 1975,
        Description = "Kral Arthur ve Yuvarlak Masa Şövalyeleri'nin Kutsal Kase'yi bulmak için çıktıkları yolculuk.",
        VideoUrl = "https://www.youtube.com/watch?v=LGq3OnOYZ5E",
        Director = "Terry Gilliam, Terry Jones",
        CategoryId = 2,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 4,
        Title = "The Grand Budapest Hotel",
        PosterUrl = "https://m.media-amazon.com/images/M/MV5BMzM5NjUxOTEyMl5BMl5BanBnXkFtZTgwNjEyMDM0MDE@._V1_.jpg",
        Genre = "Komedi, Macera, Suç",
        Language = "İngilizce",
        Duration = "99",
        IMDBRating = 8.1m,
        Year = 2014,
        Description = "Ünlü bir otelin konsiyerji Gustave H. ve çalışanı Zero'nun çalınan bir tabloyla ilgili macerası.",
        VideoUrl = "https://www.youtube.com/watch?v=1Fg5iWmQjfk",
        Director = "Wes Anderson",
        CategoryId = 2,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 5,
        Title = "Mad Max: Fury Road",
        PosterUrl = "https://play-lh.googleusercontent.com/IAFsZ8rqbpzrwuavU_ZkgPmxFiB-0lxM3na4kYnvm4qeOcA5J5rZ5E7ue2uPjLvZhCoylQ",
        Genre = "Aksiyon, Macera, Bilim Kurgu",
        Language = "İngilizce",
        Duration = "120",
        IMDBRating = 8.1m,
        Year = 2015,
        Description = "Max, Immortan Joe'dan kaçan Furiosa ve diğerleriyle özgürlüğe kaçmaya çalışır.",
        VideoUrl = "https://www.youtube.com/watch?v=hEJnMQG9ev8",
        Director = "George Miller",
        CategoryId = 3,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 6,
        Title = "Die Hard",
        PosterUrl = "https://tr.web.img2.acsta.net/pictures/bzp/01/4019.jpg",
        Genre = "Aksiyon, Gerilim",
        Language = "İngilizce",
        Duration = "132",
        IMDBRating = 8.2m,
        Year = 1988,
        Description = "John McClane, teröristlerce rehin alınan karısını kurtarmaya çalışır.",
        VideoUrl = "https://www.youtube.com/watch?v=2TQ-pOvI6Xo",
        Director = "John McTiernan",
        CategoryId = 3,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 7,
        Title = "The Shawshank Redemption",
        PosterUrl = "https://m.media-amazon.com/images/I/61-vQDr7ecL._AC_UF894,1000_QL80_.jpg",
        Genre = "Dram",
        Language = "İngilizce",
        Duration = "142",
        IMDBRating = 9.3m,
        Year = 1994,
        Description = "Andy Dufresne'nin Shawshank Hapishanesi'ndeki yılları ve dostluğu anlatılır.",
        VideoUrl = "https://www.youtube.com/watch?v=6hB3S9bIaco",
        Director = "Frank Darabont",
        CategoryId = 4,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    },
    new Movie
    {
        Id = 8,
        Title = "The Godfather",
        PosterUrl = "https://m.media-amazon.com/images/M/MV5BNGEwYjgwOGQtYjg5ZS00Njc1LTk2ZGEtM2QwZWQ2NjdhZTE5XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
        Genre = "Suç, Dram",
        Language = "İngilizce",
        Duration = "175",
        IMDBRating = 9.2m,
        Year = 1972,
        Description = "Corleone ailesinin mafya dünyasındaki yükselişi ve aile içi çatışmaları.",
        VideoUrl = "https://www.youtube.com/watch?v=sY1S34973zA",
        Director = "Francis Ford Coppola",
        CategoryId = 4,
        CreatedDate = new DateTime(2024, 1, 1),
        UpdatedDate = new DateTime(2024, 1, 1)
    }
);

        }
    }
}
