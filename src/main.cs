global using Il2Cpp;

using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace ttrIndoorTemps
{
    public static class BuildInfo
    {
        public const string Name = "IndoorTemps"; // Name of the Mod.  (MUST BE SET)
        public const string Description = "In doors temps are dynamic."; // Description for the Mod.  (Set as null if none)
        public const string Author = "ttr"; // Author of the Mod.  (MUST BE SET)
        public const string Company = null; // Company that made the Mod.  (Set as null if none)
        public const string Version = "0.2.0"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }
    internal class ttrIndoorTemps : MelonMod
	{
        internal static dynamic ETDCommon = null;

        internal static void LinkDeps()
        {
            Assembly ETD;
            try
            {
                ETD = Assembly.Load("ExtremeTempDrop");
            }
            catch
            {
                ETD = null;
            }
            if (ETD != null)
            {
                Type ETDType = ETD.GetType("ExtremeTempDrop.Common");
                //Type ETDType = Type.GetType("ExtremeTempDrop.Common, ExtremeTempDrop");
                //MelonLogger.Msg("temp (extreme1): " + ETDType);

                if (ETDType != null)
                {
                    ETDCommon = Activator.CreateInstance(ETDType);
                }
            }

        }
        public override void OnApplicationStart()
		{
			Debug.Log($"[{Info.Name}] Version {Info.Version} loaded!");
			Settings.OnLoad();
            LinkDeps();
		}
	}
    
    [HarmonyPatch(typeof(Weather), "Update")]
    internal class Weather_Update
    {
        public static void Prefix(Weather __instance)
        {
            if (GameManager.m_IsPaused)
            {
                return;
            }
            float baseTemp = __instance.m_BaseTemperature;
            if (!InterfaceManager.IsMainMenuEnabled())
            {
               if (__instance.IsIndoorEnvironment())
                {

                    if (ttrIndoorTemps.ETDCommon != null)
                    {
                        baseTemp = (float)ttrIndoorTemps.ETDCommon.GetTempDropC() * -1;
                        //MelonLogger.Msg("temp (extreme): " + baseTemp);
                    }
                    if (!GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger || !GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger.m_UseOutdoorTemperature)
                    {
                        __instance.m_IndoorTemperatureCelsius = Mathf.Lerp(baseTemp, __instance.m_TempHigh, Settings.options.indoorRatio);
                    }
                }
            }
            //MelonLogger.Msg("temp: " + __instance.m_CurrentTemperature + " " + __instance.m_IndoorTemperatureCelsius + " h:" + __instance.m_TempHigh + " l:" + __instance.m_TempLow + " " + baseTemp);
        }
    }
    [HarmonyPatch(typeof(Weather), "CalculateCurrentTemperature")]
    internal class Weather_CalculateCurrentTemperature
    {
        public static void Postfix(Weather __instance)
        {
            if (GameManager.m_IsPaused)
            {
                return;
            }
            if (!InterfaceManager.IsMainMenuEnabled())
            {
                /*
                float shelter = GameManager.GetSnowShelterManager().GetTemperatureIncreaseCelsius(); // +15
                float indoortrig = 0;
                if (GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger)
                {
                    indoortrig = GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger.m_TemperatureDeltaCelsius; // +8
                }

                float car = 0f; // GameManager.GetPlayerInVehicle().GetTempIncrease(); // +5

                float num6 = GameManager.GetTimeOfDayComponent().GetHoursPlayedNotPaused() / 24f;
                num6 -= GameManager.GetExperienceModeManagerComponent().GetOutdoorTempDropCelcius(num6);
                float calc = (__instance.m_BaseTemperature + __instance.m_TempHigh) / 2;
                float calc2 = Mathf.Lerp(__instance.m_BaseTemperature, __instance.m_TempHigh, 0.7f);
                MelonLogger.Msg("temp: " + __instance.m_CurrentTemperature + " " + __instance.m_IndoorTemperatureCelsius + " " + shelter + " indoor:" + indoortrig + " " + num6 + " " + car + " h:" + __instance.m_TempHigh + " l:" + __instance.m_TempLow + " " + __instance.m_BaseTemperature + " c:" + calc + " | " + calc2);
                */

                float tbase = __instance.m_BaseTemperature;
                float tmax = __instance.m_TempHigh;
                float tdiff = 0f;
                if (GameManager.GetSnowShelterManager().PlayerInNonRuinedShelter())
                {
                    // vanilla +15
                    tdiff = Mathf.Lerp(tbase, tmax, Settings.options.snowRatio);
                    tdiff = Mathf.Abs(tdiff - tbase);
                    __instance.m_CurrentTemperature -= GameManager.GetSnowShelterManager().GetTemperatureIncreaseCelsius();
                    __instance.m_CurrentTemperature += Math.Min(tdiff, GameManager.GetSnowShelterManager().GetTemperatureIncreaseCelsius());
                }
                if (GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger)
                {
                    // +8
                    tdiff = Mathf.Lerp(tbase, tmax, Settings.options.semiRatio);
                    tdiff = Mathf.Abs(tdiff - tbase);
                    __instance.m_CurrentTemperature -= GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger.m_TemperatureDeltaCelsius;
                    __instance.m_CurrentTemperature += Math.Min(tdiff, GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger.m_TemperatureDeltaCelsius);
                }
                if (GameManager.GetPlayerInVehicle().IsInside())
                {
                    // +5
                    tdiff = Mathf.Lerp(tbase, tmax, Settings.options.carRatio);
                    tdiff = Mathf.Abs(tdiff - tbase);
                    __instance.m_CurrentTemperature -= GameManager.GetPlayerInVehicle().GetTempIncrease();
                    __instance.m_CurrentTemperature += Math.Min(tdiff, GameManager.GetPlayerInVehicle().GetTempIncrease());
                }
                //MelonLogger.Msg("tdiff: " + tdiff);                    
            }
        }
    }

}