using BasicLinq.Data;
using BasicLinq.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BasicLinq;

internal static class NewBaseType
{
        [STAThread]
        static void Main()
        {
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();

                // Leer cadena de conexión desde appsettings.json
                Application.Run(
                    new MainForm(
                        new AppDbContext(
                            new DbContextOptionsBuilder<AppDbContext>()
                                .UseSqlServer(
                                    new ConfigurationBuilder()
                                        .SetBasePath(AppContext.BaseDirectory)
                                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                                        .Build().GetConnectionString("DefaultConnection") ??
                                    throw
                                        new InvalidOperationException("No se encontró 'DefaultConnection' en appsettings.json")
                                    ).Options
                        )
                    )
                );
        }
}

