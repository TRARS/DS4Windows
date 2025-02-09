using CustomMacroFactory.MVVM.Helpers;
using CustomMacroFactory.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using TrarsUI.Shared.Helpers.Extensions;
using TrarsUI.Shared.Interfaces;
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
            return _serviceProvider.GetRequiredService<IAbstractFactory<MacroViewerVM>>().Create();
        }

        public static void Register(IServiceCollection services)
        {
            services.AddFormFactory<MacroViewerVM>();
            services.AddFormFactory<ImageColorPickerVM>();
            services.AddSingleton<Manager>();
        }
    }
}
