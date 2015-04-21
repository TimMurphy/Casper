Feature: BlogPostRepository
	As a developer
	I want to publish blog posts to a website

Background: 
	Given TimeZoneId is Russian Standard Time

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
	| title        | content                   | published                         | author                    | fileName                       | fileContents                                                                                                                                                                                                                                                     |
	| Women's Open | Will be held in Melbourne | 2015-02-05T00:00:00.0000000+00:00 | Tim <tim@example.com>     | blog/2015/02/05/womens-open.md | ---{newline}Title: Women's Open{newline}Published: 2015-02-05T03:00:00.0000000+03:00{newline}Author:{newline}  Name: Tim{newline}  Email: tim@example.com{newline}  TimeZoneId: Russian Standard Time{newline}{newline}---{newline}Will be held in Melbourne |
	| Men's Open   | Will be held in Sydney    | 2015-06-30T00:00:00.0000000+00:00 | Susan <susan@example.com> | blog/2015/06/30/mens-open.md   | ---{newline}Title: Men's Open{newline}Published: 2015-06-30T03:00:00.0000000+03:00{newline}Author:{newline}  Name: Susan{newline}  Email: susan@example.com{newline}  TimeZoneId: Russian Standard Time{newline}{newline}---{newline}Will be held in Sydney  |
