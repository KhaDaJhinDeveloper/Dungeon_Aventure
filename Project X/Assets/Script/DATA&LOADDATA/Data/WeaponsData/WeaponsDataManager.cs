using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponsDataManager : Singleton<WeaponsDataManager>,IDataManager
{
    public static WeaponsDataManager S_WeaponsDataManager {  get; private set; }
    private const string FILE_WEAPONS_DATA = "WeaponsData.json";
    private WeaponsData weaponsData = new WeaponsData();
    protected override void Awake()
    {
        base.Awake();
        S_WeaponsDataManager = this;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void SaveData()
    {
        WeaponManager  weaponManager = GameObject.FindFirstObjectByType<WeaponManager>();
        if (weaponManager != null)
        {
            string weapon1Name = (weaponManager.Weaponlist.Count > 0 && weaponManager.Weaponlist[0] != null)
                ? weaponManager.Weaponlist[0].name : string.Empty;
            string weapon2Name = (weaponManager.Weaponlist.Count > 1 && weaponManager.Weaponlist[1] != null)
                ? weaponManager.Weaponlist[1].name : string.Empty;
            string weaponReserveName = weaponManager.WeaponReserve != null
                ? weaponManager.WeaponReserve.name : string.Empty;


            this.weaponsData = new WeaponsData(weapon1Name, 
                                               weapon2Name, 
                                               weaponReserveName);
            JsonFileUtility.SaveToJson(this.weaponsData, FILE_WEAPONS_DATA);
        }
    }

    public void LoadData()
    {
        WeaponsData loadWeaponsData = JsonFileUtility.LoadFromJson<WeaponsData>(FILE_WEAPONS_DATA);
        WeaponManager weaponManager = GameObject.FindFirstObjectByType<WeaponManager>();
        if(loadWeaponsData != null)
        {
            weaponsData = loadWeaponsData;
            if (weaponManager == null) return;
            weaponManager.Weaponlist.Clear();
            weaponManager.AddWeaponList(weaponsData.weaponName1);
            DebugLogger.Log($"Load weapons{weaponsData.weaponName1}");
            weaponManager.AddWeaponList(weaponsData.weaponName2);
            DebugLogger.Log($"Load weapons{weaponsData.weaponName2}");
            weaponManager.AddWeaponReserve(weaponsData.weaponReserve);
            DebugLogger.Log($"Load weapons{weaponsData.weaponReserve}");
        }
        else DebugLogger.Log("WeaponsData not found!");
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            LoadData();
        }
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_WEAPONS_DATA);
        this.weaponsData = new WeaponsData();
    }
}
