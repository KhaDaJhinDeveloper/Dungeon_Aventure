using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Crafting/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public RecoveryItem item1;
    public RecoveryItem item2;
    public RecoveryItem result;
}
