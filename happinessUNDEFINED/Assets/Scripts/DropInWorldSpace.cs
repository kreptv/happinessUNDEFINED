using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropInWorldSpace : EventTrigger
{

    public override void OnDrop(PointerEventData eventData)
    {

        Debug.Log("Dropping item in world space...");

            //DragHandler dragHandler = eventData.pointerDrag.GetComponent<DragHandler>();

    }



}