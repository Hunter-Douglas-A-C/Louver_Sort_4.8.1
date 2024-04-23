using System;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents different styles of louvers.
    /// </summary>
    [Serializable]
    public class LouverStyle
    {
        /// <summary>
        /// Enum representing the available louver styles.
        /// </summary>
        public enum LouverStyles
        {
            XL,        // Extra Large
            Standard   // Standard
        }

        /// <summary>
        /// Converts a boolean value to a LouverStyles enum.
        /// </summary>
        /// <param name="isXL">A boolean indicating if the louver style is XL.</param>
        /// <returns>The corresponding LouverStyles enum value.</returns>
        public static LouverStyles ConvertToStyle(bool isXL)
        {
            return isXL ? LouverStyles.XL : LouverStyles.Standard;
        }
    }
}
