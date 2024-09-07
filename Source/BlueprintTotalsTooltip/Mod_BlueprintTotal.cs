using HarmonyLib;
using UnityEngine;
using Verse;
using BlueprintTotalsTooltip.TotalsTipSettingsUtilities;

namespace BlueprintTotalsTooltip
{
    public class Mod_BlueprintTotal : Mod
    {
        public static ModSettings_BlueprintTotal settings;
        public static Mod_BlueprintTotal Instance;
        public Mod_BlueprintTotal(ModContentPack content) : base(content)
        {
            settings = GetSettings<ModSettings_BlueprintTotal>();
            Harmony harmony = new(this.Content.PackageIdPlayerFacing);
            harmony.PatchAll();
            LongEventHandler.ExecuteWhenFinished(() =>
            {
                TotalsTooltipDrawer.ResolveReferences();
            });

        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoSettingsWindowContents(inRect);
        }
        public override string SettingsCategory()
        {
            return "Blueprint Total Tooltip";
        }
        public override void WriteSettings()
        {
            base.WriteSettings();
        }
        private TotalsTooltipDrawer TotalsTipDrawer { get; }

    }
}
