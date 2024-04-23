using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    /// <summary>
    /// Represents an identifier for a set.
    /// </summary>
    [Serializable]
    public class SetID
    {
        private SetId _value;

        [JsonProperty("set_id")]
        public SetId Value { get => _value; set => _value = value; }









        /// <summary>
        /// Enum representing possible set identifiers.
        /// </summary>
        public enum SetId
        {
            Top,
            Middle,
            Bottom
        }

        /// <summary>
        /// Dictionary to map string representations to SetId enum values.
        /// </summary>
        private static readonly Dictionary<string, SetId> setIdMap = new Dictionary<string, SetId>(StringComparer.OrdinalIgnoreCase)
        {
            { "T", SetId.Top },
            { "M", SetId.Middle },
            { "B", SetId.Bottom }
        };

        /// <summary>
        /// Converts a string representation to the corresponding SetId enum value.
        /// </summary>
        /// <param name="input">The string representation of the set identifier.</param>
        /// <returns>The SetId enum value corresponding to the input string.</returns>
        public static SetId ConvertStringToSetId(string input)
        {
            if (setIdMap.TryGetValue(input, out SetId setId))
            {
                return setId;
            }
            return SetId.Top; // Default value if input is not found
        }

        /// <summary>
        /// Overloads the == operator to compare two SetID objects.
        /// </summary>
        public static bool operator ==(SetID a, SetID b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            {
                return false;
            }

            // Return true if the fields match:
            return a._value == b._value;
        }

        /// <summary>
        /// Overloads the != operator to compare two SetID objects.
        /// </summary>
        public static bool operator !=(SetID a, SetID b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Overrides the Equals method to compare two SetID objects.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = obj as SetID;
            return other != null && this._value == other._value;
        }

        /// <summary>
        /// Overrides the GetHashCode method to compute the hash code of the SetID object.
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
