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
        public const string Version = "0.3.1"; // Version of the Mod.  (MUST BE SET)
        public const string DownloadLink = null; // Download Link for the Mod.  (Set as null if none)
    }
    internal class ttrIndoorTemps : MelonMod
    {
        internal static dynamic? ETDCommon = null;

        internal static void LinkDeps()
        {
            Assembly? ETD;
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
                Type? ETDType;
                ETDType = ETD.GetType("ExtremeTempDrop.Common");
                //Type ETDType = Type.GetType("ExtremeTempDrop.Common, ExtremeTempDrop");
                //MelonLogger.Msg("temp (extreme1): " + ETDType);

                if (ETDType != null)
                {
                    ETDCommon = Activator.CreateInstance(ETDType);
                    MelonLogger.Msg("Linked to ExtremeTempDrop");
                }
            }

        }
        public override void OnInitializeMelon()
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
            float tmax = Mathf.Lerp(0, __instance.m_TempHigh, Settings.options.tMaxRatio);
            if (!InterfaceManager.IsMainMenuEnabled())
            {
               if (__instance.IsIndoorEnvironment())
                {

                    if (ttrIndoorTemps.ETDCommon != null)
                    {
                        baseTemp -= (float)ttrIndoorTemps.ETDCommon.GetTempDropC();
                    }
                    if (!GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger || !GameManager.GetPlayerManagerComponent().m_IndoorSpaceTrigger.m_UseOutdoorTemperature)
                    {
                        __instance.m_IndoorTemperatureCelsius = Mathf.Lerp(baseTemp, tmax, Settings.options.indoorRatio);
                    }
                }
            }
            //MelonLogger.Msg("temp: b:" + __instance.m_BaseTemperature + " c:" + __instance.m_CurrentTemperature + " i:" + __instance.m_IndoorTemperatureCelsius + " h:" + __instance.m_TempHigh + " (" + tmax + ") l:" + __instance.m_TempLow + " " + baseTemp);
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

                float tbase = __instance.m_BaseTemperature;
                float tmax = Mathf.Lerp(0, __instance.m_TempHigh, Settings.options.tMaxRatio);
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
