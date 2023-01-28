using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

  public GameObject player;
  public Transform followTransform;

  public GameManager gm;

  private float camY,camX;
  private float camOrthsize;
  private float cameraRatio;
  private Camera mainCam;
  private Vector3 smoothPos;
  public float smoothSpeed = 0.5f;

  private void Start()
  {
      followTransform = player.GetComponent<Transform>();

      mainCam = GetComponent<Camera>();
      camOrthsize = mainCam.orthographicSize;
      cameraRatio = (gm.xMax + camOrthsize) / 2.0f;
      this.transform.position = player.transform.position - new Vector3(0, 0, 10);
  }

  void FixedUpdate()
  {
      camY = Mathf.Clamp(followTransform.position.y, gm.yMin + camOrthsize, gm.yMax - camOrthsize);
      camX = Mathf.Clamp(followTransform.position.x, gm.xMin + cameraRatio, gm.xMax - cameraRatio);
      smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
      this.transform.position = smoothPos;
  }



}
