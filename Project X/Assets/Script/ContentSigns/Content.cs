using UnityEngine;
[CreateAssetMenu(menuName = "Content/ContentSigns")]
public class Content : ScriptableObject
{
    public Sprite spriteDescription;
    [TextArea] public string textDescription;
}
