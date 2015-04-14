Feature: SlugFactory
	I want to create URL slugs

Scenario Outline: Create
	Given title is <title>
	When I call SlugFactory.CreateSlug(title)
	Then the result should be <slug>

	Examples:
	| title                   | slug                  |
	| quick brown fox         | quick-brown-fox       |
	| Men's Open              | mens-open             |
	| Jess' Open              | jess-open             |
	| The 'British' Open      | the-british-open      |
	| the "12th british" open | the-12th-british-open |
	| news 1 - title          | news-1-title          |