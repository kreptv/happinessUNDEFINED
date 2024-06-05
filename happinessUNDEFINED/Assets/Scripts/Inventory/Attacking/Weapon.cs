using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float attackCooldown;
    public float knockback;

    private void Start()
    {
        this.GetComponent<Collider>().enabled = false;
    }

}
