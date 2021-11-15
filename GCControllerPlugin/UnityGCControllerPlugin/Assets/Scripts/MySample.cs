using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using HovelHouse.GameController;
using TMPro;
using Rewired;

public class MySample : MonoBehaviour
{
    public enum EGlyphLoadMode
    {
        Precached,
        OnDemand
    };

    public EGlyphLoadMode GlyphLoadMode = EGlyphLoadMode.OnDemand;
    public UnityEngine.UI.Image anImage;
    public TMP_Text statusTextMesh;

    public float symbolPointSize;
    public UIImageSymbolWeight symbolWeight;
    public bool showFilled = true;
    public CachedGlyphProvider.LoadFillOption LoadFillOption;

    // These define how we map the rewired custom controller elements to the adapters
    public RewiredToGCMicroGamepadElementMap SecondGenSiriRemoteElementMap;
    public RewiredToGCMicroGamepadElementMap FirstGenSiriRemoteElementMap;
    public RewiredToGCExtendedGamepadElementMap ExtendedGamepadElementMap;
    
    // This maps the GCController element name to a SFSymbol name for the fallback glyph helper
    // which is used on iOS 13
    public GCControllerElementToSFSymbolNameMap xBoxFallbackMap;
    public GCControllerElementToSFSymbolNameMap dualShockFallbackMap;
        
    // Set this to true to test the fallback system on iOS 14
    // I don't know about you, but I don't have any iOS 13 devices to test with
    public bool ForceFallbackGlyphHelper;
    
    public ControllerElementToSpriteMap xboxFallbackGlyphs;
    public ControllerElementToSpriteMap ps4FallbackGlyphs;
    
    private GCController controller;
    private IGlyphProvider _glyphProvider;
    private IGlyphHelper _glyphHelper;
    
    void Awake()
    {
        statusTextMesh.text = "waiting for controller...";

        if (GlyphLoadMode == EGlyphLoadMode.Precached)
        {
            var symbolSet = new CachedGlyphProvider(symbolPointSize, symbolWeight);
            _glyphProvider = symbolSet;
        }
        else
        {
            var onDemandProvider = new OnDemandGlyphProvider(symbolPointSize, symbolWeight);
            _glyphProvider = onDemandProvider;
        }

        ReInput.ControllerConnectedEvent += OnControllerConnected;
    }

    private void OnControllerConnected(ControllerStatusChangedEventArgs obj)
    {
        var player = ReInput.players.GetPlayer(0);
        var controller = obj.controller;

        statusTextMesh.text =
            $"connected\nname:'{controller.name}'\nhardware name:'{controller.hardwareName}'\ncontroller map:'{controller.mapTypeString}'";

        var isControllerAssigned = ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controller);
        Debug.Log($"is controller assigned? {(isControllerAssigned ? "YES" : "NO")}");
        
        var isPS4Controller = CultureInfo.InvariantCulture.CompareInfo.IndexOf(controller.name, "dualshock",
            CompareOptions.OrdinalIgnoreCase) >= 0;
        
