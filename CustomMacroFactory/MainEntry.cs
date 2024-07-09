using CustomMacroFactory.MainView;
using CustomMacroFactory.MainView.UserControlEx.ClientEx;
using CustomMacroFactory.MainView.UserControlEx.PixelPicker;
using CustomMacroFactory.MainView.UserControlEx.RainbowLineEx;
using CustomMacroFactory.MainView.UserControlEx.TitleBarEx;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
                           sc.AddSingleton<MainWindow>(sp => new() { DataContext = sp.GetRequiredService<MainWindow_viewmodel>() });
                           sc.AddSingleton<MainWindow_viewmodel>();

                           sc.AddSingleton<uTitleBar_viewmodel>();
                           sc.AddSingleton<uRainbowLine_viewmodel>();
                           sc.AddSingleton<uClient_viewmodel>();

                           sc.AddSingleton<uPixelPicker>(sp => new() { DataContext = sp.GetRequiredService<uPixelPicker_viewmodel>() });
                           sc.AddSingleton<uPixelPicker_viewmodel>();
                       });
        }

        static MainEntry()
        {
            host.Start();
        }

        public static T GetService<T>() where T : notnull
        {
            return host.Services.GetRequiredService<T>();
        }
    }
}
