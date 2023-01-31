# Automation Test Sample Application
This is a sample application for running automation tests.

## Requirements For This Project

Run the application and launch the homepage, which documents the context and the requirements. 

## Prerequisites

You will need the following tools installed to build and run this project:

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/en/download/)

## Running Locally

### Using Visual Studio 2022

To run the application locally using Visual Studio 2022 open the solution file `AutomationTestSample.sln`
and press F5 (with debugging) or Ctrl + F5 (without debugging). This should automatically launch the application
in your default browser.

### Using VS Code

 To start the application locally run the following commands:

    cd AutomationTestSample/ClientApp
    npm install
    cd ..
    dotnet run

Open https://localhost:7150/ in your browser to access the application.

## Application Architecture

This application was built using the Visual Studio template `ASP.NET Core with Angular`.
It has a standard ASP.NET Core application and a standar Angular 14 application.
The Angular application is placed insdie the `ClientApp` directory.

It uses a SPA proxy to route API traffic to the ASP.NET Core API. The proxy configuration
is at following locations:
- The `ClientApp/proxy.conf.js` file handles Angular side of things
- The project file `AutomationTestSample.csproj` defines the `SpaProxyServerUrl` and the `SpaProxyLaunchCommand`


