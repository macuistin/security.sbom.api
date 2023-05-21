using ExRam.Gremlinq.Core;
using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Core.Models;
using Microsoft.AspNetCore.Mvc;
using security.sbom.Entities;
using security.sbom.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddGremlinq(
        setup => setup
            .ConfigureEnvironment(env => env
                .UseModel(GraphModel.FromBaseTypes<Vertex, Edge>(lookup => lookup.IncludeAssembliesOfBaseTypes())
                    .ConfigureProperties(pm =>
                        pm.ConfigureMemberMetadata(m =>
                            m.UseCamelCaseNames()))))
            .UseCosmosDb()
    )
    .AddLogging(o => o.AddConsole())
    .AddScoped<ISbomService, SbomService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

builder.Services.AddAuthorization(options => options
    .AddPolicy("UploadSpbx", policy =>
        policy.RequireClaim("scope", "security.sbom.api.uploadsbom")));

var app = builder.Build();
app.UseHsts();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}

app.UseAuthentication()
    .UseAuthorization()
    .UseHttpsRedirection();

app.MapPost("/spbx",
        ([FromBody]SPDX22Document spbxDocument, ISbomService sbomService) =>
        {
            try
            {
                sbomService.SaveSpbxAsync( spbxDocument);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }

            return Results.Ok();
        })
    .WithName("PostSpbx")
    .AllowAnonymous(); // Leave as anonymous while figuring stuff out
    //.RequireAuthorization("UploadSpbx");

app.Run();
