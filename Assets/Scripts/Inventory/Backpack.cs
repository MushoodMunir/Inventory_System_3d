
using System.Collections;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("Backpack Settings")]
    public Transform typeAHolder;
    public Transform typeBHolder;
    public Transform typeCHolder;

    [Header("Animation Settings")]
    public float snapSpeed = 5f;

    public InventoryManager invManager;

    // Smoothly stores an item by moving it to its designated slot.
    public void StoreItem(InventoryItem item)
    {
        Transform targetHolder = GetHolderForType(item.type);
        if (targetHolder != null)
        {
            StartCoroutine(SmoothTransition(item.transform, targetHolder.position, targetHolder.rotation, item, true));
        }
        else
        {
            Debug.LogWarning("No holder found for item type: " + item.type);
        }
    }

    
    // Smoothly retrieves an item by moving it back to its original position.
    public void RetrieveItem(InventoryItem item)
    {
        StartCoroutine(SmoothTransition(item.transform, item.originalPosition, item.originalRotation, item, false));
    }

    private IEnumerator SmoothTransition(Transform itemTransform, Vector3 targetPosition, Quaternion targetRotation, InventoryItem item, bool isStoring)
    {
        while (Vector3.Distance(itemTransform.position, targetPosition) > 0.01f)
        {
            itemTransform.position = Vector3.Lerp(itemTransform.position, targetPosition, Time.deltaTime * snapSpeed);
            itemTransform.rotation = Quaternion.Lerp(itemTransform.rotation, targetRotation, Time.deltaTime * snapSpeed);
            yield return null;
        }
        itemTransform.position = targetPosition;
        itemTransform.rotation = targetRotation;

        if (isStoring)
        {
            item.onStore.Invoke(item);
            item.isStored = true;
            // Notify the InventoryManager that the item is stored.
            if (invManager != null)
            {
                invManager.AddItem(item);
            }
        }
        else
        {
            item.onRetrieve.Invoke(item);
            item.isStored = false;
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
            // Notify InventoryManager that the item is retrieved.
            if (invManager != null)
            {
                invManager.RemoveItem(item);
            }
        }
    }

    private Transform GetHolderForType(ItemType type)
    {
        switch (type)
        {
            case ItemType.TypeA:
                return typeAHolder;
            case ItemType.TypeB:
                return typeBHolder;
            case ItemType.TypeC:
                return typeCHolder;
            default:
                return null;
        }
    }
}
