using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Region : MonoBehaviour
{
        public string regionName;
        [SerializeField] public float size;
        
        [HideInInspector] public PolygonCollider2D regionCollider;


        private void Awake()
        {
            regionCollider = this.gameObject.transform.GetComponent<PolygonCollider2D>();
        }

        void ShowRegion()
        {
            regionCollider.gameObject.SetActive(true);
        } // Show Region

        void HideRegion()
        {
            regionCollider.gameObject.SetActive(false);
        } // Hide Region

        public void UpdateCameraSize()
        {
            CameraScript.UpdateSize(size);
        } // Update camera size

}
