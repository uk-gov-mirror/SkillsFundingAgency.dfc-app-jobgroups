Feature: View Job Group LMI

@JobGroup @Smoke
Scenario: Navigate to Job Group page
	Given I am on the nurse job profile page
	When I click the Explore job trends for this group link
	Then I am on the Job group: Nurses page

	@JobGroup
	Scenario: Job Growth information is displayed
	Given I am on the nurse job profile page
	When I click the Explore job trends for this group link
	Then I am on the Job group: Nurses page
	And the Job growth information is displayed

	@JobGroup
	Scenario: Qualifications information is displayed
	Given I am on the nurse job profile page
	When I click the Explore job trends for this group link
	Then I am on the Job group: Nurses page
	And the Qualifications information is displayed

	@JobGroup
	Scenario: Regional information is displayed
	Given I am on the nurse job profile page
	When I click the Explore job trends for this group link
	Then I am on the Job group: Nurses page
	And the Regional information is displayed

	@JobGroup
	Scenario: Industry information is displayed
	Given I am on the nurse job profile page
	When I click the Explore job trends for this group link
	Then I am on the Job group: Nurses page
	And the Industry information is displayed