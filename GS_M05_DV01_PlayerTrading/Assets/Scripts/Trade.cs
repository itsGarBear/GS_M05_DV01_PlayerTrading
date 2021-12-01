using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;
public class Trade : MonoBehaviour
{
    public GameObject tradeCanvas;
    public TextMeshProUGUI inventoryText;

    [HideInInspector]
    public List<ItemInstance> inventory;

    [HideInInspector]
    public List<CatalogItem> catalog;

    public static Trade instance;

    public UnityEvent onRefreshUI;

    public TextMeshProUGUI displayText;


    void Awake() { instance = this; }

    public void OnLoggedIn()
    {
        tradeCanvas.SetActive(true);
        if (onRefreshUI != null)
            onRefreshUI.Invoke();
    }

    public void SetDisplayText(string text, bool isError)
    {
        displayText.text = text;
        if (isError)
            displayText.color = Color.red;
        else
            displayText.color = Color.green;
        Invoke("HideDisplayText", 2.0f);
    }
    void HideDisplayText()
    {
        displayText.text = "";
    }

    public void GetInventory()
    {
        ExecuteCloudScriptRequest grantItems = new ExecuteCloudScriptRequest
        {
            FunctionName = "GiveItemsToNewUser"
        };
        PlayFabClientAPI.ExecuteCloudScript(grantItems,
            result =>
            {
                print("Granted items: " + result.ToJson());
            },
            error => print("Could not give user items")
        );

        inventoryText.text = "";
        
        GetPlayerCombinedInfoRequest getInvRequest = new GetPlayerCombinedInfoRequest
        {
            PlayFabId = LoginRegister.instance.playFabId,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetUserInventory = true
            }
        };

        PlayFabClientAPI.GetPlayerCombinedInfo(getInvRequest,
            result =>
            {
                inventory = result.InfoResultPayload.UserInventory;
                foreach (ItemInstance item in inventory)
                    inventoryText.text += item.DisplayName + ", ";
            },
            error => Debug.Log(error.ErrorMessage)
        );
    }
    public void GetCatalog()
    {
        GetCatalogItemsRequest getCatalogRequest = new GetCatalogItemsRequest
        {
            CatalogVersion = "PlayerItems"
        };

        PlayFabClientAPI.GetCatalogItems(getCatalogRequest,
            result => catalog = result.Catalog,
            error => Debug.Log(error.ErrorMessage)
        );
    }
}
