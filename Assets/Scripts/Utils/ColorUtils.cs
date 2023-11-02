using System.Globalization;
using System;
using UnityEngine;

public class ColorUtils
{
    public static Color ParseHexColor(string hexString)
    // Translates a html hexadecimal definition of a color into a .NET Framework Color.
    // The input string must start with a '#' character and be followed by 6 hexadecimal
    // digits. The digits A-F are not case sensitive. If the conversion was not successful
    // the color white will be returned.
    {
        Color actColor;
        if (hexString.StartsWith("#") && hexString.Length == 7)
        {
            var r = HexToInt(hexString.Substring(1, 2));
            var g = HexToInt(hexString.Substring(3, 2));
            var b = HexToInt(hexString.Substring(5, 2));
            actColor = new Color(r / 255f, g / 255f, b / 255f);
        }
        else if (hexString.StartsWith("#") && hexString.Length == 9)
        {
            var r = HexToInt(hexString.Substring(1, 2));
            var g = HexToInt(hexString.Substring(3, 2));
            var b = HexToInt(hexString.Substring(5, 2));
            var a = HexToInt(hexString.Substring(7, 2));
            actColor = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        else
        {
            actColor = Color.white;
        }

        return actColor;
    }

    private static int HexToInt(string hex)
    {
        Int32.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hexInt);
        return hexInt;
        //return int.Parse(hex, NumberStyles.AllowHexSpecifier);
    }
}
