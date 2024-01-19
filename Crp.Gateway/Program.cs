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
var serviceName = builder.Configuration.GetValue("ServiceName", defaultValue: "otel-test")!;
var serviceVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown";

// Build a resource configuration action to set service information.
Action<ResourceBuilder> resource = r => r.AddService(
    serviceName: serviceName,
    serviceVersion: serviceVersion,
    serviceInstanceId: Environment.MachineName);

//add OpenTelemetry with tracing and config OpenTelemetry to export trace info to lightstep 
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddSource(serviceName)
        .ConfigureResource(resource)
        .AddAspNetCoreInstrumentation()
        //.AddSource("crp.lst.otel-test-dz")
        .AddConsoleExporter()
        .AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri(builder.Configuration.GetValue("Otlp/ls:Tracing:Endpoint", defaultValue: "http://localhost:4317")!);
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

var tracer = TracerProvider.Default.GetTracer("CRP.Gateway.TestWebServer");
using (var parentSpan = tracer.StartActiveSpan("parent span"))
{
    parentSpan.SetAttribute("mystring", "value");
    parentSpan.SetAttribute("myint", 100);
    parentSpan.SetAttribute("mydouble", 101.089);
    parentSpan.SetAttribute("mybool", true);
    parentSpan.UpdateName("parent span new name");

    var childSpan = tracer.StartSpan("child span");
    childSpan.AddEvent("sample event").SetAttribute("ch", "value").SetAttribute("more", "attributes");
    childSpan.SetStatus(Status.Ok);
    childSpan.End();
}

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
