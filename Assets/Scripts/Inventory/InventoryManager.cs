using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> storedItems = new List<InventoryItem>();
    public InventoryUI inventoryUI;

    public void AddItem(InventoryItem item)
    {
        if (!storedItems.Contains(item))
        {
            storedItems.Add(item);
            UpdateUI();
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        if (storedItems.Contains(item))
        {
            storedItems.Remove(item);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.UpdateUI(storedItems);
        }
    }
}
