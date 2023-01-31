/* Note:
 * 1. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/app/* directory are meant for steps related to app/project we are working on.
 * 2. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/lib/* directory are meant for common steps libraries which can be used across multiple apps/projects. 
 *    No code which is related to project should be in here.
 */

import { DataTable, When } from '@badeball/cypress-cucumber-preprocessor'
import * as utils from '../../lib/utilities'
import { ordersPage } from './locators';

When('I click orders from topnav', () => {
  cy.contains('Orders').click();
})

When(`I fill and submit the new order form with below data`,(table: DataTable)=>{
  table.hashes().forEach(rowData => {
    cy.get(ordersPage.mrnFieldInput).click({ force: true }).type(rowData.mrn);
    cy.get(ordersPage.firstNameFieldInput).click().type(rowData.firstName);
    cy.get(ordersPage.lastNameFieldInput).click().type(rowData.lastName);
    cy.get(ordersPage.accessionNumberFieldInput).click().type(rowData.accessionNumber);
    cy.get(ordersPage.orgCodeFieldInput).select(rowData.organisation);
    cy.get(ordersPage.siteIdFieldInput).select(rowData.siteId);
    cy.get(ordersPage.modalityFieldInput).select(rowData.modality);
    cy.get(ordersPage.studyDateTimeFieldInput).click().type(utils.getDate(rowData.studyDateTime, "yyyy-MM-dd'T'hh:mm")); //ex of expected format: '2023-01-31T02:10'
    cy.contains("Submit").click();
  });
})
