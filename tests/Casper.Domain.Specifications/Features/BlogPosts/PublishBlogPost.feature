Feature: PublishBlogPost
	As a developer
	I want to add blog posts to my website

Scenario: Publish blog post with valid data
	Given title is valid
	And content is valid
	And author is valid
	When I send the PublishBlogPost command
	Then the blog post should be created
	And a PublishedBlogPost event should be published
