// ***********************************************************
// This example support/index.ts is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************
import './commands'
import 'cypress-mochawesome-reporter/register';

// load type definitions that come with Cypress module
/// <reference types="cypress" />
declare global {
  namespace Cypress {
    interface Chainable {
      //Note: For each new custom command added under commands.ts, specify the respective declaration here. Ex: "PreserveSession(): Chainable<Element>,"
    }
  }
}
