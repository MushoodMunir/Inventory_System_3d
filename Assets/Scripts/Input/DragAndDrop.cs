using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    private InventoryItem selectedItem;
    private Vector3 offset;
    private float zDistance;

    public Backpack backpack;
    public float dropScreenThreshold = 50f;
    public float groundThreshold = 0f;
    public LayerMask draggableLayer;

    // Define allowed horizontal drop boundaries.
    public float dropAreaXMin = -10f;
    public float dropAreaXMax = 10f;
    public float dropAreaZMin = -10f;
    public float dropAreaZMax = 10f;
    Rigidbody selectedItemRigidbody;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, draggableLayer))
            {
                InventoryItem item = hit.collider.GetComponent<InventoryItem>();
                // Only allow dragging if the item is not already stored.
                if (item != null && !item.isStored)
                {
                    selectedItem = item;
                    selectedItemRigidbody = selectedItem.GetComponent<Rigidbody>();
                    if (selectedItemRigidbody != null)
                    {
                        selectedItemRigidbody.isKinematic = true;
                    }
                    zDistance = Vector3.Distance(mainCamera.transform.position, selectedItem.transform.position);
                    Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance));
                    offset = selectedItem.transform.position - worldPoint;
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedItem != null)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance);
            Vector3 targetPosition = mainCamera.ScreenToWorldPoint(currentScreenPoint) + offset;
            if (selectedItemRigidbody != null)
            {
                selectedItemRigidbody.MovePosition(targetPosition);
            }
            else
            {
                selectedItem.transform.position = targetPosition;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedItem != null)
        {
            // If dropped too far below ground or outside the horizontal safe area, reset its position.
            if (selectedItem.transform.position.y < groundThreshold ||
                selectedItem.transform.position.x < dropAreaXMin ||
                selectedItem.transform.position.x > dropAreaXMax ||
                selectedItem.transform.position.z < dropAreaZMin ||
                selectedItem.transform.position.z > dropAreaZMax)
            {
                ResetItemPosition();
                return;
            }

            // Use screen-space distance between item and backpack.
            Vector3 itemScreenPos = mainCamera.WorldToScreenPoint(selectedItem.transform.position);
            Vector3 backpackScreenPos = mainCamera.WorldToScreenPoint(backpack.transform.position);
            float screenDistance = Vector2.Distance(new Vector2(itemScreenPos.x, itemScreenPos.y),
                                                     new Vector2(backpackScreenPos.x, backpackScreenPos.y));

            if (screenDistance < dropScreenThreshold)
            {
                backpack.StoreItem(selectedItem);
                NetworkHandler.Instance.SendInventoryEvent(selectedItem.identifier, "stored");
            }
            else
            {
                if (selectedItemRigidbody != null)
                {
                    selectedItemRigidbody.isKinematic = false;
                }
            }
            selectedItem = null;
            selectedItemRigidbody = null;
        }
    }

    private void ResetItemPosition()
    {
        if (selectedItem != null)
        {
            selectedItem.transform.position = selectedItem.originalPosition;
            if (selectedItemRigidbody != null)
            {
                selectedItemRigidbody.isKinematic = false;
            }
            selectedItem = null;
            selectedItemRigidbody = null;
        }
    }
}
