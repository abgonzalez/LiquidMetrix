using System.Collections.Generic;

namespace LiquidMetrix
{
    public interface IRoverContainer
    {
        Dictionary<int, Rover> Rovers { get; }
        Rover SelectedRover { get; }
        int SelectedRoverId { get; set; }
        Rover Find(int key);
        bool Contains(int key);
        int Add();
    }
}