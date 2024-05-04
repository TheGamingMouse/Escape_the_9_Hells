using UnityEngine;

[CreateAssetMenu(fileName = "StoreMenu", menuName = "Scriptable Objects/New Perk Item", order = 1)]
public class PerkItemsSO : ScriptableObject
{
    [Header("Strings")]
    public string title;
    public string description;
}
