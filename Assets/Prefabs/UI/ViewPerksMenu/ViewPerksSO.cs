using UnityEngine;

[CreateAssetMenu(fileName = "ViewPerks", menuName = "Scriptable Objects/New View Perk Item", order = 5)]
public class ViewPerksSO : ScriptableObject
{
    [Header("Strings")]
    public string title;
    public int amount;
    public bool active;
    public bool isUsed;
}