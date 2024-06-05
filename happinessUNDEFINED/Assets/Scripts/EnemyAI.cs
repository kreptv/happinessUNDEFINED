using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 8f;
    private int currentPointIndex = 0;
    private Transform target;
    public bool playerSpotted = false;

    void Update()
    {
        if (!playerSpotted)
        {
            Patrol();
        }
        else
        {
            FollowPlayer();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0)
        {
            this.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
            return;
        }
        this.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);

        Transform patrolTarget = patrolPoints[currentPointIndex];
        float step = speed * Time.deltaTime;

        // Create a new position vector that maintains the current Y position
        Vector3 targetPosition = new Vector3(patrolTarget.position.x, transform.position.y, patrolTarget.position.z);
        Vector3 previousPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the enemy needs to flip
        if (transform.position.x < previousPosition.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.GetChild(1).transform.localScale = new Vector3(-4, transform.GetChild(1).transform.localScale.y, transform.GetChild(1).transform.localScale.z);
            transform.GetChild(2).transform.localScale = new Vector3(-1, transform.GetChild(2).transform.localScale.y, transform.GetChild(2).transform.localScale.z);
        }
        else if (transform.position.x > previousPosition.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.GetChild(1).transform.localScale = new Vector3(4, transform.GetChild(1).transform.localScale.y, transform.GetChild(1).transform.localScale.z);
            transform.GetChild(2).transform.localScale = new Vector3(1, transform.GetChild(2).transform.localScale.y, transform.GetChild(2).transform.localScale.z);
        }

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(patrolTarget.position.x, 0, patrolTarget.position.z)) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    public void SpotPlayer(Transform player)
    {
        playerSpotted = true;
        target = player;
    }

    void FollowPlayer()
    {
        if (target == null) return;

        float step = speed * Time.deltaTime;

        // Create a new position vector that maintains the current Y position
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 previousPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the enemy needs to flip
        if (transform.position.x < previousPosition.x)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            transform.GetChild(1).transform.localScale = new Vector3(-4, transform.GetChild(1).transform.localScale.y, transform.GetChild(1).transform.localScale.z);
            transform.GetChild(2).transform.localScale = new Vector3(-1, transform.GetChild(2).transform.localScale.y, transform.GetChild(2).transform.localScale.z);
        }
        else if (transform.position.x > previousPosition.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            transform.GetChild(1).transform.localScale = new Vector3(4, transform.GetChild(1).transform.localScale.y, transform.GetChild(1).transform.localScale.z);
            transform.GetChild(2).transform.localScale = new Vector3(1, transform.GetChild(2).transform.localScale.y, transform.GetChild(2).transform.localScale.z);
        }
    }

    public void LosePlayer()
    {
        playerSpotted = false;
    }
}
