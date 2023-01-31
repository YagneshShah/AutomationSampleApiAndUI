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


## Details on API and UI Automation for the given application
#### Functional API Tests: Framework is created using C#, Xunit, HttpClient, AutoFixture, Fluent-Assertions
- Go to: https://github.com/YagneshShah/AutomationSampleApiAndUI/tree/main/APIFunctionalTests/FunctionalApiTests

#### PACT API Contract Testing: Framework using PactNet for contract testing, Xunit
- Go to: https://github.com/YagneshShah/AutomationSampleApiAndUI/tree/main/APIFunctionalTests/PactTests

- To execute Functional API tests and PACT tests, simply go to Test Explorer in Visual Studio.
- Select all tests and Run tests
  ![image](https://user-images.githubusercontent.com/4996330/215808261-10a786e0-d5ce-4ca2-9331-72344af32d5f.png)
- Note: Some tests might fails and are intentional as mentioned in test or test data description 
  ![image](https://user-images.githubusercontent.com/4996330/215808623-8ccefdc6-005c-4d99-a7e6-7731982cd20e.png)

  
#### UI Testing: using Cypress in javascript to do this and is in progress. Maybe 1 more day to finish this.
- For detailed guide on setting up Cypress and execution refer:
  https://github.com/YagneshShah/AutomationSampleApiAndUI/blob/main/AutomationTestSample/ClientApp/README.md
- Sample Cypress execution HTML report generated with 1 failed test to show how to debug along with screenshot attached:
  ![image](https://user-images.githubusercontent.com/4996330/215817120-5a0a80b0-0ea4-466b-a7e1-6abc87ce922d.png)
  ![image](https://user-images.githubusercontent.com/4996330/215817408-fa5cbe0d-7c58-4067-84d9-588d1bcef721.png)     

