using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Nop.Core.Configuration;
using Nop.Web.Framework.Infrastructure.Extensions;
using Nop.Data.Migrations;
using Nop.Data;

// added these libraries for fluent migration as per https://fluentmigrator.github.io/articles/quickstart.html?tabs=runner-in-process

using System;
using System.Linq;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Nop.Data.Mapping;
using Nop.Core.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
builder.Configuration.AddEnvironmentVariables();

//Add services to the application and configure service provider
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();
//Add services to Configure the fluent migration services



//builder.Services.AddFluentMigratorCore()
//                            .AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
//                            .AddTransient<IMappingEntityAccessor>(x => x.GetRequiredService<IDataProviderManager>().DataProvider)
//                            .ConfigureRunner(rb => rb.WithVersionTable(new MigrationVersionInfo()).AddSqlServer().AddMySql5().AddPostgres()
//                                        .ScanIn(executingAssembly).For.Migrations())
//                                        .AddLogging(lb => lb.AddFluentMigratorConsole())
//                                        .Configure<FluentMigratorLoggerOptions>(options =>
//                                        {
//                                            options.ShowSql = true;
//                                            options.ShowElapsedTime = true;
//                                        });
// .WithGlobalConnectionString(@"Server=N2T-J3PQ6H2;Database=nopCommerce_4.50.2_Source;Trusted_Connection=true")
//   .ScanIn(typeof(Migration_14072022).Assembly).For.Migrations())
//Enable logging to console in the FluentMigrator way

//Build the service provider
//.BuildServiceProvider(false);

var app = builder.Build();

//Configure the application HTTP request pipeline
app.ConfigureRequestPipeline();
app.StartEngine();
app.Run();
