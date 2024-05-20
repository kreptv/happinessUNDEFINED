using UnityEngine;

public class HighlightController : MonoBehaviour
{

    public GameObject outlineObject; // Assign the outline object in the inspector

    void Start()
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(false); // Start with the outline disabled
        }
    }

    public void SetHighlight(bool highlight)
    {
        if (outlineObject != null)
        {
            outlineObject.SetActive(highlight);
        }
    }
    void OnMouseEnter()
    {
        SetHighlight(true);
        Debug.Log("Mouse entering area");
    }

    void OnMouseExit()
    {
        SetHighlight(false);
        Debug.Log("Mouse leaving area");
    }
}