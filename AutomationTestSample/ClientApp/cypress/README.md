# Welcome to Cypress 
    https://www.cypress.io/
    https://docs.cypress.io/guides/getting-started/writing-your-first-test
    https://docs.cypress.io/api/table-of-contents


# Plugins being used
    * badeball/cypress-cucumber-preprocessor: Plugin to support BDD with Cypress and preferred bundler 'esbuild' for typescript
	    references:
		https://github.com/badeball/cypress-cucumber-preprocessor
		https://github.com/badeball/cypress-cucumber-preprocessor/blob/master/docs/readme.md
        https://github.com/badeball/cypress-cucumber-preprocessor/tree/master/examples/esbuild
        https://github.com/badeball/cypress-cucumber-preprocessor/tree/master/examples/esbuild/cypress/integration

    * cypress-esbuild-preprocessor: preferred bundler to support typescript with cypress-cucumber-preprocessor
        mentioned in: https://docs.cypress.io/plugins/directory
        github page: https://github.com/sod/cypress-esbuild-preprocessor

	* cypress-testing-library: 
    	references:
        https://testing-library.com/docs/cypress-testing-library/intro/
		https://github.com/testing-library/cypress-testing-library#differences-from-dom-testing-library
        
    * cypress-mochawesome-reporter: Zero config Mochawesome reporter for Cypress with screenshots attached to tests
        reference:
        https://github.com/LironEr/cypress-mochawesome-reporter


# Setup 
    1. Download cypress.zip and set system variable for the downloaded path:
    * Use this url "https://download.cypress.io/desktop/8.3.1/win32-x64/cypress.zip" to download windows cypress.zip a given folder. 
        Ex: under "C:/cypress/cypress.zip"
    * set windows system environment variable > new variable under 'system variable':
        Name: CYPRESS_INSTALL_BINARY
        Value: C:\Cypress.zip


    2. Once the repo is cloned then execute following commands:
    * cd AutomationTestSample/ClientApp
    * npm install  
        <!-- Note: this install all packages from package.json file including the cypress to node-modules. The version of cypress being installed as dev dependency would be the one from cypress.zip downloaded from earlier step -->
    * cd ..   //to go to folder ".\AutomationTestSample\" which contains the .csproj file for dotnet project
    * dotnet run  //to start the application
    * Open https://localhost:7150/ in your browser to access the application. It will automatically redirect to 'https://localhost:44449/'


# Execution ways
    * Open cypress GUI and run specific tests:
        Following are different ways to Open Cypress in GUI and run individual tests or files of your choice:
        * cd ./AutomationTestSample/ClientApp
        * npx cypress open   <!-- if no config-file specified then by default opens cypress GUI with configs from cypress.json file -->

    * Run via CLI for electron/your_choice_of_browser
        Following are different ways to execute in headless mode based on different requirements:
        npx cypress run   <!--- runs all test --->
        npx cypress run --browser electron,chrome,firefox   <!-- execute for specific browser -->

# PDF Reports with screenshots for easy failure debugging
    * cypress-mochawesome-reporter plugin support will automatically generate the PDF report only when tests are executed via CLI
    * report can be accessed from directory ./ClientApp/cypress/mochawesome-report/*


