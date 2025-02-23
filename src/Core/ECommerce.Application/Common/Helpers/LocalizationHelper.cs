using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;

namespace ECommerce.Application.Common.Helpers;

public sealed class LocalizationHelper : ISingletonDependency
{
    private readonly ILocalizationService _localizationService;

    public LocalizationHelper(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string this[string key] => _localizationService.GetLocalizedString(key);

    public string this[string key, string language] => _localizationService.GetLocalizedString(key, language);
}