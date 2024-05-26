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
    private static Collider boundary; // Reference to the PolygonCollider2D boundary
    [SerializeField] private float boundaryMargin = 0.5f; // Margin to keep the camera inside the boundary

    [SerializeField] private float fadeDistance = 1.0f; // Distance at which sprites start to fade
    [SerializeField] private float disappearDistance = 0.5f; // Distance at which sprites disappear completely

    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    private List<Collider> colliders = new List<Collider>();

    public static void UpdateSize(float size)
    {
        Camera.main.orthographicSize = size;
    }

    public static void UpdateBoundary(Collider collider)
    {
        boundary = collider;
    }

    void Start()
    {
        // Get all sprite renderers and colliders in the scene
        spriteRenderers.AddRange(FindObjectsOfType<SpriteRenderer>());
        colliders.AddRange(FindObjectsOfType<Collider>());
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Clamp the camera position to the PolygonCollider2D boundary
        Vector3 clampedPosition = ClampPositionToBoundary(smoothedPosition);

        transform.position = clampedPosition;

        HandleSpriteFading();
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

    void HandleSpriteFading()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer)
            {
                float distanceToCamera = Vector3.Distance(spriteRenderer.transform.position, transform.position);

                if (distanceToCamera < disappearDistance)
                {
                    SetSpriteAlpha(spriteRenderer, 0f);
                    ToggleCollider(spriteRenderer.gameObject, false);
                }
                else if (distanceToCamera < fadeDistance)
                {
                    float alpha = Mathf.InverseLerp(disappearDistance, fadeDistance, distanceToCamera);
                    SetSpriteAlpha(spriteRenderer, alpha);
                    ToggleCollider(spriteRenderer.gameObject, true);
                }
                else
                {
                    SetSpriteAlpha(spriteRenderer, 1f);
                    ToggleCollider(spriteRenderer.gameObject, true);
                }
            }
        }
    }

    void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void ToggleCollider(GameObject obj, bool enable)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = enable;
        }
    }
}