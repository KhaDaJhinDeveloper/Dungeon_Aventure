using UnityEngine;

public class NameEvent 
{
    public const string Event_PlayerAnimationDrop = "AnimationDrop";
    //================UI=================================================================
    //-------CONTENTUI-----------------------------------------------------------------
        public const string Event_LoadIndex = "LoadIndex";
        public const string Event_ShowContentBoard = "ShowContentBoard";
        public const string Event_HideContentBroad = "HideContentBroad";
    //-------BUTTON--------------------------------------------------------------------
    //--------------ButtonTrigger----------------------------------------------------
    public const string Event_ShowButtonTrigger = "EventShowButton";
        public const string Event_HiddenButtonTrigger = "EventHiddenButton";

      //-------TEXT----------------------------------------------------------------------
        //--------------TextPopup--------------------------------------------------------
        public const string Event_LoadCoinText = "LoadCoinText";
        public const string Event_ShowDamageText = "ShowDamageText";
        //--------------TextDialog-------------------------------------------------------
        public const string Event_LoadDialogText = "LoadDialogText";
      //--------SHOPUI-------------------------------------------------------------------
        public const string Event_OpenShop = "OpenShop";
        public const string Event_CloseShop = "CloseShop";
    //--------SOUND---------------------------------------------------------------------
    //================DATA================================================================
    //-------INVENTORY-----------------------------------------------------------------
    public const string Event_InventorySaveData = "InventorySaveData";
    public const string Event_InventoryLoadData = "InventoryLoadData";
    public const string Event_InventoryDeleteData = "InventoryDeleteData";
    //================BOSSEVENT============================================================
    public const string Event_StartBossFight = "StartBossFight";
    public const string Event_EndBossFight = "EndBossFight";
}
