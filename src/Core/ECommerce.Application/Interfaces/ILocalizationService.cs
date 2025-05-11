namespace ECommerce.Application.Interfaces;

public interface ILocalizationService
{
    string GetLocalizedString(string key, string language);
    string GetLocalizedString(string key);
}