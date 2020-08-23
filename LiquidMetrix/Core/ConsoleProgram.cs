using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LiquidMetrix
{
    public class ConsoleProgram : IConsoleProgram
    {
        private readonly ILogger<ConsoleProgram> _logger;
        private readonly IRoverContainer _roverContainer;
        public ConsoleProgram(ILogger<ConsoleProgram> logger,
                              IRoverContainer roverContainer)
        {
            _logger = logger;
            _roverContainer = roverContainer;
        }
        public void Orders(string input)
        {
            if (input.Contains("-new", StringComparison.OrdinalIgnoreCase))
            {
                int newRover = _roverContainer.Add();
                Console.WriteLine($"Rover {newRover}");
            }
            else if (input.Contains("-move", StringComparison.OrdinalIgnoreCase))
            {
                var moveOrder = input.Split(' ')[1];
                if (Regex.IsMatch(moveOrder, @"^[LR]"))
                {
                    var rover = _roverContainer.SelectedRover;
                    if ( rover.Move(moveOrder) == StatusCode.Successful)
                        Console.WriteLine($"Rover {_roverContainer.SelectedRoverId} has move to: {rover.ToString()}");
                    else
                        _logger.LogWarning($"Internal Error.");
                }
                else
                {
                    _logger.LogWarning($"Move Order is in valid: {moveOrder}");
                }
            }
            else if (input.Contains("-list", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var rover in _roverContainer.Rovers)
                {
                    Console.WriteLine($"Rover {rover.Key}, {rover.ToString()}");
                }
            }
            else if (input.Contains("selectedRover"))
            {
                Console.WriteLine($"Selected Rover:  {_roverContainer.SelectedRoverId} => {_roverContainer.SelectedRover.ToString()}");
            }
            else if (input.Contains("changeRover"))
            {
                input = input.Split(' ')[1];
                if (Regex.IsMatch(input, @"^[1-9]$"))
                {
                    if (int.TryParse(input, out var id))
                    {
                        _roverContainer.SelectedRoverId = id;
                        Console.WriteLine($"Rover {_roverContainer.SelectedRoverId} => {_roverContainer.SelectedRover.ToString()}");
                    }
                    else
                    {
                        _logger.LogWarning($"Input is in valid: {input}");
                    }
                }
            }
            else
            {
                _logger.LogWarning($"Input is in valid: {input}");
            }

        }
    }
}
