using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

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
        Address = new Address()
        {
            City = "Warszawa",
            Street = "Szeroka"
        }
    };

    var user2 = new User()
    {
        Email = "user2@test.com",
        FullName = "User Two",
        Address = new Address()
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

    /*var statesCount = await db.WorkItems
    .GroupBy(wi => wi.StateId)
    .Select(s => new { stateId = s.Key, count = s.Count() })
    .ToListAsync();

    return statesCount;*/

    /*var epicList = await db.Epics
    .Where(e => e.StateId == 4)
    .OrderBy(p => p.Priority)
    .ToListAsync();

    return epicList;*/

    /*var mostCommentUser = await db.Comments
        .GroupBy(c => c.AuthorId)
        .Select(g => new { g.Key, count = g.Count() })
        .ToListAsync();

    var topAuthor = mostCommentUser.First(a => a.count == mostCommentUser.Max(acc => acc.count));
    var userdetail = db.Users.First(u => u.Id == topAuthor.Key);*/

    // return new { userdetail, commentCount = topAuthor.count };

    /* var user = await db.Users
     .Include(u=>u.Comments).ThenInclude(wi=>wi.WorkItem)
     .Include(a=>a.Address)
     .FirstAsync(u => u.Id == Guid.Parse("C2377DEE-3B38-4089-CBE2-08DA10AB0E61"));*/
    //var commentsquantity = await db.Comments
    //.Where(c => c.AuthorId == user.Id)
    //.ToListAsync();

    var mincount = "85";
    var states = await db.WorkItemStates
    .FromSqlInterpolated($@"
    SELECT wis.Id, wis.Value
    FROM WorkItemStates wis
    JOIN WorkItems wi on wi.StateId = wis.Id
    GROUP BY wis.Id,wis.Value
    HAVING COUNT(*) > {mincount}")
    .ToListAsync();

    await db.Database.ExecuteSqlRawAsync(@"
    UPDATE Comments
    SET UpdatedDate = GETDATE()
    WHERE AuthorId = '4EBB526D-2196-41E1-CBDA-08DA10AB0E61'
    ");

    var entries = db.ChangeTracker.Entries();

    return states;

});

app.MapPost("update", async (MyBoardsContext db) =>
{
    var epic = await db.Epics.FirstAsync(e => e.Id == 1);

    var rejectedState = await db.WorkItemStates.FirstAsync(a => a.Value == "Rejected");

    /*epic.Area = "Updated area";
    epic.Priority = 1;
    epic.StartDate = DateTime.Now;*/
    epic.State = rejectedState;
    await db.SaveChangesAsync();

     return epic;

    

});

app.MapPost("create", async (MyBoardsContext db) =>
{
    /*Tag tagmvc = new Tag()
    {
        Value = "MVC"
    };
    Tag tagasp = new Tag()
    {
        Value = "ASP"
    };

    await db.Tags.AddRangeAsync(tagmvc,tagasp);
    await db.SaveChangesAsync();*/

    var address = new Address()
    {
        City = "Kraków",
        Country = "Poland",
        Street = "D³uga"
    };

    var user = new User()
    {
        Email = "adduser@test.com",
        FullName = "Test User",
        Address = address
    };

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
});

app.MapDelete("delete", async (MyBoardsContext db) =>
{
    /* var workitemTags = await db.WorkItemHasTags.Where(wi => wi.WorkItemId == 12).ToListAsync();

     db.WorkItemHasTags.RemoveRange(workitemTags);

     var workItem = await db.WorkItems.FirstAsync(wi => wi.Id == 16);

     db.RemoveRange(workItem);*/

    var user = await db.Users
    .Include(c=>c.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("E9AD3A55-351A-4D5B-CBFD-08DA10AB0E61"));

   // var commentsToDelete = await db.Comments.Where(c => c.AuthorId == user.Id).ToListAsync();

    //db.RemoveRange(commentsToDelete);

    //await db.SaveChangesAsync();

    db.Users.Remove(user);

    await db.SaveChangesAsync();

});

app.Run();

