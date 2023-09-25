var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var players = new List<Player>
{
    new Player { Id = 0, Name = "BoostedBlyat", Score = 12993.74F  },
    new Player { Id = 1, Name = "Rusik Robokop", Score = 733.2F  },
    new Player { Id = 2, Name = "Rando", Score = 8983.26F  },
};

app.MapGet("/players", () =>
{
    return Results.Ok(players);
});

app.MapGet("/players/{id}", (int id) =>
{
    var player = players.Find(p => p.Id == id);

    if(player is null)
        return Results.NotFound("Sorry no player found with that id");

    return Results.Ok(player);
});

app.MapPost("/players", (Player p) =>
{
    players.Add(p);
    return Results.Ok(players);
});

app.MapPut("/players/{id}", (Player editedPlayer, int id) =>
{
    var player = players.Find(p => p.Id == id);
    if (player is null)
        return Results.NotFound("No players to edit with that id");

    player.Score = editedPlayer.Score;
    player.Name = editedPlayer.Name;

    return Results.Ok(players);
});

app.MapDelete("/players/{id}", (int id) =>
{
    var player = players.Find(p =>p.Id == id);
    if (player is null)
        return Results.NotFound("No player to delete with that id");

    players.Remove(player);

    return Results.Ok(players);
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
