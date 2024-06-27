using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChairScript : MonoBehaviour
{
    public int chairID;

    public Vector3 KodaSitPosition;

    public bool canBeUsed;
    [HideInInspector] public bool isBeingSatOn;
    private GameObject ActionPopupTransform;

    public bool whenSittingFaceRight = true;

    private bool isOnCooldown;

    public bool inRange = false;



    void Start()
    {
        inRange = false;
        ActionPopupTransform = this.transform.GetChild(0).gameObject;
        ActionPopupTransform.SetActive(false);
        SetActionPopupToSit();
        isBeingSatOn = false;
        isOnCooldown = false;
    }

    void SetActionPopupToSit()
    {
        ActionPopupTransform.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Sit";
    }

    void SetActionPopupToStand()
    {
        ActionPopupTransform.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Stand";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionPopupTransform.SetActive(true);
            inRange = true;


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionPopupTransform.SetActive(false);
            inRange = false;


        }
    }




    private void Update()
    {
        if (!inRange) { return; }

        else if (Input.GetKeyDown(KeyCode.E) && canBeUsed && !isOnCooldown) // Player presses E key
        {

            if (InventoryScript.instance.TryCollectClosestItem()) { return; }

            if (!isBeingSatOn && KodaManager.instance.kodaCanMove) // sit down
            {
                isBeingSatOn = true;

                // play coroutine
                StartCoroutine(SitDown());

                SetActionPopupToStand();

            }

            else if (isBeingSatOn) // stand up
            {
                isBeingSatOn = false;

                // play coroutine

                StartCoroutine(StandUp());

                SetActionPopupToSit();

            }





        }



    }


    private IEnumerator StandUp()
    {

        PlayerMovementScript.instance.myAnimator.SetBool("sitting", false);

        KodaManager.instance.kodaCanMove = true;

        PlayerMovementScript.instance.gameObject.GetComponent<Rigidbody>().useGravity = true;

        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator SitDown()
    {
        isOnCooldown = true;

        if (whenSittingFaceRight)
        {
            PlayerMovementScript.instance.facingRight = true;
            PlayerMovementScript.instance.transform.GetChild(0).position = new Vector3(PlayerMovementScript.instance.transform.GetChild(0).position.x - 4, PlayerMovementScript.instance.transform.GetChild(0).position.y, PlayerMovementScript.instance.transform.GetChild(0).position.z);
            PlayerMovementScript.instance.transform.GetChild(0).localScale = new Vector3(50, PlayerMovementScript.instance.transform.GetChild(0).localScale.y, PlayerMovementScript.instance.transform.GetChild(0).localScale.z);
        }
        else
        {
            PlayerMovementScript.instance.facingRight = false;
            PlayerMovementScript.instance.transform.GetChild(0).position = new Vector3(PlayerMovementScript.instance.transform.GetChild(0).position.x + 4, PlayerMovementScript.instance.transform.GetChild(0).position.y, PlayerMovementScript.instance.transform.GetChild(0).position.z);
            PlayerMovementScript.instance.transform.GetChild(0).localScale = new Vector3(-50, PlayerMovementScript.instance.transform.GetChild(0).localScale.y, PlayerMovementScript.instance.transform.GetChild(0).localScale.z);
        }

        PlayerMovementScript.instance.myAnimator.SetBool("walking", false);

        PlayerMovementScript.instance.myAnimator.SetBool("sitting", true);

        KodaManager.instance.kodaCanMove = false;

        //PlayerMovementScript.instance.gameObject.transform.position = KodaSitPosition;

        PlayerMovementScript.instance.gameObject.GetComponent<Rigidbody>().useGravity = false;

       StartCoroutine(MoveOverTime());

       yield return new WaitForSeconds(0.5f);

        isOnCooldown = false;
    }



    // Coroutine to move the character over time
    private IEnumerator MoveOverTime()
    {

        Vector3 startPosition = PlayerMovementScript.instance.gameObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            PlayerMovementScript.instance.gameObject.transform.position = Vector3.Lerp(startPosition, KodaSitPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is exactly the target position
        PlayerMovementScript.instance.gameObject.transform.position = KodaSitPosition;
    }


}
