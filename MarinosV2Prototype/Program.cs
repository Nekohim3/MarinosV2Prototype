using MarinosV2Prototype;
using MarinosV2PrototypeShared.Utils;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

DatabaseConnection.DatabaseSettings = new DatabaseSettings();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(_ =>
                                                    {
                                                        _.SerializerSettings.ReferenceLoopHandling      = ReferenceLoopHandling.Ignore;
                                                        _.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
                                                        _.SerializerSettings.NullValueHandling          = NullValueHandling.Ignore;
                                                        _.SerializerSettings.MaxDepth                   = 1024;
                                                        _.SerializerSettings.TypeNameHandling           = TypeNameHandling.None;
                                                        _.SerializerSettings.ContractResolver           = new CustomContractResolver();
                                                    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MarinosContext>();
//                                          {
//                                              _.UseNpgsql(builder.Configuration.GetConnectionString("TagDbConnection"));
//                                          });


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
