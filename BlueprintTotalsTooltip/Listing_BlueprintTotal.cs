﻿using BlueprintTotalsTooltip.TotalsTipSettingsUtilities;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace BlueprintTotalsTooltip
{
    [StaticConstructorOnStartup]
    public class Listing_BlueprintTotal : Listing_Standard
    {
        public new float verticalSpacing = 5f;
        public void CustomIntBoxWithButtons(string label, ref int intValue, int minValue, int maxValue, string tooltip = null, float height = 0f, float labelPct = 1f)
        {
            float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
            Rect rect = base.GetRect(height2, labelPct);
            rect.width = Math.Min(rect.width + 24f, base.ColumnWidth);
            if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
            {
                if (!tooltip.NullOrEmpty())
                {
                    if (Mouse.IsOver(rect))
                    {
                        Widgets.DrawHighlight(rect);
                    }
                    TooltipHandler.TipRegion(rect, tooltip);
                }
                CustomWidgets.DrawIntOptionWithButtons(rect, ref intValue, label, minValue, maxValue);
            }
            base.Gap(this.verticalSpacing);
        }
        public void TooltipDrawer(string label, ref int intValue, ref int minValue, string tooltip = null, float height = 0f, float labelPct = 1f)
        {
            float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
            Rect rect = base.GetRect(height2, labelPct);
            rect.width = Math.Min(rect.width + 24f, base.ColumnWidth);
            if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
            {
                if (!tooltip.NullOrEmpty())
                {
                    if (Mouse.IsOver(rect))
                    {
                        Widgets.DrawHighlight(rect);
                    }
                    TooltipHandler.TipRegion(rect, tooltip);
                }
                Rect sliderRect = new Rect(rect);
                sliderRect.height = 300f;
                sliderRect.width = 300f;
                TipPosSettingsHandler.DrawTipPosSetting(sliderRect, intValue, minValue);
            }
            base.Gap(this.verticalSpacing + 300f);
        }
        public void CustomDropdownLabeledEnum<T>(string label, ref T selectedValue, Dictionary<T, Action> enumActions, string tooltip = null, float height = 0f, float labelPct = 1f, float dropdownWidthFactor = 0.75f) where T : Enum
        {
            float height2 = (height != 0f) ? height : Text.CalcHeight(label, base.ColumnWidth * labelPct);
            Rect rect = base.GetRect(height2, labelPct);
            float labelWidth = rect.width * 0.4f;
            float dropdownWidth = rect.width * dropdownWidthFactor;
            float rightPadding = 10f;
            Rect dropdownRect = new Rect(rect.xMax - dropdownWidth - rightPadding, rect.y, dropdownWidth, rect.height);
            Rect labelRect = new Rect(rect.x, rect.y, rect.width - dropdownWidth - rightPadding, rect.height);
            if (this.BoundingRectCached == null || rect.Overlaps(this.BoundingRectCached.Value))
            {
                if (!tooltip.NullOrEmpty())
                {
                    if (Mouse.IsOver(dropdownRect))
                    {
                        Widgets.DrawHighlight(dropdownRect);
                    }
                    TooltipHandler.TipRegion(dropdownRect, tooltip);
                }
                TextAnchor originalAnchor = Text.Anchor;
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(labelRect, label);
                Text.Anchor = originalAnchor;
                string translatedSelectedValue = ("zoomForTracking_" + selectedValue.ToString()).Translate();
                if (Widgets.ButtonText(dropdownRect, translatedSelectedValue))
                {
                    List<FloatMenuOption> options = new List<FloatMenuOption>();

                    foreach (T enumValue in Enum.GetValues(typeof(T)))
                    {
                        options.Add(new FloatMenuOption(enumValue.ToString(), () =>
                        {
                            if (enumActions != null && enumActions.ContainsKey(enumValue))
                            {
                                enumActions[enumValue]?.Invoke();
                            }
                            else
                            {
                                Log.Warning("No action defined for: " + enumValue.ToString());
                            }
                        }));
                    }
                    Find.WindowStack.Add(new FloatMenu(options));
                }
            }
            base.Gap(this.verticalSpacing);
        }
        public float CustomSliderLabel(string label, float val, float min, float max, float labelPct = 0.5f, string tooltip = null, string label2 = null, string rightLabel = null, string leftLabel = null, float roundTo = -1f)
        {
            Rect rect = base.GetRect(30f, 1f);
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect.LeftPart(labelPct), label);
            if (tooltip != null)
            {
                TooltipHandler.TipRegion(rect.LeftPart(labelPct), tooltip);
            }
            Text.Anchor = TextAnchor.UpperLeft;
            float result = CustomWidgets.HorizontalSlider(rect.RightPart(1f - labelPct), val, min, max, true, label2, leftLabel, rightLabel, roundTo);
            base.Gap(this.verticalSpacing);
            return result;
        }

        private Rect ClampRectToScreen(Rect rect)
        {
            float screenWidth = UI.screenWidth;
            float screenHeight = UI.screenHeight;

            if (rect.xMax > screenWidth)
            {
                rect.x -= (rect.xMax - screenWidth);
            }
            if (rect.x < 0)
            {
                rect.x = 0;
            }

            if (rect.yMax > screenHeight)
            {
                rect.y -= (rect.yMax - screenHeight);
            }
            if (rect.y < 0)
            {
                rect.y = 0;
            }

            return rect;
        }


    }
}