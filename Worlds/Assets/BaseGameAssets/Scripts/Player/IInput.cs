using Unity.Mathematics;

namespace Player
{
    public interface IInput
    {
        float3 axisInput { get; set; }
        LastButton lastButton { get; set; }
    }

    public enum LastButton
    {
        none,
        fire1,
        fire2,
        fire3,
    }
}