using HovelHouse.GameController;
using Rewired;

public interface IRewiredAppleControllerAdapter
{
    CustomController VirtualController { get; }
    GCController AppleController { get; }

    GCControllerElement GetGCElementForRewiredElementId(
        ControllerElementType elementType,
        int elementId);
}
