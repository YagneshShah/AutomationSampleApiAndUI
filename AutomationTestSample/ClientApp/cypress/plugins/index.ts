import * as createBundler from "@bahmutov/cypress-esbuild-preprocessor";
import { createEsbuildPlugin } from "@badeball/cypress-cucumber-preprocessor/esbuild";
import { beforeRunHook, afterRunHook } from 'cypress-mochawesome-reporter/lib';

// ***********************************************************
// This example plugins/index.ts can be used to load plugins
//
// You can change the location of this file or turn off loading
// the plugins file with the 'pluginsFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/plugins-guide
// ***********************************************************

// This function is called when a project is opened or re-opened (e.g. due to
// the project's config changing)
export default (
  on: Cypress.PluginEvents,
  config: Cypress.PluginConfigOptions
): Cypress.PluginConfigOptions => {
  // Hack to allow overriding of config.baseUrl with env.baseUrl.
  //
  // This is important for us because our devs and CI/CD pipeline need to
  // run Cypress tests against localhost:3000. However when Tamara does QA
  // testing against staging, she wants to run her test cases against
  // staging.mastt.com.au instead.
  // This isn't officially supported, but seems to be the community solution:
  // - https://github.com/cypress-io/cypress/issues/909
  config.baseUrl = config.env.baseUrl || config.baseUrl;

  on(
    "file:preprocessor",
    createBundler({
      plugins: [createEsbuildPlugin(config)],
    })
  );

  on('before:run', async (details) => {
    console.log('override before:run with cypress-mochawesome-reporter');
    await beforeRunHook(details);
  });

  on('after:run', async () => {
    console.log('override after:run with cypress-mochawesome-reporter');
    await afterRunHook();
  });

  return config;
};
