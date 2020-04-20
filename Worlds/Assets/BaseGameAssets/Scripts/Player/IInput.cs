using Unity.Mathematics;

namespace Worlds.Player
{
    public interface IInput
    {
        float3 axisInput { get; set; }
        Button[] latestButtons { get; set; }

        bool ButtonsContains(Button button);
    }

    public enum Button
    {
        none,
        fire1,
        fire2,
        fire3,
    }
}