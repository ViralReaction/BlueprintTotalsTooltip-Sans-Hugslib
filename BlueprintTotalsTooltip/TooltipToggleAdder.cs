using HarmonyLib;
using RimWorld;
using Verse;
using Verse.Sound;
using UnityEngine;
using BlueprintTotalsTooltip.CameraChangeDetection;
using System.Collections.Generic;
using System;

namespace BlueprintTotalsTooltip
{
	[HarmonyPatch(typeof(PlaySettings))]
	[HarmonyPatch("DoPlaySettingsGlobalControls")]
	class TooltipToggleAdder
	{
		private static List<Action> methodsToNotifyOnToggle = new List<Action>();

		public static void RegisterMethod(Action method) => methodsToNotifyOnToggle.Add(method);

		public static void DeregisterMethod(Action method)
		{
			if (methodsToNotifyOnToggle.Contains(method))
				methodsToNotifyOnToggle.Remove(method);
		}
        public bool DrawTooltip => ModSettings_BlueprintTotal.ShouldDrawTooltip;

        static void Postfix(WidgetRow row, bool worldView)
        {
            if (worldView || row == null) return;

            bool previous = ModSettings_BlueprintTotal.ShouldDrawTooltip;

            row.ToggleableIcon(ref ModSettings_BlueprintTotal.ShouldDrawTooltip,
                AssetLoader.totalsTooltipToggleTexture,
                "ShowTotalsTooltipTip".Translate(),
                SoundDefOf.Mouseover_ButtonToggle);
            if (previous != ModSettings_BlueprintTotal.ShouldDrawTooltip)
            {
                NotifyPlaySettingToggled();
            }

        }
        public static void NotifyPlaySettingToggled()
        {
            foreach (Action method in methodsToNotifyOnToggle)
                method();
        }

        // IN MEMORIAM: My first working transpiler patch previously written in PlaySettingChangeDetector.cs
        // Superseded by additional code in this class. ~9/8/2018 to 4/1/2019
    }
}
