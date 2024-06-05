using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    #region Singleton
    public static Attack instance;

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


    [SerializeField] private bool isWeaponEnabled;
    [HideInInspector] public Collider currentWeaponCollider;


    public void attackEnemiesInRange()
    {
        StartCoroutine(weaponEnabled());
    }

    private IEnumerator weaponEnabled()
    {
        isWeaponEnabled = true;
        currentWeaponCollider.enabled = true;
        yield return new WaitForSeconds(0.3f);
        isWeaponEnabled = false;
        currentWeaponCollider.enabled = false;
    }



}
