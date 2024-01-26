using Ase.Doc.Demo.Configuration;
using Ase.Doc.Demo.Services;

var MyAllowSpecificOrigins = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);
//
// Add services to the container.
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});
//
//  Configurations
//
builder.Services.AddSingleton<AppConfig>();
//
//  Services
//
builder.Services.AddSingleton<DocumentAiService>();
builder.Services.AddSingleton<Storage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

//app.UseAuthentication();
//app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.Run();

