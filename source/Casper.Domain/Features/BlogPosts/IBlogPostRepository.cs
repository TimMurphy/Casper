using System.Threading.Tasks;

namespace Casper.Domain.Features.BlogPosts
{
    public interface IBlogPostRepository
    {
        Task PublishAsync(BlogPost blogPost);
    }
}