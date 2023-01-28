using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;

public class PlayerMovementScript : MonoBehaviour
{
      public GameManager gm;
      public CubismModel playerModel;

      float speed = 7.0f;
      Rigidbody2D body;
      //public Animator animator;

      public bool grounded;
      public bool facingRight;


      void Awake()
      {

        body = transform.GetComponent<Rigidbody2D>();
        facingRight = true;
      }

      void LateUpdate(){

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dir = (Input.mousePosition - pos).normalized;

      }

      // Update is called once per frame
      void FixedUpdate()
      {
        if (gm.bindMovement){return;}


        if (Input.GetAxis("Horizontal") > 0.1 ){
          body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
          //animator.SetBool("IsRunning", true);
          if (!facingRight){
            facingRight = true;
            transform.localScale = new Vector3(1,1,1);
          }
        } else if (Input.GetAxis("Horizontal") < -0.1){
          body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);
          //animator.SetBool("IsRunning", true);
            if (facingRight){
              facingRight = false;
              transform.localScale = new Vector3(-1,1,1);
            }
        }
        else {
          //animator.SetBool("IsRunning", false);
        }

          if (Input.GetKey(KeyCode.Space) && grounded){
              body.velocity = new Vector2(body.velocity.x, speed);
              grounded = false;
              //animator.SetBool("IsJumping", true);
          }
      }

      // If player is on the ground, you can jump
      private void OnCollisionEnter2D(Collision2D collision)
      {
          if (collision.gameObject.tag == "Ground")
              grounded = true;
              //animator.SetBool("IsJumping", false);
      }

}
