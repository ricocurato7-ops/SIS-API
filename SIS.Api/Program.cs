var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ===== CORS FIX =====
// Allow requests from your Netlify frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "https://your-netlify-app.netlify.app",  // ⚠️ REPLACE with your actual Netlify URL
                "https://*.netlify.app",                  // Allow all Netlify apps (for testing)
                "http://localhost:5500",                  // VS Code Live Server
                "http://localhost:5000",                  // Local frontend
                "http://127.0.0.1:5500"                   // Alternative local
            )
            .AllowAnyMethod()      // GET, POST, PUT, DELETE
            .AllowAnyHeader()      // Content-Type, Authorization, etc.
            .AllowCredentials();    // Allow cookies/tokens
    });
});

var app = builder.Build();

// ===== CORS ORDER MATTERS! =====
// Use CORS before other middleware
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
