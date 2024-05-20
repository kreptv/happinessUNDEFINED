using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    #region Singleton
    public static CameraScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    #endregion

    [SerializeField] private Transform player; // Reference to the player's transform
    [SerializeField] private Vector3 offset; // Offset between the camera and the player
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing speed for the camera movement
    private static Collider2D boundary; // Reference to the PolygonCollider2D boundary
    [SerializeField] private float boundaryMargin = 0.5f; // Margin to keep the camera inside the boundary

    public static void UpdateSize(float size)
    {
        Camera.main.orthographicSize = size;
    }

    public static void UpdateBoundary(Collider2D collider)
    {
        boundary = collider;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp the camera position to the PolygonCollider2D boundary
        Vector3 clampedPosition = ClampPositionToBoundary(smoothedPosition);

        transform.position = clampedPosition;
    }

    Vector3 ClampPositionToBoundary(Vector3 position)
    {
        if (boundary == null)
        {
            return position;
        }

        // Calculate the boundaries adjusted for margin
        Bounds bounds = boundary.bounds;
        float minX = bounds.min.x + boundaryMargin;
        float maxX = bounds.max.x - boundaryMargin;
        float minY = bounds.min.y + boundaryMargin;
        float maxY = bounds.max.y - boundaryMargin;

        // Clamp the position inside the adjusted boundaries
        float clampedX = Mathf.Clamp(position.x, minX, maxX);
        float clampedY = Mathf.Clamp(position.y, minY, maxY);

        return new Vector3(clampedX, clampedY, position.z);
    }

}
