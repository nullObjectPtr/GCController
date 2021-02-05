using HovelHouse.GameController;
using Rewired;

public interface IRewiredAppleControllerAdapter
{
    CustomController VirtualController { get; }
    GCController AppleController { get; }

    GCControllerElement GetGCElementForRewiredElementName(string rewiredElementName);
    string GetElementName(GCControllerElement element);
}
