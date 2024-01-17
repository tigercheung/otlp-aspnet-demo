#Summary
Instrument ASP.NET Core Web API to generate telemetry data
Export telemetry data directly to Cloud Observability backend via OTLP through protobuf protocol

Source
https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/

install OpenTelemetry nuget packages for distributed tracing
```
dotnet add package OpenTelemetry
dotnet add package OpenTelemetry.Exporter.Console
dotnet add package OpenTelemetry.Exporter.OpenTelemetryProtocol
dotnet add package OpenTelemetry.Extensions.Hosting
dotnet add package OpenTelemetry.Instrumentation.Http
dotnet add package OpenTelemetry.Instrumentation.AspNetCore
```

Import OpenTelemetry packages
```
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
```