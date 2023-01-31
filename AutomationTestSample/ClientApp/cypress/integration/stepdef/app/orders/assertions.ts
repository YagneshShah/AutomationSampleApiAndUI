/* Note:
 * 1. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/app/* directory are meant for steps related to app/project we are working on.
 * 2. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/lib/* directory are meant for common steps libraries which can be used across multiple apps/projects. 
 *    No code which is related to project should be in here.
 */

import { DataTable, Then } from '@badeball/cypress-cucumber-preprocessor'
import { ordersPage } from './locators'
import * as utils from '../../lib/utilities'

Then('I should be on page containing url {string} and title {string}', (url: string, pageTitle: string) => {
  cy.url().should('include', url)
  cy.get(ordersPage.pageTitle).should('have.text', pageTitle)
})

Then(`I confirm that newly created order record is present on the page`,(table: DataTable)=>{
  table.hashes().forEach(rowData => {
    cy.contains(rowData.accessionNumber).parent().within(() =>{
      cy.get(ordersPage.ordersTableColumnContent(1)).should('have.text', rowData.accessionNumber)
      cy.get(ordersPage.ordersTableColumnContent(2)).should('have.text', rowData.organisation)
      cy.get(ordersPage.ordersTableColumnContent(3)).should('have.text', rowData.siteId)
      cy.get(ordersPage.ordersTableColumnContent(4)).should('have.text', rowData.mrn)
      cy.get(ordersPage.ordersTableColumnContent(5)).should('have.text', `${rowData.firstName} ${rowData.lastName}`)
      cy.get(ordersPage.ordersTableColumnContent(6)).should('have.text', rowData.modality)
      cy.get(ordersPage.ordersTableColumnContent(7)).should('contain', utils.getDate(rowData.studyDateTime, "dd/MM/yyyy")); //ex of expected format: '2023-01-31T02:10'
      cy.get(ordersPage.ordersTableColumnContent(8)).should('have.text', rowData.status)
    })
  });
})

Then (`I delete only if it exists the order with accessionNumber {int}`, (accessionNumber: number)=>{
  cy.get('body').then(($body) => {
    // synchronously ask for the body's text
    // and do something based on whether it includes
    // another string
    if ($body.text().includes(`${accessionNumber}`)) {
      // yup found it
      cy.contains(accessionNumber).parent().within(() =>{
          cy.get(`${ordersPage.ordersTableColumnContent(9)} > i`).click().wait(3000);
      })
    } else {
      // nope not here...do nothing as the record doesnt exist and hence no need for delete record
    }
  })
})
