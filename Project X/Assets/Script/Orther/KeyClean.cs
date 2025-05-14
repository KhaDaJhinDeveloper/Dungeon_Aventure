
public class KeyClean 
{
    public static string CleanKey(string rawKey)
    {
        return rawKey.Replace("(Clone)", "").Trim();
    }
}
