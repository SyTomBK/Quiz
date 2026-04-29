using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProtoBuf.Grpc.Server;
using QuizSvc.Application.Extensions;
using QuizSvc.Infrastructure;
using QuizSvc.Infrastructure.Extensions;
using QuizSvcSvc.Configurations;
using QuizSvcSvc.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#region Declare info to this Processor
var connectionString = config.GetConnectionString("DatabaseConnectionString");
// Register DB, You can register multiple databases here
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
dataSourceBuilder.EnableDynamicJson();

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<DataContext>(options =>options.UseNpgsql(dataSource));

builder.Services.AddInfrastructure(config);

// Add application
builder.Services.AddApplication(config);

// Logging
builder.Services.AddSvcLogging();

// gRPC
builder.Services.AddSvcGrpc();

builder.Services.AddHealthChecks().AddNpgSql(connectionString: connectionString?? "", name: "postgresql");


#endregion 

//var appConfiguration = GetAppConfiguration();
var app = builder.Build();

app.UseRouting();

#region Init & start this Processor. End of it, ready to handle work

app.MapGrpcEndpoints();

app.MapHealthChecksWithResponse();

app.MapGet("/", () => "QuizSvc gRPC Service is running!");

IWebHostEnvironment env = app.Environment;
var isMig = !(!args.Any(x => x == "mig") && !env.IsDevelopment());
app.UseApplicationDatabase<DataContext>(isMig);

if (env.IsDevelopment())
{
    app.MapCodeFirstGrpcReflectionService();
}

app.Run();
#endregion
