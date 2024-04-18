using KutumbaBhoj.Application.Interfaces;
using KutumbaBhoj.Application.Services;
using KutumbaBhoj.Infrastructure.DbContext;
using KutumbaBhoj.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDishes, ServiceDishes>();
builder.Services.AddScoped<IFoods, ServiceFoods>();
//builder.Services.AddScoped<IUsers, ServiceUsers>();
builder.Services.AddScoped<IOrder, ServiceOrders>();
builder.Services.AddScoped<IRestaurants, ServiceRestaurants>();
builder.Services.AddScoped<IFeedback, ServiceFeedbacks>();

//builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
//builder.Services.AddScoped<Facebook>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var Configuration = builder.Configuration;

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://example.com")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure Google authentication
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

// Configure the authentication services
var services = builder.Services;

services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = Configuration["421845565319-shgepssgp2s21jsuagnhemkgeqbgor40.apps.googleusercontent.com"];
    googleOptions.ClientSecret = Configuration["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"];
})
.AddFacebook(options =>
{
    options.AppId = Configuration["3685898741738785"];
    options.AppSecret = Configuration["597c889249e9f11407992b0daab9bd3f"];
   // options.AccessDeniedPath = "/AccessDeniedPathInfo";
});


