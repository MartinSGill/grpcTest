# Example ASP .NET Core gRPC Implementation

I was disappointed by the simple example in the MSDN for gRPC with .NET Core, so I thought I'd try out a slightly more involved example.

My main objectives were to understand how the code is generated, and to see how easily I could expand/modify the contracts and what subsequent changes are required to the code.

Turns out that other than an slight issue with Rider's intellisense not picking up the generated files it was surprisingly easy.

## Scenario

I imagined a simple scenario of a Dashboard style website that gathers information from a number of remote/distributed services to display information to the user.

## Try it out

Build the projects

```powershell
./build.ps1
```

Run `grpcTodo` project and leave it running.

Run `grpcWeather` project and leave it running.

Run the `grcpDashboard` project and browse to its home page. _Page will open automatically if run from Visual Studio._

You should see some information like this:

> Welcome
> Learn about building Web apps with ASP.NET Core.
>
> Temperature 100 C
>
> |Priority	|Date	|Message |
> |----|-----|----|
> |1	|26/06/2020	|This is a todo|
> |2	|26/06/2020	|This is another todo|

The temperature  value comes from `grpcWeather`, and the list of todos comes from `grpcTodo`.

## The Server(s)

First we add a package reference to the .NET Core gRPC implementation:

```xml
<PackageReference Include="Grpc.AspNetCore" Version="2.27.0" />
```

This includes a number of nested packages. The nested packages are what's usually required for client, and you'll see them later in the client section.

Next we next we need to include and define the contract using the protobuf IDL.

```xml
  <ItemGroup>
    <Protobuf Include="Protos\weather.proto">
      <GrpcServices>Server</GrpcServices>
    </Protobuf>
  </ItemGroup>
  ```

  protobuf definition:

  ```protobuf
syntax = "proto3";

option csharp_namespace = "GRpcTest.GRpcWeather";

package grpcTest.grpcWeather;

service Weather {
    rpc Temperature(LocationMessage) returns (TemperatureReply);
}

message LocationMessage {
    string name = 1;
}

message TemperatureReply {
    int32 Celsius = 1;
    int32 Fahrenheit = 2;
    int32 Kelvin = 3;
}
  ```

You can now build the project and the server implementation classes will be generated; by default in the `obj` folder.

You can now start implementing the server.

__Note:__ If you are using Rider you'll need to add the following to the csproj for intellisense to work correctly:

```xml
<!-- Work-around for Rider intellisense not working -->
<Target Name="Protobuf_Compile_Before_AssemblyReferences" BeforeTargets="ResolveAssemblyReferences">
    <CallTarget Targets="_Protobuf_Compile_BeforeCsCompile" />
</Target>
```

Implementing the server is very easy. Simply inherit from the generated classes. Namespaces and names are as you would expect from the protobuf definition. _If you have trouble finding out the class/namespace names then you can just read the code in the generated files in `obj` folder._

```csharp
public class WeatherService : Weather.WeatherBase
{
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(ILogger<WeatherService> logger)
    {
        _logger = logger;
    }

    public override Task<TemperatureReply> Temperature(LocationMessage request, ServerCallContext context)
    {
        return Task.FromResult(
            new TemperatureReply { Celsius = 100, Fahrenheit = 212, Kelvin = 373 });
    }
}
```

Note that the implemention is  simply an override and therefore provides everything you need to know what your inputs and outputs are.

## The Client

The client is in many ways much simpler.

First add the required packages for the client:

```xml
<ItemGroup>
    <PackageReference Include="Google.Protobuf" Version="3.12.3" />
    <PackageReference Include="Grpc" Version="2.30.0" />
    <PackageReference Include="Grpc.Net.Client" Version="2.27.0" />
    <PackageReference Include="Grpc.Tools" Version="2.30.0" />
</ItemGroup>
```

Then add references to the service contracts. Note that `GrpcServices` is 'Client' this time.

```xml
<ItemGroup>
    <Protobuf Include="..\grpcWeather\Protos\weather.proto">
        <GrpcServices>Client</GrpcServices>
    </Protobuf>
    <Protobuf Include="..\grpcTodo\Protos\todo.proto">
        <GrpcServices>Client</GrpcServices>
    </Protobuf>
</ItemGroup>
```
Building the code now will generate the required clients.

All that's needed then is create the clients and invoke the methods:

```csharp
var weatherChannel = GrpcChannel.ForAddress("https://localhost:5002");
var weather = new Weather.WeatherClient(weatherChannel);
var temp = weather.Temperature(new LocationMessage { Name = "London" });
TemperatureCelcius = temp.Celsius;

var todoChannel = GrpcChannel.ForAddress("https://localhost:5003");
var todoClient = new Todo.TodoClient(todoChannel);
var todos = todoClient.GetTodos(new GetTodosMessage());
Todos = todos.Todos;
```

## Things of Interest

- grpcTodo has an example of a method without parameters; note how it needs an empty message object as its input.
- grpcTodo has an example of an enumerable, or list, construct.
- grpcDashboard is almost entirely boilerplate, most things of interest are the `grpcDashboard.csproj` and the `index.cshtml.cs` file.

## Sources

- [Introduction to gRPC on .NET Core](https://docs.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-3.1)
- [Issues with Rider Intellisense](https://github.com/grpc/grpc/issues/17898#issuecomment-459832750)
- [Official gRPC site](https://grpc.io/)
