using ModSettings;

namespace ttrIndoorTemps
{
    internal class IndoorTempsSettings : JsonModSettings
    {
        [Section("Insulation ratios:")]
		
		[Name("In door")]
		[Description("In Door areas that have scene change (this will include transition caves).\n Recommended: 0.7-1")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float indoorRatio = 0.7f;
		
		[Name("Semi in-door")]
		[Description("Inside areas w/o scene change (shallow caves, look-out towers).\n Recommended: 0.3-0.5")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float semiRatio = 0.3f;
		
		[Name("Snow Shellter")]
		[Description("Recommended: 0.4-0.7")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float snowRatio = 0.4f;
		
		[Name("Cars")]
		[Description("Recommended: 0.1-0.3")]
		[Slider(0f, 1f, NumberFormat = "{0:F2}")]
		public float carRatio = 0.1f;

		[Section("Tunables:")]
        [Name("Upper boundaries ratio")]
        [Description("Ratio of max temp in calculations (insulation = 1). 0 is 0C, 1 is \"High of a day\" temperature.\nSetting this higher will result drastically colder temps. Recommended: 0.5")]
        [Slider(0f, 1f, NumberFormat = "{0:F2}")]
        public float tMaxRatio = 0.5f;


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