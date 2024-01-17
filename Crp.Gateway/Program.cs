using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection.PortableExecutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//------------- instrumentataion ---------------------//
// Build a resource configuration action to set service information.
Action<ResourceBuilder> resource = r => r.AddService(
    serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "otel-test")!,
    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
    serviceInstanceId: Environment.MachineName);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource)
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        //.AddSource("crp.lst.otel-test-dz")
        .AddConsoleExporter()
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Tracing:Endpoint", defaultValue: "http://localhost:4317")!);
            opt.Protocol = OtlpExportProtocol.HttpProtobuf;
            opt.HttpClientFactory = () =>
            {
                var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                HttpClient client = new(clientHandler);
                client.DefaultRequestHeaders.Add("lightstep-access-token", (builder.Configuration.GetValue("Otlp:LightstepAccessToken", defaultValue: "NO_TOKEN_FOUND")!));
                return client;
            };
        })
);    


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
