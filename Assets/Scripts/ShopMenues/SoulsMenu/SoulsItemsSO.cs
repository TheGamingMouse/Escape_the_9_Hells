using UnityEngine;

[CreateAssetMenu(fileName = "StoreMenu", menuName = "Scriptable Objects/New Souls Upgrade", order = 1)]
public class SoulsItemsSO : ScriptableObject
{
    [Header("Strings")]
    public string title;
    public string description;
    public int price;
}
