using SIS.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Services ────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddSingleton<JsonDatabase>();   // singleton so file locks don't clash

// Allow the HTML frontend (opened from file:// or a local dev server) to call the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()   // tighten to your origin in production
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

// ── App pipeline ────────────────────────────────────────────────────────────
var app = builder.Build();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Default route → friendly message so you know the API is alive
app.MapGet("/", () => Results.Ok(new
{
    status  = "running",
    message = "SIS API is online. Use /api/students and /api/auth/login.",
    time    = DateTime.UtcNow
}));

app.Run();
