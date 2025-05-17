using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SinemaArsivSitesi.Data.DbContext;
using SinemaArsivSitesi.Models;
using SinemaArsivSitesi.Services.Category;
using SinemaArsivSitesi.Services.Movie;
using System.Text;

using SinemaArsivSitesi.Models;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=SinemaArsivSitesi.db"));

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sinema Arşiv API",
        Version = "v1",
        Description = "Sinema Arşiv Sitesi API"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sinema Arşiv API V1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Movies.Any())
    {
        var movies = new List<Movie>
        {
            new Movie
            {
                Title = "Tenet",
                Year = 2020,
                PosterUrl = "https://tr.web.img3.acsta.net/pictures/20/08/28/14/59/2855138.jpg",
                Description = "“Tenet” kelimesiyle silahlanmış bir CIA ajanı, gerçek zamanın ötesinde gelişen küresel bir casusluk dünyasında tehlikeli bir görev üstlenir.",
                IMDBRating = 7.3m,
                Director = "Christopher Nolan",
                Duration = "2 saat 30 dakika",
                Genre = "Aksiyon, Bilim Kurgu, Gerilim",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/AZGcmvrTX9M",
                Actors = new List<string>
                {
                    "John David Washington – Protagonist",
                    "Robert Pattinson – Neil",
                    "Elizabeth Debicki – Kat",
                    "Kenneth Branagh – Andrei Sator",
                    "Michael Caine – Crosby",
                    "Dimple Kapadia – Priya",
                    "Aaron Taylor-Johnson – Ives"
                }
            },
            new Movie
            {
                Title = "Inception",
                Year = 2010,
                PosterUrl = "https://biletinial.com/tr-tr/sinema/inception",
                Description = "Dom Cobb, insanların rüyalarına girerek bilinçaltlarından sır çalabilen bir hırsızdır...",
                IMDBRating = 8.8m,
                Director = "Christopher Nolan",
                Duration = "2 saat 28 dakika",
                Genre = "Bilim Kurgu, Aksiyon, Gerilim",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/YoHD9XEInc0",
                Actors = new List<string>
                {
                    "Leonardo DiCaprio – Dom Cobb",
                    "Joseph Gordon-Levitt – Arthur",
                    "Elliot Page – Ariadne",
                    "Tom Hardy – Eames",
                    "Ken Watanabe – Saito",
                    "Marion Cotillard – Mal",
                    "Cillian Murphy – Robert Fischer",
                    "Michael Caine – Miles"
                }
            },
            new Movie
            {
                Title = "Monty Python and the Holy Grail",
                Year = 1975,
                PosterUrl = "https://tr.web.img3.acsta.net/medias/nmedia/18/80/37/57/19546984.jpg",
                Description = "Kral Arthur ve Yuvarlak Masa Şövalyeleri'nin Kutsal Kâse'yi bulmak için çıktıkları yolculuğu absürt ve mizahi bir dille anlatır.",
                IMDBRating = 8.2m,
                Director = "Terry Gilliam & Terry Jones",
                Duration = "1 saat 31 dakika",
                Genre = "Absürt Komedi, Parodi",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/LG1PlkURjxE",
                Actors = new List<string>
                {
                    "Graham Chapman – Kral Arthur",
                    "John Cleese – Sir Lancelot",
                    "Eric Idle – Sir Robin",
                    "Terry Gilliam – Patsy",
                    "Terry Jones – Sir Bedevere",
                    "Michael Palin – Sir Galahad"
                }
            },
            new Movie
            {
                Title = "The Grand Budapest Hotel",
                Year = 2014,
                PosterUrl = "https://m.media-amazon.com/images/S/pv-target-images/9da1d89f1cbdde85baa0012711230c2f2170cebd0cec6a4de90f415332918176.jpg",
                Description = "Ünlü bir otelin konsiyerji Gustave H. ve sadık çalışanı Zero'nun, çalınan bir tablo ve büyük bir miras davası etrafındaki macerası.",
                IMDBRating = 8.1m,
                Director = "Wes Anderson",
                Duration = "1 saat 39 dakika",
                Genre = "Komedi, Macera, Suç",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/1Fg5iWmQjwk",
                Actors = new List<string>
                {
                    "Ralph Fiennes – M. Gustave",
                    "Tony Revolori – Zero",
                    "Saoirse Ronan – Agatha",
                    "Adrien Brody – Dmitri",
                    "Willem Dafoe – Jopling",
                    "Jeff Goldblum – Kovacs",
                    "Edward Norton – Henckels",
                    "Tilda Swinton – Madame D."
                }
            },
            new Movie
            {
                Title = "Mad Max: Fury Road",
                Year = 2015,
                PosterUrl = "https://play-lh.googleusercontent.com/IAFsZ8rqbpzrwuavU_ZkgPmxFiB-0lxM3na4kYnvm4qeOcA5J5rZ5E7ue2uPjLvZhCoylQ",
                Description = "Post-apokaliptik bir dünyada, Max Rockatansky'nin özgürlük arayışındaki mücadele hikayesi.",
                IMDBRating = 8.1m,
                Director = "George Miller",
                Duration = "2 saat",
                Genre = "Aksiyon, Macera, Bilim Kurgu",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/hEJnMQG9ev8",
                Actors = new List<string>
                {
                    "Tom Hardy – Max Rockatansky",
                    "Charlize Theron – Furiosa",
                    "Nicholas Hoult – Nux",
                    "Hugh Keays-Byrne – Immortan Joe"
                }
            },
            new Movie
            {
                Title = "Die Hard",
                Year = 1988,
                PosterUrl = "https://tr.web.img2.acsta.net/pictures/bzp/01/4019.jpg",
                Description = "John McClane, Noel arifesinde karısını teröristlerden kurtarmaya çalışır.",
                IMDBRating = 8.2m,
                Director = "John McTiernan",
                Duration = "2 saat 12 dakika",
                Genre = "Aksiyon, Gerilim",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/2TQ-pOvI6Xo",
                Actors = new List<string>
                {
                    "Bruce Willis – John McClane",
                    "Alan Rickman – Hans Gruber",
                    "Bonnie Bedelia – Holly Gennaro",
                    "Reginald VelJohnson – Sgt. Powell"
                }
            },
            new Movie
            {
                Title = "The Shawshank Redemption",
                Year = 1994,
                PosterUrl = "https://m.media-amazon.com/images/I/61-vQDr7ecL._AC_UF894,1000_QL80_.jpg",
                Description = "Andy Dufresne'nin Shawshank Hapishanesi'ndeki özgürlük ve dostluk dolu öyküsü.",
                IMDBRating = 9.3m,
                Director = "Frank Darabont",
                Duration = "2 saat 22 dakika",
                Genre = "Dram",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/NmzuHjWmXOc",
                Actors = new List<string>
                {
                    "Tim Robbins – Andy Dufresne",
                    "Morgan Freeman – Red",
                    "Bob Gunton – Warden Norton",
                    "Clancy Brown – Captain Hadley"
                }
            },
            new Movie
            {
                Title = "The Godfather",
                Year = 1972,
                PosterUrl = "https://m.media-amazon.com/images/M/MV5BNGEwYjgwOGQtYjg5ZS00Njc1LTk2ZGEtM2QwZWQ2NjdhZTE5XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg",
                Description = "İtalyan mafya ailesi Corleone'lerin iktidar mücadelesi.",
                IMDBRating = 9.2m,
                Director = "Francis Ford Coppola",
                Duration = "2 saat 55 dakika",
                Genre = "Suç, Dram",
                Language = "İngilizce",
                VideoUrl = "https://www.youtube.com/embed/sY1S34973zA",
                Actors = new List<string>
                {
                    "Marlon Brando – Vito Corleone",
                    "Al Pacino – Michael Corleone",
                    "James Caan – Sonny Corleone",
                    "Robert Duvall – Tom Hagen"
                }
            }
        };

        context.Movies.AddRange(movies);
        context.SaveChanges();

    }
    var services = scope.ServiceProvider;
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();


    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole("User"));
    }
    var allUsers = userManager.Users.ToList();

    foreach (var user in allUsers)
    {
        if (await userManager.IsInRoleAsync(user, "Admin"))
        {
            await userManager.DeleteAsync(user);
        }
    }
    await SeedAdminUser(userManager, roleManager);

    async Task SeedAdminUser(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        string adminEmail = "cevikgokhan11@gmail.com";
        string adminUsername = "gokhan";
        string adminPassword = "Gökhan123.";
        string adminRoleName = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
        }

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
            else
            {
                throw new Exception("Admin kullanıcı oluşturulamadı: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }



}
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
        ctx.Context.Response.Headers.Append("Pragma", "no-cache");
        ctx.Context.Response.Headers.Append("Expires", "0");
    }
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "account",
    pattern: "Account/{action}",
    defaults: new { controller = "Account" });

await app.RunAsync();
