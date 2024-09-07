using UnityEngine;
using Verse;
using System;

namespace BlueprintTotalsTooltip.TotalsTipSettingsUtilities
{
	enum RectDimensionPosition : int
	{
		LowerWithLowerOffset = 0,
		LowerCenter = 1,
		LowerWithHigherOffset = 2,
		CenterWithLowerOffset = 3,
		Center = 4,
		CenterWithHigherOffset = 5,
		HigherWithLowerOffset = 6,
		HigherCenter = 7,
		HigherWithHigherOffset = 8
	}

	static class TipPosSettingsHandler
	{
		
		public static void DrawTipPosSetting(Rect rect, int xHandler, int yHandler)
		{
			settingsChanged = false;
            ModSettings_BlueprintTotal.TipXPositionCustomDrawerHeight = rect.width;
			Rect drawingRect = rect.ContractedBy(marginSize);
            int[] valuesFromSliders = DrawTwoAxisIntSliders(drawingRect, 8f, new int[] { xHandler, yHandler });
			if (valuesFromSliders[0] != xHandler || valuesFromSliders[1] != yHandler)
			{
				xHandler = valuesFromSliders[0];
				yHandler = valuesFromSliders[1];
				settingsChanged = true;
			}
			DoToolTip(rect);
			DrawHelperGraphics(drawingRect, xHandler, yHandler, 16f);
		}

		private static void DoToolTip(Rect settingsRect)
		{
			Rect tooltipRect = new Rect(settingsRect.x - settingsRect.width, settingsRect.y, settingsRect.width, settingsRect.height);
			TooltipHandler.TipRegion(tooltipRect, new TipSignal("tooltipPosition_desc".Translate()));
		}

		private static int[] DrawTwoAxisIntSliders(Rect rect, float sliderWidth, int[] values)
		{
            GUI.BeginGroup(rect);
            Rect horizontalRect = new Rect(sliderWidth + 20f, 0f, rect.width - sliderWidth, sliderWidth);
            Rect verticalRect = new Rect(5f, sliderWidth + 20f, sliderWidth, rect.height - sliderWidth);
            ModSettings_BlueprintTotal.TipXPosition = (int)Widgets.HorizontalSlider(horizontalRect, values[0], 0, 8, true, null, null, null, 1);
            ModSettings_BlueprintTotal.TipYPosition = (int)ToolTipSettingsUtility.LabelLessVerticalSlider(verticalRect, values[1], 0, 8);
			GUI.EndGroup();
			return new int[] { ModSettings_BlueprintTotal.TipXPosition, ModSettings_BlueprintTotal.TipYPosition  };
		}

        private static void DrawHelperGraphics(Rect rect, int xHandler, int yHandler, float sliderWidth)
        {
            Rect rect2 = new Rect(rect.x + sliderWidth + 20f, rect.y + sliderWidth + 20f, rect.width - sliderWidth, rect.height - sliderWidth).ContractedBy(16f * TipPosSettingsHandler.marginSize);
            Widgets.DrawHighlight(rect2);
            float num = 12f * TipPosSettingsHandler.marginSize;
            float dimensionFromSetting = TipPosSettingsHandler.GetDimensionFromSetting(rect2.xMin, rect2.xMax, num, (RectDimensionPosition)xHandler);
            float dimensionFromSetting2 = TipPosSettingsHandler.GetDimensionFromSetting(rect2.yMin, rect2.yMax, num, (RectDimensionPosition)yHandler);
            Widgets.DrawBox(new Rect(dimensionFromSetting, dimensionFromSetting2, num, num), 1, null);
            TipPosSettingsHandler.DrawCenterlines(rect2.center.x, rect2.center.y, num + rect2.width / 2f);
        }

        public static float GetDimensionFromSetting(float lower, float upper, float dimWidth, RectDimensionPosition setting)
		{
			switch (setting)
			{
				case RectDimensionPosition.LowerWithLowerOffset:
					return lower - dimWidth;
				case RectDimensionPosition.LowerCenter:
					return lower - dimWidth / 2;
				case RectDimensionPosition.LowerWithHigherOffset:
					return lower;
				case RectDimensionPosition.CenterWithLowerOffset:
					return 0.5f * (lower + upper) - dimWidth;
				case RectDimensionPosition.Center:
					return 0.5f * (lower + upper) - dimWidth / 2;
				case RectDimensionPosition.CenterWithHigherOffset:
					return 0.5f * (lower + upper);
				case RectDimensionPosition.HigherWithLowerOffset:
					return upper - dimWidth;
				case RectDimensionPosition.HigherCenter:
					return upper - dimWidth / 2;
				case RectDimensionPosition.HigherWithHigherOffset:
					return upper;
			}
			return 0;
		}

        private static void DrawCenterlines(float centerX, float centerY, float length)
        {
            Widgets.DrawLineHorizontal(centerX - length, centerY, 2f * length);
            Widgets.DrawLineVertical(centerX, centerY - length, 2f * length);
        }
        private static readonly float marginSize = 3f;

        public static bool settingsChanged = false;

       
    }
}
