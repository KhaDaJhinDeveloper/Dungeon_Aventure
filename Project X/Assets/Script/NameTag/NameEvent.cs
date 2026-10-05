using UnityEngine;

public class NameEvent 
{
    public const string Event_NavMeshSetUp = "NavmeshSetup";
    public const string Event_PlayerAnimationDrop = "AnimationDrop";
    //UI=========================
    //-------CONTENTUI
        public const string Event_LoadIndex = "LoadIndex";
        public const string Event_ShowContentBoard = "ShowContentBoard";
        public const string Event_HideContentBroad = "HideContentBroad";
    //-------STATS
    public const string Event_LoadHPBar = "loadHP";
    public const string Event_loadArmorBar ="LoadArmor";
    public const string Event_LoadAntiMagicBar = "LoadAntiMagic";
    public const string Event_LoadManaBar = "LoadMana";
    public const string Event_LoadHPText = "LoadHPText";
    public const string Event_LoadManaText = "LoadManaText";
    //-------BUTTON
    //--------------ButtonTrigger
    public const string Event_ShowButtonTrigger = "EventShowButton";
        public const string Event_HiddenButtonTrigger = "EventHiddenButton";

   //-------TEXT
        //--------------TextPopup
        public const string Event_LoadCoinText = "LoadCoinText";
        public const string Event_ShowDamageText = "ShowDamageText";
        //--------------TextDialog
        public const string Event_LoadDialogText = "LoadDialogText";
    //--------SHOPUI
        public const string Event_OpenShop = "OpenShop";
        public const string Event_CloseShop = "CloseShop";
    //--------WEAPON
        //WeaponSlotUI
        public const string Event_ImageSlotWeapon_LoadWeaponSlotUI = "LoadWeaponSlotUI";
        //WeaponController
        public const string Event_WeaponControll_ChangeSlotWeapon = "ChangeSlotWeapon";
        public const string Event_WeaponControll_SwapWeapon = "SwapWeapon";
        //WeaponChangeSLot
        public const string Event_ChangeWeaponSlot_LoadInfoWeapon = "LoadInfoWeapon";
        public const string Event_ChangeWeaponSlot_HideUI = "HideChangeSlotUI";
        public const string Event_ChangeWeaponSlot_ShowUI = "ShowChangeSlotUI";
    //--------SOUND
    //DATA=========================
    //-------INVENTORY
    public const string Event_InventorySaveData = "InventorySaveData";
    public const string Event_InventoryLoadData = "InventoryLoadData";
    public const string Event_InventoryDeleteData = "InventoryDeleteData";
    //BOSSEVENT====================
    public const string Event_StartBossFight = "StartBossFight";
    public const string Event_EndBossFight = "EndBossFight";
}
