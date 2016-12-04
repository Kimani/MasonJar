// [Ready Design Corps] - [Mason Jar] - Copyright 2016

using System.Drawing;

namespace MasonJar.Common
{
    public enum CategorySwatch
    {
        [SwatchColorR(0),   SwatchColorG(0),   SwatchColorB(0)]   Color1,  // Black
        [SwatchColorR(255), SwatchColorG(255), SwatchColorB(255)] Color2,  // White
        [SwatchColorR(255), SwatchColorG(79),  SwatchColorB(63)]  Color3,  // Red
        [SwatchColorR(58),  SwatchColorG(94),  SwatchColorB(255)] Color4,  // Blue
        [SwatchColorR(58),  SwatchColorG(150), SwatchColorB(63)]  Color5,  // Green
        [SwatchColorR(171), SwatchColorG(53),  SwatchColorB(255)] Color6,  // Purple
        [SwatchColorR(255), SwatchColorG(153), SwatchColorB(239)] Color7,  // Pink
        [SwatchColorR(255), SwatchColorG(165), SwatchColorB(56)]  Color8,  // Orange
        [SwatchColorR(249), SwatchColorG(241), SwatchColorB(7)]   Color9,  // Yellow
        [SwatchColorR(158), SwatchColorG(158), SwatchColorB(158)] Color10, // Grey
        [SwatchColorR(124), SwatchColorG(93),  SwatchColorB(18)]  Color11, // Brown
        [SwatchColorR(16),  SwatchColorG(18),  SwatchColorB(124)] Color12, // Dark Blue
        [SwatchColorR(104), SwatchColorG(244), SwatchColorB(255)] Color13, // Light Blue
        [SwatchColorR(169), SwatchColorG(130), SwatchColorB(255)] Color14, // Light Purple
        [SwatchColorR(136), SwatchColorG(224), SwatchColorB(85)]  Color15, // Light Green
        [SwatchColorR(224), SwatchColorG(136), SwatchColorB(112)] Color16, // Light Red
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
    }
}