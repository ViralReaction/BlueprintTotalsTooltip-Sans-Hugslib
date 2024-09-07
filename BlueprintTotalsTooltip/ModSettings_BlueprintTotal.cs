using BlueprintTotalsTooltip.TotalsTipSettingsUtilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Verse;
using Verse.Sound;
using static Mono.Xml.MiniParser;
using Color = UnityEngine.Color;

namespace BlueprintTotalsTooltip
{
    public class ModSettings_BlueprintTotal : ModSettings
    {
        public override void ExposeData()
        {
            Scribe_Values.Look<int>(ref VisibilityMargin, "visibilityMargin", 100);
            Scribe_Values.Look<int>(ref TooltipClampMargin, "clampMargin", 10);
            Scribe_Values.Look<int>(ref TipXPosition, "TipXPosition", 8);
            Scribe_Values.Look<int>(ref TipYPosition, "TipYPosition", 2);
            Scribe_Values.Look<float>(ref TipYPositionCustomDrawerHeight, "TipYPositionCustomDrawerHeight", 8f);
            Scribe_Values.Look<float>(ref TipXPositionCustomDrawerHeight, "TipXPositionCustomDrawerHeight", 8f);
            Scribe_Values.Look<float>(ref HighlightOpacity, "HighlightOpacity", 0.10f);
            Scribe_Values.Look<bool>(ref TrackingVisible, "trackingVisible", true);
            Scribe_Values.Look<bool>(ref TrackingForbidden, "trackingForbidden", false);
            Scribe_Values.Look<bool>(ref ClampTipToScreen, "clampTipToScreen", false);
            Scribe_Values.Look<bool>(ref ShowRowToolTips, "showTips", true);
            Scribe_Values.Look<bool>(ref CountInStorage, "countInStorage", false);
            Scribe_Values.Look<bool>(ref CountForbidden, "countForbidden", false);
            Scribe_Values.Look<bool>(ref TransferSelection, "transferSel", true);
            Scribe_Values.Look<bool>(ref ShouldDrawTooltip, "ShouldDrawTooltip", false);
            Scribe_Values.Look(ref zoomForVisibleTracking, "ZoomForVisibleTracking", ZoomVisibleTrackingMode.Middle);
            base.ExposeData();
        }

        private Vector2 scrollPosition;

        public void DoSettingsWindowContents(Rect inRect)
        {
            Rect rect = new(inRect.x, inRect.y, inRect.width - 20f, inRect.height + 300);
            float contentHeight = 1000f;
            Widgets.BeginScrollView(inRect, ref this.scrollPosition, new Rect(0f, 0f, rect.width, contentHeight), true);
            Listing_BlueprintTotal options = new();
            options.Begin(rect);

            options.GapLine();
            Text.Font = GameFont.Medium;
            Text.Font = GameFont.Small;
            options.CheckboxLabeled("trackingVisible_title".Translate(), ref TrackingVisible, "trackingVisible_desc".Translate());
            options.CheckboxLabeled("trackingForbidden_title".Translate(), ref TrackingForbidden, "trackingForbidden_desc".Translate());
            options.Gap();
            var enumActions = new Dictionary<ZoomVisibleTrackingMode, Action>
            {
                { ZoomVisibleTrackingMode.VeryClose, () => zoomForVisibleTracking = ZoomVisibleTrackingMode.VeryClose },
                { ZoomVisibleTrackingMode.Close, () => zoomForVisibleTracking = ZoomVisibleTrackingMode.Close },
                { ZoomVisibleTrackingMode.Middle, () => zoomForVisibleTracking = ZoomVisibleTrackingMode.Middle },
                { ZoomVisibleTrackingMode.Far, () => zoomForVisibleTracking = ZoomVisibleTrackingMode.Far },
                { ZoomVisibleTrackingMode.VeryFar, () => zoomForVisibleTracking = ZoomVisibleTrackingMode.VeryFar }
            };
            options.CustomDropdownLabeledEnum("zoomForTracking_title".Translate(), ref zoomForVisibleTracking, enumActions, "zoomForTracking_desc".Translate(), 0f, 1f, 0.25f);
            options.CustomIntBoxWithButtons("visibilityMargin_title".Translate(), ref VisibilityMargin, 0, UI.screenHeight / 2, "visibilityMargin_desc".Translate(), 0f, 1f);
            options.Gap();
            options.CheckboxLabeled("clampTipToScreen_title".Translate(), ref ClampTipToScreen, "clampTipToScreen_desc".Translate());
            options.CustomIntBoxWithButtons("clampMargin_title".Translate(), ref TooltipClampMargin, 0, UI.screenHeight / 2, "clampMargin_desc".Translate(), 0f, 1f);
            options.Gap();
            HighlightOpacity = options.CustomSliderLabel("highlightOpacity_title".Translate(), HighlightOpacity, 0f, 0.25f, 0.5f, "highlightOpacity_desc".Translate(), HighlightOpacity.ToString("F2"), 1.ToString(), 0.ToString(), 0.01f);
            options.CheckboxLabeled("showTips_title".Translate(), ref ShowRowToolTips, "showTips_desc".Translate());
            options.CheckboxLabeled("countInStorage_title".Translate(), ref CountInStorage, "countInStorage_desc".Translate());
            options.CheckboxLabeled("countForbidden_title".Translate(), ref CountForbidden, "countForbidden_desc".Translate());
            options.Gap();
            options.Label("tooltipPosition_title".Translate());
            options.TooltipDrawer("tooltipPosition_title".Translate(), ref TipXPosition, ref TipYPosition, "tooltipPosition_desc".Translate(), 0, 1f);
            UIRoot_OnGUI_Patch.totalsTooltipDrawer.ResolveSettings();
            options.CheckboxLabeled("transferSel_title".Translate(), ref TransferSelection, "transferSel_desc".Translate());
            options.GapLine();
            options.Gap();

            if (options.ButtonText("Reset to Defaults"))
            {
                ResetSettingsToDefault();
            }
            options.End();
            Widgets.EndScrollView();
        }
       
        private const float scalingFactor = 0.75f;
        public void ResetSettingsToDefault()
        {
            VisibilityMargin = 100;
            TooltipClampMargin = 10;
            TipXPosition = 8;
            TipYPosition = 2;

            TrackingVisible = true;
            TrackingForbidden = false;
            ClampTipToScreen = false;
            ShowRowToolTips = true;
            CountInStorage = true;
            CountForbidden = false;
            TransferSelection = false;

            HighlightOpacity = 0.1f;
        }
        public static int
          VisibilityMargin = 100,
          TooltipClampMargin = 10,
          TipXPosition = 4,
          TipYPosition = 4;

        public static float
            HighlightOpacity = 0.10f,
            TipXPositionCustomDrawerHeight = 4f,
            TipYPositionCustomDrawerHeight = 4f;

        public static bool
            TrackingVisible = false,
            TrackingForbidden = false,
            ClampTipToScreen = false,
            ShowRowToolTips = true,
            CountInStorage = true,
            CountForbidden = false,
            TransferSelection = false,
            ShouldDrawTooltip = true;
        public void SettingsChanged()
        {
            //TotalsTipDrawer.ResolveSettings();
            BlueprintSelectionTransferer.transferring = TransferSelection;
        }

        public enum ZoomVisibleTrackingMode : byte
        {
            VeryClose,
            Close,
            Middle,
            Far,
            VeryFar
        }

        public static ZoomVisibleTrackingMode zoomForVisibleTracking = ZoomVisibleTrackingMode.Middle;

        public static ModSettings_BlueprintTotal Instance { get; private set; }

    }
}
