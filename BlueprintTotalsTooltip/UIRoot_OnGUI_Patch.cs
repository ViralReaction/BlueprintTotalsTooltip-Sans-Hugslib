using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace BlueprintTotalsTooltip
{
    [HarmonyPatch(typeof(UIRoot))]
    [HarmonyPatch("UIRootOnGUI")]
    public static class UIRoot_OnGUI_Patch
    {
        public static TotalsTooltipDrawer totalsTooltipDrawer;
        static UIRoot_OnGUI_Patch()
        {
            Mod_BlueprintTotal mod = LoadedModManager.GetMod<Mod_BlueprintTotal>();
            if (mod != null)
            {
                totalsTooltipDrawer = new TotalsTooltipDrawer(mod);
                totalsTooltipDrawer.ResolveSettings();
            }
            else
            {
                Log.Error("TotalsTooltipDrawer instance not found! Cannot initialize TotalsTooltipDrawer.");
            }
        }

        [HarmonyPostfix]
        public static void OnGUIHook()
        {
            totalsTooltipDrawer.OnGUI();
        }
    }
}
