Feature: UploadFile
	As a developer
	I want to add files to my website

Scenario: Upload file with valid data
	Given UploadFile command is valid
	When I send the UploadFile command
	Then the file should be saved
	And a UploadedFile event should be published
