
using System.Text.RegularExpressions;

public class KeyClean 
{
    public static string CleanKey(string rawKey)
    {
        return Regex.Replace(rawKey, @"(\(Clone\)|\(\d+\))", "").Trim();
    }
}
