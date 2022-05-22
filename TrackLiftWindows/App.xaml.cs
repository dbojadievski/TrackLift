using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using TrackLift.DataLayer.Interfaces;
using TrackLift.DataLayer.Windows.Repositories;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.DependencyInjection;
using TrackLift.ViewModels;
using Microsoft.Extensions.Hosting;

namespace TrackLiftWindows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>(x => new DebugLoggerProvider().CreateLogger("logFile.log"));
            services.AddSingleton<IExerciseRepository, ExerciseRepository>();
            services.AddSingleton<ExercisesVM>();
            services.AddSingleton<MainWindow>();
            Provider = services.BuildServiceProvider();
        }

        protected void OnStartup(object sender, StartupEventArgs e)
        {

            var mainWindow = Provider.GetService<MainWindow>();
            mainWindow.Show();
        }

        public ServiceProvider Provider
        {
            get; private set;
        }

    }
}
