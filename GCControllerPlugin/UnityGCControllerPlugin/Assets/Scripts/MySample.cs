using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using HovelHouse.GameController;
using TMPro;
using Rewired;

public class MySample : MonoBehaviour
{
    public UnityEngine.UI.Image anImage;
    public TMP_Text statusTextMesh;

    public float symbolPointSize;
    public UIImageSymbolWeight symbolWeight;
    public bool loadFilledSymbolVariants;
    
    public ControllerElementToSpriteMap xboxFallbackGlyphs;
    public ControllerElementToSpriteMap ps4FallbackGlyphs;
    
    private GCController controller;
    private GCPhysicalInputProfile profile;

    private SFSymbolSet _symbolSet;
    private IGlyphHelper _glyphHelper;
    
    void Awake()
    {
        // Load symbols early - it's an expensive process
        _symbolSet = new SFSymbolSet();
        _symbolSet = new SFSymbolSet(symbolPointSize, symbolWeight);
        _symbolSet.LoadAllControllerGlyphs(loadFilledSymbolVariants);
        
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
        
        // Determine which glyph helper were gonna use
        if (UIImage.SFSymbolsAreAvailable())
        {
            var appleController = GCController.Controllers().FirstOrDefault();
            
            Debug.Log($"got apple controller: {appleController.VendorName}");
        
            // Use the extended gamepad property to skip past physicalInputProfile
            // which was only added in macOS 11.0+
            var extendedGamepad = appleController.ExtendedGamepad;
            var siriRemote = appleController.MicroGamepad;

            Debug.Log("extended gamepad: " + extendedGamepad);
            Debug.Log("siriRemote: " + siriRemote);
            
            IRewiredAppleControllerAdapter adapter = null;
            
            if (extendedGamepad != null)
            {
                adapter = new RewiredExtendedGamepadAdapter(appleController, 0);
            }
            else if (siriRemote != null)
            {
                adapter = new RewiredSiriRemoteAdapter(appleController, 1);
            }
            
            // Debug log the controller map, in case there is a misconfiguration
            Debug.Log($"created adapter with map type: {adapter.VirtualController.mapTypeString}");
            
            Debug.Log("SF Symbols are available - using default runtime glyphs from OS");
            player.controllers.AddController(adapter.VirtualController, true);
            _glyphHelper = new AppleControllerGlyphHelper(adapter, _symbolSet);
        }
        else
        {
            Debug.Log("SF Symbols are not available on the current system - using fallback glyph map");
            
            var isPS4Controller = CultureInfo.InvariantCulture.CompareInfo.IndexOf(controller.name, "dualshock",
                                      CompareOptions.OrdinalIgnoreCase) >= 0;

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
            .GetAllGlyphsForRewiredAction(inputActionEventData.player, inputActionEventData.actionId)
            .ToList();
        
        // We're going to pick one at random for the moment
        Sprite glyph = null;
        if (glyphs.Count > 0)
        {
            glyph = glyphs[Random.Range(0, glyphs.Count)];
            anImage.sprite = glyph;
        }
        else
        {
            anImage.sprite = null;
        }

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
