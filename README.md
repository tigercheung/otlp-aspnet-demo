# Summary
Instrument ASP.NET Core Web API to generate telemetry data
Export telemetry data directly to Cloud Observability backend via OTLP through protobuf protocol

Source
https://www.mytechramblings.com/posts/getting-started-with-opentelemetry-and-dotnet-core/

# Distributed Tracing
### Cloud Observability Trace Ingest endpoint

#### Endpoint accepting application/json
```
https://ingest.lightstep.com[:443]/v1/traces
or 
ttps://ingest.lightstep.com:443/traces/otlp/v0.9
```

#### Endpoint accepting application/x-protobuf
```
https://ingest.lightstep.com[:443]/v1/traces
```


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

Creating Web API project
```
dotnet new webapi --name CRP.LST.AspNetCore.Calculation --use-controllers
```

# References

### demo (4 apps)
https://github.com/karlospn/opentelemetry-tracing-demo/tree/master

#### Distributed tracing
https://docs.lightstep.com/docs/quick-start-instrumentation-dotnet

#### To view traces
https://docs.lightstep.com/docs/view-traces

#### Logging
https://www.twilio.com/blog/build-a-logs-pipeline-in-dotnet-with-opentelemetry

#### Cloud Observability Endpoints
https://docs.lightstep.com/docs/send-otlp-over-http-to-lightstep
