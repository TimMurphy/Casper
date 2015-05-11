using System.Threading.Tasks;

namespace Casper.Domain.Features.Files
{
    public interface IFileRepository
    {
        Task PublishAsync(UploadedFile uploadedFile);
    }
}