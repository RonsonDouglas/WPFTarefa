using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Tarefas.Presentation.Services.Interfaces;
using Tarefas.Presentation.Services;
using Tarefas.Presentation.ViewModels;

namespace Tarefas.Presentation
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Registro do HttpClient e do serviço
            services.AddHttpClient<ITarefaService, TarefaService>();

            // Registro dos ViewModels
            services.AddSingleton<MainViewModel>();

            // Registro das janelas
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
