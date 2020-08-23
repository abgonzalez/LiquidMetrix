using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LiquidMetrix
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(l => l.AddConsole())
                .Configure<LoggerFilterOptions>(c => c.MinLevel = LogLevel.Trace)
                .AddScoped<IGrid>(g => new Grid(40,30))
                .AddScoped<IRover>(g=>new Rover(g.GetService<ILogger<Rover>>(), g.GetService<IGrid>()))
                .AddScoped<IRoverContainer, RoverContainer>()
                .AddScoped<ConsoleProgram>()
                .BuildServiceProvider();

            var program = serviceProvider.GetRequiredService<ConsoleProgram>();

            Console.WriteLine("Rovers Manager");
            GetCommands();
            bool exit = false;
            while (!exit)
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (input.Contains("-exit", StringComparison.OrdinalIgnoreCase))
                    {
                        exit = true;
                    }
                    else if (input.Contains("-clear", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                    }
                    else if (input.Contains("-help", StringComparison.OrdinalIgnoreCase))
                    {
                        GetCommands();
                    }
                    else
                    {
                        program.Orders(input);
                    }
                }
            }
        }

        private static void GetCommands()
        {
            Console.WriteLine("Options:");
            Console.WriteLine("-new                      \tCreate a new rover, by the default position that is \"0 0 N\".");
            Console.WriteLine("-move \"R1R3L2L1\"        \tMove the selected rover to this position to [R1R3L2L1].");
            Console.WriteLine("-selectedRover            \tShow the current selected rover.");
            Console.WriteLine("-changeRover \"number\"   \tSelecte another rover to move.");
            Console.WriteLine("-listRovers               \tList all the active rovers");
            Console.WriteLine("-exit                     \tExit program");
            Console.WriteLine("-clear                    \tClear console");
            Console.WriteLine("-help                     \tHelp");
        }
    }
}
