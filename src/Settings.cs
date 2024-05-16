using ModSettings;

namespace ttrIndoorTemps
{
    internal class IndoorTempsSettings : JsonModSettings
    {
        [Section("Insulation ratios:")]
		
		[Name("In door")]
		[Description("In Door areas that havew scene chnage (this will include transition caves).\n Recommended: 0.7")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float indoorRatio = 0.7f;
		
		[Name("Semi in-door")]
		[Description("Inside areas w/o scane change (shallow caves, look-out towers).\n Recommended: 0.25")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float semiRatio = 0.25f;
		
		[Name("Snow Shellter")]
		[Description("Recommended: 0.4")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float snowRatio = 0.4f;
		
		[Name("Cars")]
		[Description("Recommended: 0.1")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float carRatio = 0.1f;
		
        
    }
    internal static class Settings
    {
        public static IndoorTempsSettings options;
        public static void OnLoad()
        {
            options = new IndoorTempsSettings();
            options.AddToModSettings("In door temps");
        }
    }
    
}