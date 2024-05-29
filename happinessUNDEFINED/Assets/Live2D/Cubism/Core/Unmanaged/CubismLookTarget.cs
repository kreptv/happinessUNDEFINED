using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class CubismLookTarget : MonoBehaviour, ICubismLookTarget
{
    public Vector3 GetPosition()
    {
        // Get the mouse position in screen coordinates
        var targetPosition = Input.mousePosition;

        // Set the Z distance to the distance from the camera to the model
        //float modelZ = Camera.main.WorldToScreenPoint(transform.position).z;

        // Convert the screen coordinates to world coordinates
        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, 186));
        //Debug.Log("ModelZ: " + modelZ);

        // Adjust the position to match the model's local coordinates
        targetPosition = transform.InverseTransformPoint(targetPosition);

        // Ensure the target position is only affecting the x and y axis
        //targetPosition.z = 0;

        Debug.Log("Eye target position: " + targetPosition);
        return targetPosition;
    }

    public bool IsActive()
    {
        return true;
    }
}