using ECommerce.Application.Common.Interfaces;
using ECommerce.SharedKernel;

namespace ECommerce.Application.Common.Helpers;

public sealed class L : ISingletonDependency
{
    private readonly ILocalizationService _localizationService;

    public L(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public string this[string key] => _localizationService.GetLocalizedString(key);

    public string this[string key, string language] => _localizationService.GetLocalizedString(key, language);
}