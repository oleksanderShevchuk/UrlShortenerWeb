using UrlShortenerWeb.Models;
using UrlShortenerWeb.Models.DTO;

namespace UrlShortenerWeb.Services
{
    public interface IDescriptionService
    {
        Task EditDescriptionAsync(DescriptionEditDto descriptionDto);
        Description FindDescriptionById(int id);
    }
}
