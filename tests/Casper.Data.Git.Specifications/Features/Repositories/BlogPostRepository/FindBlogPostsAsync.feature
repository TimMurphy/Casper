Feature: FindBlogPostsAsync

Background:
    Given the first blog post was published 2015-01-01
    And the last blog post was published 2015-01-30
    And the title of each blog post is 'dummy title {day}'
    And one blog post is published every day
    
Scenario: Find first page of published blog posts
    Given page number is 1
    And page size is 10
    When I call FindPublishedBlogPostsAsync(pagination)
    Then 10 blog posts should be returned
    And the relative uri of each blog post should be
        | relativeUri                    |
        | blog/2015/01/30/dummy-title-30 |
        | blog/2015/01/29/dummy-title-29 |
        | blog/2015/01/28/dummy-title-28 |
        | blog/2015/01/27/dummy-title-27 |
        | blog/2015/01/26/dummy-title-26 |
        | blog/2015/01/25/dummy-title-25 |
        | blog/2015/01/24/dummy-title-24 |
        | blog/2015/01/23/dummy-title-23 |
        | blog/2015/01/22/dummy-title-22 |
        | blog/2015/01/21/dummy-title-21 |

Scenario: Find second page of published blog posts
    Given page number is 2
    And page size is 5
    When I call FindPublishedBlogPostsAsync(pagination)
    Then 5 blog posts should be returned
    And the relative uri of each blog post should be
        | relativeUri                    |
        | blog/2015/01/25/dummy-title-25 |
        | blog/2015/01/24/dummy-title-24 |
        | blog/2015/01/23/dummy-title-23 |
        | blog/2015/01/22/dummy-title-22 |
        | blog/2015/01/21/dummy-title-21 |