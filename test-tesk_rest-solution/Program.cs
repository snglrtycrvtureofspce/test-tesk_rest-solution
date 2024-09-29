using System;
using System.Reflection;
using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using test_tesk_rest_solution.Data;
using test_tesk_rest_solution.Filters;
using test_tesk_rest_solution.Jobs;
using test_tesk_rest_solution.Services.Implementations;
using test_tesk_rest_solution.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DeployConnection");
builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    if (connectionString != null) options.UseNpgsql(connectionString);
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var conf = new MapperConfiguration(p =>
{
    p.AddMaps(Assembly.GetExecutingAssembly());
});
var mapper = conf.CreateMapper();
builder.Services.AddScoped<IMapperBase>(_ => mapper);
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderProcessingJob>();
builder.Services.AddScoped<OrderPriorityJob>();

builder.Services.AddApiVersioning(
        options =>
        {
            // reporting api versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;

            options.DefaultApiVersion = new ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

            options.ApiVersionReader = ApiVersionReader.Combine(
                new HeaderApiVersionReader("apiVersion")
            );

            options.Policies.Sunset(0.9)
                .Effective(DateTimeOffset.Now.AddDays(60))
                .Link("policy.html")
                .Title("Versioning Policy")
                .Type("text/html");
        })
    .AddMvc()
    .AddApiExplorer(
        options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(Environment.GetEnvironmentVariable("HangfireConnection"))));

builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowAll");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new MyAuthorizationFilter() }
});

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<OrderPriorityJob>(
        "recalculate-order-priorities",
        job => job.RecalculateOrderPrioritiesAsync(),
        Cron.MinuteInterval(5)
    );
    recurringJobManager.AddOrUpdate<OrderProcessingJob>(
        "process-pending-orders",
        job => job.ProcessPendingOrdersAsync(),
        Cron.MinuteInterval(5)
    );
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();