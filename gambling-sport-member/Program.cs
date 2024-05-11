using gambling_sport_member.Services;
using gamblingsportmember.Interfaces;
using gamblingsportmember.Models;
using gamblingsportmember.Repositories;
using gamblingsportmember.Utilities;
using gamblingsportmember.Helpers;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPasswordUtility, PasswordUtility>();
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddDbContext<MemberDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("MemberConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MinRequestBodyDataRate = null;

    options.ListenAnyIP(50050, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1;
        });

    options.ListenAnyIP(50051, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<MemberService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

