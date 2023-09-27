using PlayerStatsServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add db context
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var players = new List<Player>
//{
//    new Player { Id = 0, Name = "BoostedBlyat", Score = 12993.74F  },
//    new Player { Id = 1, Name = "Rusik Robokop", Score = 733.2F  },
//    new Player { Id = 2, Name = "Rando", Score = 8983.26F  },
//};

app.MapGet("/players", async (DataContext context) => await context.Players.ToListAsync());

app.MapGet("/players/{id}", async(DataContext context, int id) =>
    await context.Players.FindAsync(id) is Player player ? 
        Results.Ok(player) :
        Results.NotFound("Sorry, player not found")
);

app.MapPost("/players", async (DataContext context, Player player) =>
{
    context.Players.Add(player);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Players.ToListAsync());
});

app.MapPut("/players", async (DataContext context, Player updatedPlayer, int id) =>
{
    var player = await context.Players.FindAsync(id);
    if (player is null)
        return Results.NotFound("Sorry, player doesn't exist to edit");

    player.Name = updatedPlayer.Name;
    player.Score = updatedPlayer.Score;
    await context.SaveChangesAsync();

    return Results.Ok(await context.Players.ToListAsync());
});

app.MapDelete("/players", async (DataContext context, int id) =>
{
    var player = context.Players.Find(id);
    if (player is null)
        return Results.NotFound("Sorry no player found to remove");

    context.Players.Remove(player);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Players.ToListAsync());
});


app.Run();


public class Player
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float Score { get; set; }

    public Player()
    {
        Id = -1;
        Name = string.Empty;
        Score = 0;
    }
}
