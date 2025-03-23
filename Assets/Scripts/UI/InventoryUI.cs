using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    public RectTransform typeAPanel;
    public RectTransform typeBPanel;
    public RectTransform typeCPanel;
    public GameObject uiItemPrefab;
    public InventoryManager invManager;
    public Backpack backpack;

    public void UpdateUI(List<InventoryItem> items)
    {
        ClearPanel(typeAPanel);
        ClearPanel(typeBPanel);
        ClearPanel(typeCPanel);

        foreach (InventoryItem item in items)
        {
            GameObject uiItem = Instantiate(uiItemPrefab);
            uiItem.GetComponentInChildren<Text>().text = item.itemName;

            switch (item.type)
            {
                case ItemType.TypeA:
                    uiItem.transform.SetParent(typeAPanel, false);
                    break;
                case ItemType.TypeB:
                    uiItem.transform.SetParent(typeBPanel, false);
                    break;
                case ItemType.TypeC:
                    uiItem.transform.SetParent(typeCPanel, false);
                    break;
            }

            // Set up the UI button to retrieve the item when clicked.
            InventoryItemUI itemUI = uiItem.GetComponent<InventoryItemUI>();
            if (itemUI != null)
            {
                itemUI.linkedItem = item;
                itemUI.backpack = backpack;
                itemUI.inventoryManager = invManager;
            }
        }
    }

    private void ClearPanel(RectTransform panel)
    {
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }
    }

    public void OnReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
