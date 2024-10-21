var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.Jina_Passion_Api>("api-service");
builder.AddProject<Projects.Jina_Passion_Client>("frontend")
    .WithReference(api);

builder.Build().Run();
