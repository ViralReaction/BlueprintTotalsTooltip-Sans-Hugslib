using Verse;
using RimWorld;

namespace BlueprintTotalsTooltip
{
    [DefOf]
    public static class BluePrintTotalDefOf
    {
        public static KeyBindingDef ToggleTracking;

        static BluePrintTotalDefOf() => DefOfHelper.EnsureInitializedInCtor(typeof(BluePrintTotalDefOf));
    }

}
