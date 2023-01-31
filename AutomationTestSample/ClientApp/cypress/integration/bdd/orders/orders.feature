Feature: Orders functionality tests
  
@orders
Scenario: TC001 - Orders - delete order if exists and then create order then assert order created
  Given I visit page "/"
  When I click orders from topnav
  Then I should be on page containing url '/orders' and title 'Orders'
  And I delete only if it exists the order with accessionNumber 1001
  When I click on button with name 'Create New'
  Then I should be on page containing url '/new-order' and title 'New Order'
  When I fill and submit the new order form with below data
    | mrn  | firstName | lastName | accessionNumber | organisation | siteId                    | modality | studyDateTime |
    | 1001 | Winter    | Soldier  | 1001            | LUM          | Northern Beaches Hospital | MR       | today         |
  Then I should be on page containing url '/orders' and title 'Orders'
  And I confirm that newly created order record is present on the page
    | mrn  | firstName | lastName | accessionNumber | organisation | siteId         | modality | studyDateTime | status |
    | 1001 | Winter    | Soldier  | 1001            | LUM          | Baulkham Hills | MR       | today         | SC     |
  #Note: 
    #In above assertion, Tests fails when asserting "siteId = Northern Beaches Hospital" as it is an actual defect which test is catching
    #Hence, just for this demo purpose if you wish to see the test pass, then we pass "siteId = Baulkham Hills" instead of 'Northern Beaches Hospital'
    #Defect description: whatever siteId we select from dropdown while creating order, then next +1 siteId from that dropdown is used to create the order which is a application defect in Post API call



#Note: This is copy of above scenario to create order but after creating order when you assert siteId then test fails as expected due to genuine application defect
#This is a scenario to fail the test and check how the MochaAwesome PDF report looks like to debug the failed scenario with failure screenshot and reason...
# Sample Failure Scenario to test how ./clientapp/cypress/Mochawesome-report pdf looks like for debugging purpose
@orders
Scenario: TC002 - assert created order fails due to app defect
  Given I visit page "/"
  When I click orders from topnav
  Then I should be on page containing url '/orders' and title 'Orders'
  And I delete only if it exists the order with accessionNumber 1001
  When I click on button with name 'Create New'
  Then I should be on page containing url '/new-order' and title 'New Order'
  When I fill and submit the new order form with below data
    | mrn  | firstName | lastName | accessionNumber | organisation | siteId                    | modality | studyDateTime |
    | 1001 | Winter    | Soldier  | 1001            | LUM          | Northern Beaches Hospital | MR       | today         |
  Then I should be on page containing url '/orders' and title 'Orders'
  And I confirm that newly created order record is present on the page
    | mrn  | firstName | lastName | accessionNumber | organisation | siteId                    | modality | studyDateTime | status |
    | 1001 | Winter    | Soldier  | 1001            | LUM          | Northern Beaches Hospital | MR       | today         | SC     |

