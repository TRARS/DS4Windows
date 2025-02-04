using CustomMacroFactory.MVVM.Helpers;
using CustomMacroFactory.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using TrarsUI.Shared.Interfaces.UIComponents;

namespace CustomMacroFactory
{
    internal class EntryService : IContentProviderService
    {
        private readonly IServiceProvider _serviceProvider;

        public EntryService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IContentVM Create()
        {
            return _serviceProvider.GetRequiredService<MacroViewerVM>();
        }

        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<MacroViewerVM>();
            services.AddSingleton<ImageColorPickerVM>();
            services.AddSingleton<Manager>();
        }
    }
}
