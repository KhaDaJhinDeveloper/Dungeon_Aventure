using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponsSaveLoad : Singleton<WeaponsSaveLoad>,IDataManager
{
    private const string FILE_WEAPONS_DATA = "WeaponsData.json";
    protected override void Awake()
    {
        base.Awake();
    }
    public void SaveData()
    {
        WeaponController controller = FindFirstObjectByType<WeaponController>();
        if (controller == null) return;
        PlayerWeaponSlotData slotData = new PlayerWeaponSlotData();
        if (controller.weaponsArray[0].isFull)
            slotData.weaponSlotLeft = controller.weaponsArray[0].currentWeapon?.ToSaveData();
        if (controller.weaponsArray[1].isFull)
            slotData.weaponSlotRight = controller.weaponsArray[1].currentWeapon?.ToSaveData();
        if (controller.weaponReserve.isFull)
            slotData.weaponReserve = controller.weaponReserve.currentWeapon?.ToSaveData();
        JsonFileUtility.SaveToJson(slotData, FILE_WEAPONS_DATA);
    }
    public void LoadData()
    {
        PlayerWeaponSlotData slotData = JsonFileUtility.LoadFromJson<PlayerWeaponSlotData>(FILE_WEAPONS_DATA);
        if (slotData == null) return;
        WeaponController controller = Object.FindFirstObjectByType<WeaponController>();
        if (controller == null) return;
        LoadDataWeaponSlot(controller.weaponsArray[0], slotData.weaponSlotLeft);
        LoadDataWeaponSlot(controller.weaponsArray[1], slotData.weaponSlotRight);
        LoadDataWeaponSlot(controller.weaponReserve, slotData.weaponReserve);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ImageSlotWeapon_LoadWeaponSlotUI);
        EventManager.Instance.TriggerEvent(NameEvent.Event_ChangeWeaponSlot_LoadInfoWeapon);
    }
    private void LoadDataWeaponSlot(WeaponSlot slot , WeaponSaveData data)
    {
        if (slot == null) return;
        if (data == null || string.IsNullOrEmpty(data.weaponID))
        {
            slot.ClearSlot();
            return;
        }
        WeaponData baseData = WeaponsDatabase.Instance.GetItemByName(data.weaponID);
        if (baseData != null)
        {
            WeaponInstance weaponInstance = new WeaponInstance(baseData, data);
            slot.SetWeapon(weaponInstance);
        }
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_WEAPONS_DATA);
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_WEAPONS_DATA);
    }
}
