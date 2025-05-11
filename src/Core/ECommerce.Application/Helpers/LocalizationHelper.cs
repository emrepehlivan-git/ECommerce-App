using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Helpers;

public class LocalizationHelper(ILocalizationService localizationService)
{
    public string this[string key] => localizationService.GetLocalizedString(key);
    public string this[string key, string language] => localizationService.GetLocalizedString(key, language);
}