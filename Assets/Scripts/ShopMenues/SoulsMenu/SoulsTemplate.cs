using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoulsTemplate : MonoBehaviour
{
    [Header("TMP_Texts")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;

    [Header("Arrays")]
    public GameObject[] starsActive;
    public GameObject[] starsInactive;
}
