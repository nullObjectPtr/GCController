using System;
using System.ComponentModel;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public class RewiredSiriSecondGenRemoteAdapter : AbstractSiriRemoteAdapter
{
    public RewiredSiriSecondGenRemoteAdapter(
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
        UpdateButton(GCMicroGamepadElementType.DPadRingLeft);
        UpdateButton(GCMicroGamepadElementType.DPadRingRight);
        UpdateButton(GCMicroGamepadElementType.DPadRingUp);
        UpdateButton(GCMicroGamepadElementType.DPadRingDown);
        
        // Special handling for the menuButton
        var record =
            ElementConverterMap.Records.FirstOrDefault(r => r.microGamepadElementType == GCMicroGamepadElementType.ButtonMenu);

        if (record == null) { return; }
        VirtualController.SetButtonValue(record.RewiredElementName, MenuButtonWasPressedThisUpdate || MenuButtonIsPressed);
        MenuButtonWasPressedThisUpdate = false;
    }
}
