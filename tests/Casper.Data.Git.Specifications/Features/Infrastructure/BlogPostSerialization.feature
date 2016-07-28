Feature: BlogPostSerialization

Scenario: Deserialize blog post
	Given markdownWithFrontMatter is 
		"""
		---
		Title: Results 
		Published: 2015-04-15T00:00:00.0000000+10:00
		Author:
		  Name: Susan Linge
		  Email: admin@croquet-australia.com.au
		  TimeZoneId: AUS Eastern Standard Time

		---
		## Association Croquet Gold Medal

		**Gold Medal** Simon Hockey SA
		"""
	When I call BlogPost.Deserialize(markdownWithFrontMatter)
	Then a blog post should be returned
	And Title should be 'Results'
	And Published should be '2015-04-15T00:00:00.0000000+10:00'
	And Author.Name should be 'Susan Linge'
	And Author.Email should be 'admin@croquet-australia.com.au'
	And Author.TimeZoneId should be 'AUS Eastern Standard Time'
	And Content should be:
		"""
		## Association Croquet Gold Medal

		**Gold Medal** Simon Hockey SA
		"""
