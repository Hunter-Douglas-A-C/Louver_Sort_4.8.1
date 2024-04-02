using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    internal class SetID
    {
        public SetId Value { get; set; } // Property, not a method

        public enum SetId
        {
            Top,
            Middle,
            Bottom
        }

        public static SetId ConvertStringToSetId(string input)
        {
            switch (input.ToUpper()) // Use ToUpper to make the comparison case-insensitive
            {
                case "T":
                    return SetId.Top;
                case "M":
                    return SetId.Middle;
                case "B":
                    return SetId.Bottom;
                default:
                    throw new ArgumentException("Invalid input for SetId conversion.");
            }
        }

        // Overloading the == operator
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
            return a.Value == b.Value;
        }

        // Overloading the != operator
        public static bool operator !=(SetID a, SetID b)
        {
            return !(a == b);
        }

        // Override the Equals method
        public override bool Equals(object obj)
        {
            var other = obj as SetID;
            return other != null && this.Value == other.Value;
        }

        // Override the GetHashCode method
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