        // Determine which glyph helper were gonna use
        if (UIImage.SFSymbolsAreAvailable())
        {
            var appleController = GCController.Controllers().FirstOrDefault();
            
            Debug.Log($"got apple controller: {appleController.VendorName}");
        
            // Use the extended gamepad property to skip past physicalInputProfile
            // which was only added in macOS 11.0+
            var extendedGamepad = appleController.ExtendedGamepad;
            var microGamepad = appleController.MicroGamepad;
            
            Debug.Log($"extended gamepad {(extendedGamepad != null ? "YES" : "NO")}");
            Debug.Log($"micro gamepad {(microGamepad != null ? "YES" : "NO")}");
            
            IRewiredAppleControllerAdapter adapter = null;
            
            if (extendedGamepad != null)
            {
                adapter = new RewiredExtendedGamepadAdapter(appleController, 0, ExtendedGamepadElementMap);
            }
            else if (microGamepad != null)
            {
#if UNITY_TVOS
                UnityEngine.tvOS.Remote.allowExitToHome = false;
#endif
                // check if the siri remote that was connected is second gen or not...
                bool isSecondGen = microGamepad.DpadRing != null;
                if (isSecondGen)
                {
                    // we need to use a different element map, because if we're using the 2nd gen
                    // remote we do NOT want to use the dPad for input, we want the cardinal dPad
                    // instead
                    Debug.Log("Creating second gen siri remote adapter");
                    adapter = new RewiredSiriSecondGenRemoteAdapter(appleController, 1, SecondGenSiriRemoteElementMap);
                }
                else
                {
                    Debug.Log("Creating first gen siri remote adapter");
                    adapter = new RewiredSiriRemoteAdapter(appleController, 1, FirstGenSiriRemoteElementMap);
                }
            }
            
            // Debug log the controller map, in case there is a misconfiguration
            Debug.Log($"created adapter with map type: {adapter.VirtualController.mapTypeString}");
            
            Debug.Log("SF Symbols are available - using default runtime glyphs from OS");
            player.controllers.AddController(adapter.VirtualController, true);

            // Input remapping and the code for getting the SFSymbol name from a GCController element
            // wasn't added to iOS until version 14.0. So if were not iOS 14.0 yet, we won't be able
            // to query the controller element for it's symbol at runtime. In that case, we have to
            // use a fallback method
            if (GCControllerElement.SupportsSFSymbols() == false || ForceFallbackGlyphHelper)
            {
                // Because we can't query the controller element for it's SFSymbol name in iOS 13
                // we have to rely on a fallback map
                // this also means we need two different fallback maps one for PS4 and one for XBox
                
                _glyphHelper = new AppleControllerFallbackGlyphHelper(
                    adapter, 
                    _glyphProvider,
                    isPS4Controller ? dualShockFallbackMap : xBoxFallbackMap);
            }
            else
            {
                _glyphHelper = new AppleControllerGlyphHelper(adapter, _glyphProvider);
            }
        }
        
        // iOS 13 - use a fallback system because SF Symbols 2 are not available
        // This is here for demonstration purposes only, you'll want to handle this in your own way
        else
        {
            Debug.Log("SF Symbols are not available on the current system - using fallback glyph map");

            // If were using the fallback system, we need to know which of the supported extended gamepad
            // controllers were using in order to display the correct glyphs
            
            Debug.Log($"using PS4 glyphs? {(isPS4Controller ? "YES" : "NO")}");
            
            var glyphs = isPS4Controller ? ps4FallbackGlyphs : xboxFallbackGlyphs;
            _glyphHelper = new FallbackGlyphHelper(glyphs);
        }
        
        player.AddInputEventDelegate(OnButtonPressed, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed);
    }

    private void DebugLogControllerElements(Controller controller)
    {
        var elementNames = controller.Elements.Select(elem => elem.name).ToArray();
        var displayNames = controller.Elements.Select(elem => elem.elementIdentifier.name).ToArray();
        
        var sb = new StringBuilder();

        sb.AppendLine("controller elements:");
        for (var i = 0; i < elementNames.Count(); i++)
        {
            sb.AppendLine($"{i}  {elementNames[i]}:{displayNames[i]}");
        }

        Debug.Log(sb.ToString());
    }

    private void OnButtonPressed(InputActionEventData inputActionEventData)
    {
        Debug.Log($"btn pressed: {inputActionEventData.actionName}");
        
        if (_glyphHelper == null)
            return;
        
        // A single action can be bound to multiple buttons, so there could
        // be multiple appropriate glyphs
        var glyphs = _glyphHelper
            .GetAllGlyphsForRewiredAction(
                inputActionEventData.player, 
                inputActionEventData.actionId, 
                showFilled)
            .Where( g => g != null )
            .ToList();
        
        // We're going to pick one at random for the moment
        Sprite glyph = null;
        if (glyphs.Count > 0)
        {
            glyph = glyphs[Random.Range(0, glyphs.Count)];
            anImage.sprite = glyph;
        }

        // Just some debug stuff
        // Print the name of the glyph we want to display
        string glyphMsg;
        
        if (_glyphHelper is AppleControllerGlyphHelper gcGlyphHelper)
        {
            var elem = gcGlyphHelper.GetControllerElementForRewiredAction(
                inputActionEventData.player, inputActionEventData.actionId);

            glyphMsg = elem.SfSymbolsName ?? elem.UnmappedSfSymbolsName;
        }
        else
        {
            glyphMsg = glyph != null ? glyph.name : "";
        }
        
        statusTextMesh.text = $"action: '{inputActionEventData.actionName}' glyph: '{glyphMsg}'";
    }
}
