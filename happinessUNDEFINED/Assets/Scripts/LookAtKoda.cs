using Live2D.Cubism.Framework.LookAt;
using UnityEngine;

public class LookAtKoda : MonoBehaviour, ICubismLookTarget
{
    public Vector3 GetPosition()
    {
        return PlayerMovementScript.rb.gameObject.transform.position;
    }

    public bool IsActive()
    {
        return true;
    }
}
