using BusinessAccess.Repository;
using BusinessAccess.Repository.IRepository;
using CustomerTask;
using CustomerTask.Services;
using CustomerTask.Services.IServices;
using DataAccess.DataViewModel;
using DataAccess.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<CustomerDbContext>();


builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ILogger, Logger<Customer>>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IPaginationRepository<Customer, CustomerSearchFilterDTO>, PaginationRepository<Customer, CustomerSearchFilterDTO>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
