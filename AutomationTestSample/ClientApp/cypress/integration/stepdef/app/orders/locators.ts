/* Note:
 * 1. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/app/* directory are meant for steps related to app/project we are working on.
 * 2. Files(like Precondition, Actions, Assertions, etc) under ./stepdef/lib/* directory are meant for common steps libraries which can be used across multiple apps/projects. 
 *    No code which is related to project should be in here.
 */

export const ordersPage = {
  //locators: /new-orders form page
  pageTitle: '#tableLabel',
  mrnFieldInput: '#mrn',
  firstNameFieldInput: '#first-name',
  lastNameFieldInput: '#last-name',
  accessionNumberFieldInput: '#accession-number',
  orgCodeFieldInput: '#org-code',
  siteIdFieldInput: '#site-id',
  modalityFieldInput: '#modality',
  studyDateTimeFieldInput: '#study-date-time',

  //locators: /orders page
  ordersTableColumnContent: index =>`td:nth-child(${index})`,
}
