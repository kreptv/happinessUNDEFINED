using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

public class Live2DMouseFollow : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    //[SerializeField] private Transform modelTransform;

    private CubismModel cubismModel;

    private CubismParameter eyeXParameter;
    private CubismParameter eyeYParameter;
    private CubismParameter faceXParameter;
    private CubismParameter faceYParameter;

    // Range of movement for eyes and face parameters
    public float eyeMovementRange = 0.1f;
    public float faceMovementRange = 0.05f;

    void Start()
    {
        mainCamera = Camera.main;
        cubismModel = this.GetComponent<CubismModel>();

        // Find parameters for eye and face movements
        eyeXParameter = cubismModel.Parameters[18];
        eyeYParameter = cubismModel.Parameters[17];
        faceXParameter = cubismModel.Parameters[14];
        faceYParameter = cubismModel.Parameters[13];
    }

    private void LateUpdate()
    {
        if (mainCamera == null || cubismModel == null)
        {
            return;
        }

        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));

        Vector3 direction = worldMousePosition - this.transform.position;

        float eyeX = Mathf.Clamp(direction.x * eyeMovementRange, -1f, 1f);
        float eyeY = Mathf.Clamp(direction.y * eyeMovementRange, -1f, 1f);
        float faceX = Mathf.Clamp(direction.x * faceMovementRange, -1f, 1f);
        float faceY = Mathf.Clamp(direction.y * faceMovementRange, -1f, 1f);

        if (eyeXParameter != null)
        {
            eyeXParameter.Value = eyeX;
            Debug.Log(eyeXParameter + ": " + eyeX);
        }
        if (eyeYParameter != null)
        {
            eyeYParameter.Value = eyeY;
        }
        if (faceXParameter != null)
        {
            faceXParameter.Value = faceX;
        }
        if (faceYParameter != null)
        {
            faceYParameter.Value = faceY;
        }
    }
}
