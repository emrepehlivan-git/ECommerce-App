using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Common.Helpers;

public class LocalizationHelper
{
    private readonly ILocalizationService _localizationService;

    public LocalizationHelper(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string this[string key] => _localizationService.GetLocalizedString(key);

    public string this[string key, string language] => _localizationService.GetLocalizedString(key, language);
}