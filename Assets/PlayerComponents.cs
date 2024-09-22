using UnityEngine;

public class PlayerComponents : MonoBehaviour
{
    public static PlayerComponents Instance;

    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public PlayerLevel playerLevel;
    public PlayerPerks playerPerks;
    public PlayerSouls playerSouls;
    public PlayerLoadout playerLoadout;
    public PlayerEquipment playerEquipment;
    public PlayerUpgrades playerUpgrades;
    public Transform player;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        playerLevel = GetComponent<PlayerLevel>();
        playerPerks = GetComponent<PlayerPerks>();
        playerSouls = GetComponent<PlayerSouls>();
        playerLoadout = GetComponent<PlayerLoadout>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerUpgrades = GetComponent<PlayerUpgrades>();

        player = GameObject.FindWithTag("Player").transform;
    }
}
