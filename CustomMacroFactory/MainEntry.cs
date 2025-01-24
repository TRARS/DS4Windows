using CustomMacroFactory.MVVM.ViewModels;
using CustomMacroFactory.MVVM.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using TrarsUI.Shared.Helper.Extensions;
using TrarsUI.Shared.Interfaces;
using TrarsUI.Shared.Interfaces.UIComponents;
using TrarsUI.Shared.Services;

namespace CustomMacroFactory
{
    public static class MainEntry
    {
        private static readonly IHost host = GetHostBuilder().Build();

        private static IHostBuilder GetHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                       .ConfigureServices(sc =>
                       {
                           // Service
                           sc.AddSingleton<ICreateChildFormService, CreateChildFormService>();
                           sc.AddTransient<IDebouncerService, DebouncerService>();
                           sc.AddTransient<IDialogYesNoService, DialogYesNoService>();
                           sc.AddTransient<IDispatcherService, DispatcherService>();
                           sc.AddSingleton<IMessageBoxService, MessageBoxService>();
                           sc.AddTransient<IStringEncryptorService, StringEncryptorService>();
                           sc.AddScoped<ITokenProviderService, TokenProviderService>();
                           sc.AddSingleton<IContentProviderService, CustomMacroFactory.EntryService>();

                           // UI组件VM
                           sc.AddFormFactory<IuTitleBarVM, uTitleBarVM>();
                           sc.AddFormFactory<IuRainbowLineVM, uRainbowLineVM>();
                           sc.AddFormFactory<IuClientVM, uClientVM>();

                           // MainWindow MainWindowVM
                           sc.AddFormFactory<IMainWindow, IMainWindowEmpty, MainWindow>(sp =>
                           {
                               using (var scope = sp.CreateScope())
                               {
                                   var mainwindow = (MainWindow)(scope.ServiceProvider.GetRequiredService<IMainWindowEmpty>());
                                   {
                                       mainwindow.DataContext = scope.ServiceProvider.GetRequiredService<IMainWindowVM>();
                                       mainwindow.SizeToContent = SizeToContent.WidthAndHeight;
                                       mainwindow.MinWidth = 233;
                                       mainwindow.MinHeight = 423;
                                       mainwindow.MaxWidth = 1920;
                                       mainwindow.MaxHeight = 1080;
                                   }
                                   return mainwindow;
                               }
                           });
                           sc.AddTransient<IMainWindowVM, MainWindowVM>();

                           // ChildForm ChildFormVM
                           sc.AddFormFactory<IChildForm, IChildFormEmpty, ChildForm>(sp =>
                           {
                               using (var scope = sp.CreateScope())
                               {
                                   var childForm = (ChildForm)scope.ServiceProvider.GetRequiredService<IChildFormEmpty>();
                                   {
                                       childForm.DataContext = scope.ServiceProvider.GetRequiredService<IChildFormVM>();
                                       childForm.SizeToContent = SizeToContent.WidthAndHeight;
                                   }
                                   return childForm;
                               }
                           });
                           sc.AddTransient<IChildFormVM, ChildFormVM>();

                           // 其他组件VM
                           sc.AddSingleton<MacroViewerVM>();
                           sc.AddSingleton<ImageColorPickerVM>();
                       });
        }

        static MainEntry()
        {
            host.Start();
        }

        public static T GetRequiredService<T>() where T : notnull
        {
            return host.Services.GetRequiredService<T>();
        }
    }
}
