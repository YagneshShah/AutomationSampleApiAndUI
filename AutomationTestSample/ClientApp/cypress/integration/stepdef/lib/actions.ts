/* Note:
 * 1. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/app/* directory are meant for steps related to app/project we are working on.
 * 2. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/lib/* directory are meant for common steps libraries which can be used across multiple apps/projects. 
 *    No code which is related to project should be in here.
 */

import { When } from '@badeball/cypress-cucumber-preprocessor'

When('I click on button with name {string}', (buttonName: string) => {
  cy.contains("button", buttonName).click({ force: true })
})

When('On confirmation popup dialog I click {string} button', (buttonName: string) => {
  cy.contains("button", buttonName).click({ force: true })
})
