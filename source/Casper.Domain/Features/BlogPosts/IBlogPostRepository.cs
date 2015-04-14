using System.IO;
using System.Threading.Tasks;
using Casper.Domain.Features.BlogPosts.Commands;

namespace Casper.Domain.Features.BlogPosts
{
    public interface IBlogPostRepository
    {
        Task<bool> IsPublishedAsync(string path);
        Task PublishAsync(PublishBlogPost command);
    }
}