using System;
using System.Linq;
using HovelHouse.GameController;

public class RewiredSiriRemoteAdapter : AbstractSiriRemoteAdapter
{
    public event Action<GCControllerButtonInput, bool> OnButtonValueChanged;
    public event Action<GCControllerAxisInput, float> OnAxisValueChanged;
    
    private readonly GCMicroGamepad _microGamepad;
    
    private bool MenuButtonWasPressedThisUpdate;
    private bool MenuButtonIsPressed;

    public RewiredSiriRemoteAdapter(
        GCController controller, 
        int profileId, 
        RewiredToGCMicroGamepadElementMap elementConverterMap)
    : base(controller, profileId, elementConverterMap)
    {
        
    }

    protected override void OnInputSourceUpdated()
    {
        UpdateButton(GCMicroGamepadElementType.ButtonA);
        UpdateButton(GCMicroGamepadElementType.ButtonX);
        UpdateButton(GCMicroGamepadElementType.DPadLeft);
        UpdateButton(GCMicroGamepadElementType.DPadRight);
        UpdateButton(GCMicroGamepadElementType.DPadUp);
        UpdateButton(GCMicroGamepadElementType.DPadDown);

        // Special handling for the menuButton
        var record =
            ElementConverterMap.Records.FirstOrDefault(r => r.microGamepadElementType == GCMicroGamepadElementType.ButtonMenu);

        if (record == null) { return; }
        VirtualController.SetButtonValue(record.RewiredElementName, MenuButtonWasPressedThisUpdate || MenuButtonIsPressed);
        MenuButtonWasPressedThisUpdate = false;
    }
}
