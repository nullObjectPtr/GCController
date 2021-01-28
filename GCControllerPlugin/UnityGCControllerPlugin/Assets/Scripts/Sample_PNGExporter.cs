using System.Collections.Generic;
using HovelHouse.GameController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sample_PNGExporter : MonoBehaviour
{
    public TMP_InputField exportDirectoryTextMesh;
    public TMP_Text sizeValueTextMesh;
    public Slider sizeSlider;
    public TMP_Dropdown weightDropdown;
    public Button exportButton;
    public float pointSize;
    public UIImageSymbolWeight SymbolWeight;

    
    // Start is called before the first frame update
    void Start()
    {
        exportDirectoryTextMesh.text = Application.persistentDataPath;
        exportButton.onClick.AddListener(OnExport);
        sizeSlider.onValueChanged.AddListener(OnPointSizeChanged);
        OnPointSizeChanged(sizeSlider.value);
        
        weightDropdown.options = new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("All Weights"),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Black.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Bold.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Heavy.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Light.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Medium.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Regular.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Semibold.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.Unspecified.ToString()),
            new TMP_Dropdown.OptionData(UIImageSymbolWeight.UltraLight.ToString()),
        };

        Debug.Log("are sf symbols available? " + UIImage.SFSymbolsAreAvailable());
    }

    private void OnPointSizeChanged(float value)
    {
        pointSize = value;
        sizeValueTextMesh.text = $"point size: {pointSize.ToString()}";
    }

    private void OnExport()
    {
        var filepath = exportDirectoryTextMesh.text;
        var names = SymbolInfoCollection.AllControllerSymbols.AllSymbolNames();
        
        // Export all symbol weights;
        if (weightDropdown.value == 0)
        {
            SFSymbolExporter.ExportPNGS(
                names,
                pointSize,
                filepath);
        }
        
        // Export a single symbol weight
        else
        {
            var dropdownValue = weightDropdown.options[weightDropdown.value].text;
            if (UIImageSymbolWeight.TryParse(dropdownValue, out UIImageSymbolWeight weight))
            {
                SFSymbolExporter.ExportPNGS(
                    names, pointSize, weight, false, filepath);
            }
        }
    }
}
