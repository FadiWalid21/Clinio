using Clinio.Application.Common.Localization;
using Clinio.Application.Interfaces;
using Microsoft.Extensions.Localization;

namespace Clinio.Infrastructure.Services;

public class LocalizationService(IStringLocalizer<SharedResource> _localizer ) : ILocalizationService
{
    public string Get(string key)
    {
        return _localizer[key];
    }
    public string Get(string key, params object[] args)
    {
        return _localizer[key, args]; // replaces {0}, {1} etc.
    }
}