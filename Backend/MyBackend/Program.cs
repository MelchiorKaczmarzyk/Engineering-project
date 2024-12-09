using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using MyBackend;
using MyBackend.DbContext;
using MyBackend.Entities;
using MyBackend.Services;

var MyAllowSpecificOrigins = "MyCORS";
    
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          //for development only
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          //policy.WithOrigins("http://localhost:4200");
                      });
});

builder.Services.AddDbContext<MyDbContext>();
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<MyDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.AddScoped<IBackendRepository, BackendRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

 builder.Services.AddScoped<IMailService, MailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllers();

app.Run();
