using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyBoardsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsDbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var pendingMigration = dbContext.Database.GetPendingMigrations();
if (pendingMigration.Any())
{
    dbContext.Database.Migrate();
}

var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        Email = "user1@test.com",
        FullName = "User One",
        Adress = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };

    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User Two",
        Adress = new Address()
        {
            City = "Kraków",
            Street = "D³uga"
        }
    };

    dbContext.Users.AddRange(user1,user2);
    dbContext.SaveChanges();
}

app.MapGet("data",async (MyBoardsContext db) =>
{
    //var tags = db.Tags.ToList();
    //var user = db.Users.First(u => u.FullName == "User One");
    //var epic = db.Epics.First();
    //return new { tags, epic, user };

    //var toDoWorkItems = db.WorkItems.Where(wi => wi.StateId == 1).ToList();
    // return toDoWorkItems;

    //var comments = await db.Comments
    //.Where(c => c.CreatedDate > new DateTime(2022, 7, 23))
    //.ToListAsync();

    // return comments;

    /* var top5comments = await db.Comments
     .OrderByDescending(c => c.CreatedDate)
     .Take(5)
     .ToListAsync();

     return top5comments;*/

    var statesCount = await db.WorkItems
    .GroupBy(wi => wi.StateId)
    .Select(s => new { stateId = s.Key, count = s.Count() })
    .ToListAsync();

    return statesCount;

});

app.Run();

