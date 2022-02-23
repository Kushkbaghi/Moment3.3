
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Connect to Services
builder.Services.AddDbContext<CDDb>(options =>
options.UseSqlite("Data Source=CDDb.db"));

var app = builder.Build();


// Get all data 
app.MapGet("/cd", async (CDDb db) =>
    await db.CDs.ToListAsync());

// Get a post 
app.MapGet("/cd/{id}", async (int id, CDDb db) =>

    await db.CDs.FindAsync(id)
    is CD cd
    ? Results.Ok(cd)
    : Results.NotFound()

);

// post to databse 
app.MapPost("/cd", async (CD cd, CDDb db) =>
{
    db.CDs.Add(cd);
    await db.SaveChangesAsync();
    return Results.Created("Albume skapas!", cd);
});

// Delete a post 
app.MapDelete("/cd/{id}", async (int id, CDDb db) =>
{
    try
    {
        CD cd = await db.CDs.FindAsync(id);
        db.CDs.Remove(cd);
        await db.SaveChangesAsync();
        return Results.Ok("Item raderas!");
    }
    catch (global::System.Exception)
    {
        return Results.NotFound("Fel!");
        throw;
    }
    
});

// Update post 
app.MapPut("/cd/{id}", async (int id, CDDb db, CD cd) =>
{
    try
    {
        CD? cdTemp = await db.CDs.FindAsync(id);
        cdTemp.Artist = cd.Artist;
        cdTemp.Title = cd.Title;
        cdTemp.Length = cd.Length;
        cdTemp.Categury = cd.Categury;
        db.CDs.Update(cdTemp);
        await db.SaveChangesAsync();
        return Results.Ok("Item updateras!");
    }
    catch (global::System.Exception)
    {
        return Results.NotFound("Fel!");
        throw;
    }
    
});

app.MapGet("/", () => "Hej!");

app.Run();

// CD Class
class CD
{
    public int Id { get; set; }

    public string? Artist { get; set; }

    public string? Title { get; set; }

    public int? Length { get; set; }

    public string? Categury { get; set; }
}

// DbContext
class CDDb : DbContext
{
    public CDDb(DbContextOptions<CDDb> options) : base(options)
    {

    }
    public DbSet<CD> CDs => Set<CD>();
}