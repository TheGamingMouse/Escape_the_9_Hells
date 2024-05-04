using UnityEngine;

[CreateAssetMenu(fileName = "StoreMenu", menuName = "Scriptable Objects/New Loadout Item", order = 2)]
public class LoadoutItemsSO : ScriptableObject
{
    [Header("Strings")]
    public string title;
    public string description;
    public int price;
}
