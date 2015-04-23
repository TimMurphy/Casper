Feature: PageRepository

Background: 
    Given a published page where RelativeUri is 'page-1'
    And a published page where RelativeUri is 'page-2'
    And a published page where RelativeUri is 'a/page-3'
    And a published page where RelativeUri is 'a/page-4'
    And a published page where RelativeUri is 'a/b/page-5'
    And a published page where RelativeUri is 'a/b/page-6'
    And a published blog post where RelativeUri is 'blog/2015/12/12/blog-post-1'
    And a published blog post where RelativeUri is 'blog/2015/01/05/blog-post-2'

Scenario: FindPublishedPagesAsync when directory is empty string
    Given directory is 'empty string'
    When I call FindPublishedPagesAsync(directory)
    Then pages with the following relative uris should be returned
        | relativeUri |
        | page-1      |
        | page-2      |

Scenario: FindPublishedPagesAsync when directory is a
    Given directory is 'a'
    When I call FindPublishedPagesAsync(directory)
    Then pages with the following relative uris should be returned
        | relativeUri |
        | a/page-3    |
        | a/page-4    |

Scenario: FindPublishedDirectoriesAsync
    Given directory is 'empty string'
    When I call FindPublishedDirectoriesAsync(directory)
    Then the following directories should be returned
        | relativeUri | name |
        | a           | a    |

Scenario: FindPublishedDirectoriesAsync when directory is a
    Given directory is 'a'
    When I call FindPublishedDirectoriesAsync(directory)
    Then the following directories should be returned
        | relativeUri | name |
        | a/b         | b    |

Scenario: FindPublishedDirectoriesAsync when directory is a/b
    Given directory is 'a/b'
    When I call FindPublishedDirectoriesAsync(directory)
    Then no directories should be returned
