using UrlShortenerWeb.DTO;
using UrlShortenerWeb.Models;

namespace UrlShortenerWeb.Interfaces
{
    public interface IDescriptionService
    {
        Task EditDescriptionAsync(DescriptionEditDto descriptionDto);
        Description FindDescriptionById(int id);
    }
}
