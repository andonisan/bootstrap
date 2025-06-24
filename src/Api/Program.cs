using Api;
using BuildingBlocks.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notifications;
using Todos;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServices();

builder.AddIntegrationCommunucation();
builder.AddCustomSeqEndpoint();
builder.Services.AddSignalR();

TodosModule.Install(builder);
NotificationsModule.Install(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails()
    .UseHttpsRedirection()
    .UseRouting();

NotificationsModule.Map(app);

app.MapEndpoints();

app.Run();
