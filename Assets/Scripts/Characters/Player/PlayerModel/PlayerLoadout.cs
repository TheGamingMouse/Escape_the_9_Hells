using UnityEngine;
using static SaveSystemSpace.SaveClasses;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool start;
    public bool backpackActive;
    public bool seedBagActive;
    bool ready;

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
    public LoadoutItemsSO selectedPrimaryWeapon;
    public LoadoutItemsSO selectedSecondaryWeapon;
    public LoadoutItemsSO selectedCompanion;
    public LoadoutItemsSO selectedPrimaryCompanion;
    public LoadoutItemsSO selectedSecondaryCompanion;
    public LoadoutItemsSO selectedArmor;
    public LoadoutItemsSO selectedBack;

    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultArmor;
    public LoadoutItemsSO defaultBack;

    [Header("Components")]
    Weapon playerWeapon;
    Companion playerCompanion;
    Armor playerArmor;
    Backs playerBack;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (NPCSpawner.Instance)
        {
            if (NPCSpawner.Instance.rickyStart) ready = true;
        }
        else ready = true;
        
        if (!start && ready)
        {
            playerWeapon = GetComponentInChildren<Weapon>();
            playerCompanion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
            playerArmor = GetComponentInChildren<Armor>();
            playerBack = GetComponentInChildren<Backs>();
            
            var equipmentData = SaveSystem.loadedEquipmentData;

            selectedPrimaryWeapon = equipmentData.weaponData.primaryWeapon;
            selectedSecondaryWeapon = equipmentData.weaponData.secondaryWeapon;
            selectedWeapon = equipmentData.weaponData.selectedWeapon;

            selectedPrimaryCompanion = equipmentData.companionData.primaryCompanion;
            selectedSecondaryCompanion = equipmentData.companionData.secondaryCompanion;
            selectedCompanion = equipmentData.companionData.selectedCompanion;

            selectedArmor = equipmentData.armorData.selectedArmor;
            selectedBack = equipmentData.backData.selectedBack;

            // Weapon
            if (playerWeapon != null) UpdateWeapon();
            if (!selectedWeapon)
            {
                selectedWeapon = defaultWeapon;
                UpdateWeapon();
            }

            // Companion
            if (playerCompanion != null) UpdateCompanion();
            if (!selectedCompanion)
            {
                selectedCompanion = defaultCompanion;
                UpdateCompanion();
            }

            // Armor
            if (playerArmor != null) UpdateArmor();
            if (!selectedArmor)
            {
                selectedArmor = defaultArmor;
                UpdateArmor();
            }

            // Back
            if (playerBack != null) UpdateBack();
            if (!selectedBack)
            {
                selectedBack = defaultBack;
                UpdateBack();
            }

            start = true;
        }
    }

    #endregion

    #region General Methods

    public void SetLoadout(LoadoutItemsSO primaryWeapon, LoadoutItemsSO secondaryWeapon, LoadoutItemsSO primaryCompanion, LoadoutItemsSO secondaryCompanion, LoadoutItemsSO armor, LoadoutItemsSO back)
    {
        var loadedEquipmentData = SaveSystem.loadedEquipmentData;

        selectedPrimaryWeapon = primaryWeapon;
        loadedEquipmentData.weaponData.primaryWeapon = primaryWeapon;
        if (secondaryWeapon)
        {
            selectedSecondaryWeapon = secondaryWeapon;
            loadedEquipmentData.weaponData.secondaryWeapon = secondaryWeapon;
        }
        else
        {
            selectedSecondaryWeapon = primaryWeapon;
            loadedEquipmentData.weaponData.secondaryWeapon = primaryWeapon;
        }
        selectedWeapon = selectedPrimaryWeapon;
        loadedEquipmentData.weaponData.selectedWeapon = selectedWeapon;

        selectedPrimaryCompanion = primaryCompanion;
        loadedEquipmentData.companionData.primaryCompanion = primaryCompanion;
        if (secondaryCompanion)
        {
            selectedSecondaryCompanion = secondaryCompanion;
            loadedEquipmentData.companionData.secondaryCompanion = secondaryCompanion;
        }
        else
        {
            selectedSecondaryCompanion = primaryCompanion;
            loadedEquipmentData.companionData.selectedCompanion = primaryCompanion;
        }
        selectedCompanion = selectedPrimaryCompanion;
        loadedEquipmentData.companionData.selectedCompanion = selectedCompanion;

        selectedArmor = armor;
        loadedEquipmentData.armorData.selectedArmor = selectedArmor;

        selectedBack = back;
        loadedEquipmentData.backData.selectedBack = selectedBack;

        SaveSystem.Instance.Save(loadedEquipmentData, SaveSystem.equipmentDataPath);

        if (playerWeapon) UpdateWeapon();
        if (playerCompanion) UpdateCompanion();
        if (playerArmor) UpdateArmor();
        if (playerBack) UpdateBack();
    }

    void UpdateWeapon()
    {
        if (selectedBack && selectedBack.title == EquipmentData.BackData.backpackString) backpackActive = true;
        else backpackActive = false;

        var weapon = selectedWeapon.title switch
        {
            EquipmentData.WeaponData.ulfberhtString => Weapon.WeaponActive.Ulfberht,
            EquipmentData.WeaponData.pugioString => Weapon.WeaponActive.Pugio,

            _ => Weapon.WeaponActive.Pugio
        };
        playerWeapon.SwitchWeapon(weapon);

        PlayerComponents.Instance.playerUpgrades.weaponUpdated = false;
    }

    void UpdateCompanion()
    {
        if (selectedBack && selectedBack.title == EquipmentData.BackData.seedBagString) seedBagActive = true;
        else seedBagActive = false;

        var companion = selectedCompanion.title switch
        {
            EquipmentData.unequipedString => Companion.CompanionActive.None,
            EquipmentData.CompanionData.loyalSphereString => Companion.CompanionActive.LoyalSphere,
            EquipmentData.CompanionData.attackSquareString => Companion.CompanionActive.AttackSquare,

            _ => Companion.CompanionActive.None
        };
        playerCompanion.SwitchCompanion(companion);

        PlayerComponents.Instance.playerUpgrades.companionUpdated = false;
    }

    void UpdateArmor()
    {
        var armor = selectedArmor.title switch
        {
            EquipmentData.unequipedString => Armor.ArmorActive.None,
            EquipmentData.ArmorData.leatherString => Armor.ArmorActive.Leather,
            EquipmentData.ArmorData.hideString => Armor.ArmorActive.Hide,
            EquipmentData.ArmorData.ringMailString => Armor.ArmorActive.RingMail,
            EquipmentData.ArmorData.plateString => Armor.ArmorActive.Plate,

            _ => Armor.ArmorActive.None
        };
        playerArmor.SwitchArmor(armor);

        PlayerComponents.Instance.playerUpgrades.armorUpdated = false;
    }

    void UpdateBack()
    {
        var back = selectedBack.title switch
        {
            EquipmentData.unequipedString => Backs.BackActive.None,
            EquipmentData.BackData.angelWingsString => Backs.BackActive.AngelWings,
            EquipmentData.BackData.steelWingsString => Backs.BackActive.SteelWings,
            EquipmentData.BackData.backpackString => Backs.BackActive.Backpack,
            EquipmentData.BackData.capeOWindString => Backs.BackActive.CapeOWind,
            EquipmentData.BackData.seedBagString => Backs.BackActive.SeedBag,

            _ => Backs.BackActive.None,
        };
        playerBack.SwitchBack(back);
        
        PlayerComponents.Instance.playerUpgrades.backUpdated = false;
    }

    #endregion
}
