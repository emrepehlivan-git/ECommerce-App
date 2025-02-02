namespace ECommerce.Application.Common.Interfaces;

public interface ILocalizationService
{
    string GetLocalizedString(string key, string language);
    string GetLocalizedString(string key);
}