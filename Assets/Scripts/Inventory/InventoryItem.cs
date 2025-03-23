using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    TypeA,
    TypeB,
    TypeC
}

[System.Serializable]
public class InventoryEvent : UnityEvent<InventoryItem> { }

public class InventoryItem : MonoBehaviour
{
    [Header("Item Configurator")]
    public float weight = 1.0f;
    public string itemName = "Default Item";
    public string identifier = "ID_000";
    public ItemType type;

    // Flag indicating if the item is stored in the backpack.
    [HideInInspector]
    public bool isStored = false;

    [Header("Inventory Events")]
    public InventoryEvent onStore = new InventoryEvent();
    public InventoryEvent onRetrieve = new InventoryEvent();

    // Original position and rotation in the scene.
    [HideInInspector]
    public Vector3 originalPosition;
    [HideInInspector]
    public Quaternion originalRotation;

    private void Awake()
    {
        if (identifier == "ID_000")
        {
            identifier = System.Guid.NewGuid().ToString();
        }
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = weight;
        }
    }
}
