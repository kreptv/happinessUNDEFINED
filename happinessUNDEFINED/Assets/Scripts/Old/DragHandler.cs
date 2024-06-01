using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;

    public bool inInventory = false;

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // Create a ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast and check if we hit this object
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                Debug.Log("hit " + hit.collider.gameObject.name);
                isDragging = true;
                OnHandInventory.draggingItem = hit.collider.gameObject.GetComponent<Item>();
                offset = gameObject.transform.position - hit.point;
                OnHandInventory.draggingItem.gameObject.GetComponent<Collider>().enabled = false;
            }
        }

        // If dragging, update the object's position
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast, ignoring trigger colliders
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0, QueryTriggerInteraction.Ignore))
            {
                Vector3 newPosition = hit.point + offset;
                newPosition.y = gameObject.transform.position.y; // Ensure the object stays at its original y position
                transform.position = newPosition;
            }
        }

        // Stop dragging when the mouse button is released
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            OnHandInventory.draggingItem.gameObject.GetComponent<Collider>().enabled = true;
            OnHandInventory.lastDraggedItem = OnHandInventory.draggingItem;
            OnHandInventory.draggingItem = null;
        }
    }
}