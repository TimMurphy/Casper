using System;
using Casper.Data.Git.Infrastructure;
using Casper.Data.Git.Specifications.Helpers;
using FluentAssertions;
using OpenMagic.Extensions;
using TechTalk.SpecFlow;

namespace Casper.Data.Git.Specifications.Features.Infrastructure.Steps
{
    [Binding]
    public class BlogPostSerializationSteps
    {
        private readonly ActualData _actual;
        private readonly GivenData _given;
        private readonly IYamlMarkdown _yamlMarkdown;

        public BlogPostSerializationSteps(GivenData given, ActualData actual, IYamlMarkdown yamlMarkdown)
        {
            _given = given;
            _actual = actual;
            _yamlMarkdown = yamlMarkdown;
        }

        [Given(@"markdownWithFrontMatter is")]
        public void GivenMarkdownWithFrontMatterIs(string markdownWithFrontMatter)
        {
            _given.MarkdownWithFrontMatter = markdownWithFrontMatter;
        }

        [When(@"I call BlogPost\.Deserialize\(markdownWithFrontMatter\)")]
        public void WhenICallBlogPost_DeserializeMarkdownWithFrontMatter()
        {
            _actual.BlogPost = BlogPostSerialization.Deserialize("dummy/relative/uri", _given.MarkdownWithFrontMatter, _yamlMarkdown);
        }

        [Then(@"a blog post should be returned")]
        public void ThenABlogPostShouldBeReturned()
        {
            _actual.BlogPost.Should().NotBeNull();
        }

        [Then(@"Title should be '(.*)'")]
        public void ThenTitleShouldBe(string expectedTitle)
        {
            _actual.BlogPost.Title.Should().Be(expectedTitle);
        }

        [Then(@"Published should be '(.*)'")]
        public void ThenPublishedShouldBe(DateTime expectedPublished)
        {
            _actual.BlogPost.Published.Should().Be(expectedPublished);
        }

        [Then(@"Author\.Name should be '(.*)'")]
        public void ThenAuthor_NameShouldBe(string expectedName)
        {
            _actual.BlogPost.Author.Name.Should().Be(expectedName);
        }

        [Then(@"Author\.Email should be '(.*)'")]
        public void ThenAuthor_EmailShouldBe(string expectedEmail)
        {
            _actual.BlogPost.Author.Email.Should().Be(expectedEmail);
        }

        [Then(@"Author\.TimeZoneId should be '(.*)'")]
        public void ThenAuthor_TimeZoneIdShouldBe(string expectedTimeZoneId)
        {
            _actual.BlogPost.Author.TimeZoneInfo.Id.Should().Be(expectedTimeZoneId);
        }

        [Then(@"Content should be:")]
        public void ThenContentShouldBe(string expectedContent)
        {
            _actual.BlogPost.Content.NormalizeLineEndings().Should().Be(expectedContent.NormalizeLineEndings());
        }
    }
}