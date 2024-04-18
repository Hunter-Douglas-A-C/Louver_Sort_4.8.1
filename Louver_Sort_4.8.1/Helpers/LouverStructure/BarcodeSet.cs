using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel;
using System.Globalization;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    //// Custom converter for BarcodeSet
    //public class BarcodeSetConverter : JsonConverter<BarcodeSet>
    //{
    //    public override BarcodeSet ReadJson(JsonReader reader, Type objectType, BarcodeSet existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        JObject obj = JObject.Load(reader);
    //        try
    //        {
    //            string barcode1 = obj["Barcode1"].Value<string>();
    //            string barcode2 = obj["Barcode2"].Value<string>();
    //            return new BarcodeSet(barcode1, barcode2);
    //        }
    //        catch (Exception ex)
    //        {
    //            // Log or handle the error appropriately
    //            throw new JsonSerializationException("Error reading BarcodeSet from JSON.", ex);
    //        }
    //    }

    //    public override void WriteJson(JsonWriter writer, BarcodeSet value, JsonSerializer serializer)
    //    {
    //        // Create a new JSON object to represent the BarcodeSet
    //        JObject obj = new JObject();
    //        obj.Add("Barcode1", value.Barcode1);
    //        obj.Add("Barcode2", value.Barcode2);

    //        // Write the JSON object to the writer
    //        obj.WriteTo(writer);
    //    }
    //}

    //public class BarcodeSetConverter : TypeConverter
    //{
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (value is string)
    //        {
    //            string[] parts = ((string)value).Split(new char[] { ',' });
    //            return new BarcodeSet(parts[0], parts[1]); // Adjust parsing as necessary
    //        }
    //        return base.ConvertFrom(context, culture, value);
    //    }
    //}

    //public class BarcodeSetConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(BarcodeSet);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        JObject obj = JObject.Load(reader);
    //        string barcode1 = obj["Barcode1"].Value<string>();
    //        string barcode2 = obj["Barcode2"].Value<string>();
    //        return new BarcodeSet(barcode1, barcode2);
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        BarcodeSet barcodeSet = (BarcodeSet)value;
    //        JObject obj = new JObject
    //    {
    //        { "Barcode1", barcodeSet.Barcode1 },
    //        { "Barcode2", barcodeSet.Barcode2 }
    //    };
    //        obj.WriteTo(writer);
    //    }

    //}

    //public class BarcodeSetTypeConverter : TypeConverter
    //{

    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    //    }

    //    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //    {
    //        if (value is string stringValue)
    //        {
    //            // Check for null or whitespace
    //            if (String.IsNullOrWhiteSpace(stringValue))
    //            {
    //                throw new ArgumentException("The string is null or whitespace.");
    //            }

    //            // Split the string by comma
    //            var parts = stringValue.Split(',');
    //            // Check for the expected number of parts
    //            if (parts.Length == 2)
    //            {
    //                // Further validation, if necessary, can go here
    //                return new BarcodeSet(parts[0].Trim(), parts[1].Trim());
    //            }
    //            else
    //            {
    //                // Log the stringValue for debugging purposes
    //                // Logger.Log($"Unexpected format for BarcodeSet: {stringValue}");
    //                throw new ArgumentException($"Invalid format for BarcodeSet: {stringValue}");
    //            }
    //        }
    //        else
    //        {
    //            // This else block can handle cases where value is not a string
    //            // Logger.Log("Expected a string for BarcodeSet conversion.");
    //            throw new ArgumentException("Expected a string for BarcodeSet conversion.");
    //        }
    //    }
    //}

    //// Apply the JsonConverter attribute to the BarcodeSet class
    //[JsonConverter(typeof(BarcodeSetConverter))]
    //[TypeConverter(typeof(BarcodeSetTypeConverter))]
    public class BarcodeSet
    {
        public string Barcode1 { get; set; }
        public string Barcode2 { get; set; }

        public BarcodeSet(string b1, string b2)
        {
            Barcode1 = b1;
            Barcode2 = b2;
        }

        // Add any other constructors or methods as needed

        public override string ToString()
        {
            return $"{Barcode1},{Barcode2}";
        }

        public override bool Equals(object obj)
        {
            return obj is BarcodeSet other &&
                   Barcode1 == other.Barcode1 &&
                   Barcode2 == other.Barcode2;
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + (Barcode1 != null ? Barcode1.GetHashCode() : 0);
                hash = hash * 23 + (Barcode2 != null ? Barcode2.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
