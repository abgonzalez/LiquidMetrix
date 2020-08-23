using System.Diagnostics.CodeAnalysis;

namespace LiquidMetrix
{
    public interface IRover
    {
        Rover createNew();
        int CompareTo([AllowNull] Rover other);
        string GetCurrentPosition();
        StatusCode Move(string inputOrders);
        StatusCode SetPosition(int X = 0, int Y = 0, char direction = 'N');
    }
}