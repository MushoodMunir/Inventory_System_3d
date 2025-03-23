using UnityEngine;
using System.Collections;

public class BackpackUITrigger : MonoBehaviour
{
    public LayerMask hitLayer;
    public GameObject inventoryCanvas;
    public InventoryManager invManager;

    // Flag to lock the UI on once activated.
    private bool uiLocked = false;

    // Returns true if the pointer is currently over this backpack object.
    private bool IsMouseOverBackpack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayer))
        {
            return hit.transform == transform;
        }
        return false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsMouseOverBackpack())
            {
                inventoryCanvas.SetActive(true);
                uiLocked = true;
                // Update the UI when activated.
                if (invManager != null)
                {
                    invManager.UpdateUI();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && uiLocked)
        {
            // Instead of immediately disabling the canvas, delay it slightly.
            StartCoroutine(DisableCanvasAfterDelay());
        }
    }

    IEnumerator DisableCanvasAfterDelay()
    {
        yield return new WaitForSeconds(0.2f); // Adjust delay as needed.
        inventoryCanvas.SetActive(false);
        uiLocked = false;
    }
}
