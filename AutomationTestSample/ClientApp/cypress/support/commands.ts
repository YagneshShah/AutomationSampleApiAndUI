// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })

/*
NOTE: Each custom command that you add, please add the command under cypress global namespace in support/index.ts file. 
This way .ts files will not give "Property 'SignInRails' does not exist on type 'cy & EventEmitter'.ts(2339)" error for detecting custom commands
Also, please dont add any new custom commands as the file will become too big in future. As part of BDD, we would write these reusable methods under 'bdd/app' and 'bdd/common' directories.
*/



// Cypress is 'promise aware'. If a command returns a promise, then it
// waits for that promise to resolve.
//
// This cute trick means that you can use cy.Await which will prevent
// Cypress from executing any more commands from the command queue until
// the promise is resolved.
//
// Example usage:
//   cy.Await(fetch(Page.excelUrl))
//     .then(r => r.text())
//     .then(expectStringSeemsLikeACorrectCSV)
//
// Example _recommended_ solution without this helper (wtf?):
//   cy.wrap(null).then(() => {
//     return fetch(Page.excelUrl)
//       .then(r => r.text())
//       .then(expectStringSeemsLikeACorrectCSV)
//   }
//
// See:
// - https://docs.cypress.io/api/utilities/promise
Cypress.Commands.add("Await", (promise) => promise);
