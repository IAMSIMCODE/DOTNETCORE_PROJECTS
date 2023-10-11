var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

//Configure Kestrel not to display in the response header 
builder.WebHost.ConfigureKestrel(opt => opt.AddServerHeader = false);

builder.Services.AddControllers();
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

app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Codepedia-Custom-Header-Response", "Satinder singh");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Content-Security-Policy", "base-url 'self'; frame-src www.google.com; default-src 'self'; script-src 'self'; www.google.com; connect-src 'self' google-analytics.com; img-src data: 'self' www.gstatic.com; style-src 'self' fonts.googleapi.com;");

   context.Response.Headers.Remove("X-Powered-By");
   context.Response.Headers.Remove("Server");
    await next();
});

app.MapControllers();

app.Run();
