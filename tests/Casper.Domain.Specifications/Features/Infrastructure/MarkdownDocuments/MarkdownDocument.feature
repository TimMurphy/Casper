Feature: MarkdownDocument

Scenario Outline: Name property
	Given RelativeUri is <relativeUri>
	When I call Name
	Then the result should be <name>

	Examples:
	| relativeUri | name |
	| x           | x    |
	| a/b/c       | c    |
