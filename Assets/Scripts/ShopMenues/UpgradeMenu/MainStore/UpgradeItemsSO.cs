using UnityEngine;

[CreateAssetMenu(fileName = "StoreMenu", menuName = "Scriptable Objects/New Upgrade Item", order = 4)]
public class UpgradeItemsSO : ScriptableObject
{
    [Header("Strings")]
    public string title;
    public string description;
    public int price;
}
