public struct SFSymbolInfo 
{ 
    public readonly string symbolName; 
    public readonly string filledSymbolName; 
    public readonly string altFilledSymbolName; 
 
    public bool hasFill => string.IsNullOrEmpty(filledSymbolName) == false; 
    public bool hasAltFill => string.IsNullOrEmpty(altFilledSymbolName) == false; 
     
    public SFSymbolInfo(string symbolName) 
    { 
        this.symbolName = symbolName; 
        filledSymbolName = null; 
        altFilledSymbolName = null; 
    } 
 
    public SFSymbolInfo(string symbolName, string filledSymbolName) 
    { 
        this.symbolName = symbolName; 
        this.filledSymbolName = filledSymbolName; 
        altFilledSymbolName = null; 
    } 
 
    public SFSymbolInfo(string symbolName, string filledSymbolName, string altFilledSymbolName) 
    { 
          this.symbolName = symbolName; 
          this.filledSymbolName = filledSymbolName; 
          this.altFilledSymbolName = altFilledSymbolName; 
    } 
} 