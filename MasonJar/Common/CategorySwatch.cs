// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System;
using System.Drawing;

namespace MasonJar.Common
{
    public enum CategorySwatch
    {
        [SwatchColorR(255), SwatchColorG(79),  SwatchColorB(63)]  Color1,  // Red
        [SwatchColorR(58),  SwatchColorG(94),  SwatchColorB(255)] Color2,  // Blue
        [SwatchColorR(58),  SwatchColorG(150), SwatchColorB(63)]  Color3,  // Green
        [SwatchColorR(171), SwatchColorG(53),  SwatchColorB(255)] Color4,  // Purple
        [SwatchColorR(255), SwatchColorG(153), SwatchColorB(239)] Color5,  // Pink
        [SwatchColorR(255), SwatchColorG(165), SwatchColorB(56)]  Color6,  // Orange
        [SwatchColorR(249), SwatchColorG(241), SwatchColorB(7)]   Color7,  // Yellow
        [SwatchColorR(158), SwatchColorG(158), SwatchColorB(158)] Color8,  // Grey
        [SwatchColorR(124), SwatchColorG(93),  SwatchColorB(18)]  Color9,  // Brown
        [SwatchColorR(16),  SwatchColorG(18),  SwatchColorB(124)] Color10, // Dark Blue
        [SwatchColorR(104), SwatchColorG(244), SwatchColorB(255)] Color11, // Light Blue
        [SwatchColorR(169), SwatchColorG(130), SwatchColorB(255)] Color12, // Light Purple
        [SwatchColorR(136), SwatchColorG(224), SwatchColorB(85)]  Color13, // Light Green
        [SwatchColorR(224), SwatchColorG(136), SwatchColorB(112)] Color14, // Light Red
        [SwatchColorR(0),   SwatchColorG(0),   SwatchColorB(0)]   Color15, // Black
        [SwatchColorR(255), SwatchColorG(255), SwatchColorB(255)] Color16, // White
    }

    public static class CategorySwatchExtensionMethods
    {
        public static Color GetColor(this CategorySwatch cs)
        {
            return Color.FromArgb(255,
                                  EnumAttributes.GetAttributeValue<SwatchColorR, byte>(cs),
                                  EnumAttributes.GetAttributeValue<SwatchColorG, byte>(cs),
                                  EnumAttributes.GetAttributeValue<SwatchColorB, byte>(cs));
        }

        public static CategorySwatch GetSwatchFromIndex(int index)
        {
            return (CategorySwatch)Enum.GetValues(typeof(CategorySwatch)).GetValue(index);
        }
    }
}