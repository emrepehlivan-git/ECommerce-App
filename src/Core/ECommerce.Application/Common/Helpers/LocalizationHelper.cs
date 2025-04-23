using ECommerce.Application.Common.Interfaces;

namespace ECommerce.Application.Common.Helpers;

public class LocalizationHelper(ILocalizationService localizationService)
{
    public string this[string key] => localizationService.GetLocalizedString(key);
    public string this[string key, string language] => localizationService.GetLocalizedString(key, language);
}