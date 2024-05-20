using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour
{

    private Vector3 offset;
    private bool isDragging = false;

    public bool inInventory = false;

    //public CanvasGroup canvas;

    void Update()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // Check if we clicked on this object
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isDragging = true;
                OnHandInventory.draggingItem = hit.collider.gameObject.GetComponent<Item>();
                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                OnHandInventory.draggingItem.gameObject.GetComponent<Collider2D>().enabled = false;
            }
        }

        // If dragging, update the object's position
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = -3; // Ensure the object stays in the 2D plane
            transform.position = newPosition;
        }

        // Stop dragging when the mouse button is released
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            OnHandInventory.draggingItem.gameObject.GetComponent<Collider2D>().enabled = true;
            OnHandInventory.lastDraggedItem = OnHandInventory.draggingItem;
            OnHandInventory.draggingItem = null;
        }
    }

}