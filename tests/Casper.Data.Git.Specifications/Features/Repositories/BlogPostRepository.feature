Feature: BlogPostRepository
	As a developer
	I want to publish blog posts to a website

Scenario: IsPublishedAsync when blog post has been published
	Given I have published a blog post
	When I call IsPublishedAsync(path)
	Then the result should be true

Scenario Outline: PublishAsync 
	Given Title is <title>
		And Content is <content>
		And Published is <published>
		And Author is <author>
	When I call PublishAsync(PublishBlogPost command)
	Then the blog post should saved to <fileName> file
		And the file contents should be <fileContents>
		And the file should be committed to the master branch of the git repository
		And the master branch should be pushed to the remote server

	Examples: 
	| title        | content                   | published  | author                   | fileName                       | fileContents                                              |
	| Women's Open | Will be held in Melbourne | 2015-02-05 | Tim, tim@example.com     | blog/2015/02/05/womens-open.md | # Women's Open{newline}{newline}Will be held in Melbourne |
	| Men's Open   | Will be held in Sydney    | 2015-12-05 | Susan, susan@example.com | blog/2015/12/05/mens-open.md   | # Men's Open{newline}{newline}Will be held in Sydney      |