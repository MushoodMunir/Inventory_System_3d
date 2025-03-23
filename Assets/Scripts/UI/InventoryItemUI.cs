using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemUI : MonoBehaviour, IPointerUpHandler
{
    public InventoryItem linkedItem;
    public Backpack backpack;
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RectTransform rt = GetComponent<RectTransform>();
            // Check if the mouse is within this UI element's rectangle.
            bool contains = RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition);
            Debug.Log("Input.mousePosition: " + Input.mousePosition + ", RectContains: " + contains);

            if (contains)
            {
                Debug.Log("Mouse released over UI element. Retrieval triggered.");
                HandleRetrieval();
            }
            else
            {
                Debug.Log("Mouse released, but not over this UI element.");
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp called from IPointerUpHandler");
        // It might not trigger if the user didn't start the press on this element.
        HandleRetrieval();
    }

    private void HandleRetrieval()
    {
        if (backpack != null && linkedItem != null)
        {
            backpack.RetrieveItem(linkedItem);
            NetworkHandler.Instance.SendInventoryEvent(linkedItem.identifier, "retrieved");
        }
        if (inventoryManager != null)
        {
            inventoryManager.RemoveItem(linkedItem);
        }
        Destroy(gameObject);
    }
}
