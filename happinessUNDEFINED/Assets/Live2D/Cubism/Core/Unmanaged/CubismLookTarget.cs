using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    public Vector3 GetPosition()
    {
        var targetPosition = Input.mousePosition;
        targetPosition.z = Camera.main.nearClipPlane; // Set the Z distance to the near clipping plane
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition); // Convert to world point

        // Adjust the position to match the model's local coordinates
        targetPosition = transform.InverseTransformPoint(targetPosition);

        Debug.Log("Eye target position: " + targetPosition);
        return targetPosition;
    }

    public bool IsActive()
    {
        return true;
    }
}