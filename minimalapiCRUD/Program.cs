using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using minimalapiCRUD;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<Appdbcontext>();



//database
builder.Services.AddDbContext<Appdbcontext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("dbconn"));
});

//cors
builder.Services.AddCors(option =>
{
    option.AddPolicy("frontendApps", policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


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

app.UseCors("frontendApps");

app.UseHttpsRedirection();

app.MapIdentityApi<IdentityUser>();

app.MapPost("/create", async (Appdbcontext db, Post post) =>
{
    await db.Posts.AddAsync(post);
    await db.SaveChangesAsync();

    return Results.Created($"/posts/{post.id}", post);
});

app.MapGet("/user", [Authorize] async (ClaimsPrincipal user) =>
{
    Dictionary<string, string> authuser = new Dictionary<string, string>();
    authuser.Add("userEmail", user?.Identity?.Name.ToString());
    return Results.Ok(authuser);
});

app.MapGet("/posts", async (Appdbcontext db) =>
{
    return await db.Posts.ToListAsync();
});

app.MapGet("/posts/{id}", async (Appdbcontext db, int id) =>
{
    return await db.Posts.FirstOrDefaultAsync(x => x.id == id);
})
.WithOpenApi();

app.Run();

