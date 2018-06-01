using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speedBoost;
    public float jumpSpeed;
    public Transform feet;
    public float boxWidth = 1f;
    public float boxHeight = 1f;
    public bool isGrounD = false;
    public LayerMask WhatIsground;
    private bool conDoubleJump = false;





    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isJumping = false;
   
    
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        isGrounD = Physics2D.OverlapBox(feet.position, new Vector2(boxWidth, boxHeight), 360.0f, WhatIsground);
        float moveSpeed =
            Input.GetAxisRaw("Horizontal");
        if(moveSpeed != 0)
        {
            MoveHor(moveSpeed);
        }
        else
        {
            StopMoving();
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }		
        if(rb.velocity.y<0)
        {
            showFalling();
        }
	}
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(feet.position, new Vector3(boxWidth, boxHeight, 0f));
            
    }
    private void showFalling()
    {

        anim.SetInteger("State", 3);
    }

    private void Jump()
    {
        //Debug.Log("Jump");
        if (isGrounD)
        {
            isJumping = true;
            rb.AddForce(new Vector2(0, jumpSpeed));
            anim.SetInteger("State", 2);
            conDoubleJump = true;
        }   
        else
        {
            if(conDoubleJump)
            {
                conDoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(new Vector2(0, jumpSpeed));
                anim.SetInteger("State", 2);

            }
        }
    }

    private void MoveHor(float speed)
    {
        if (speed > 0)
            sr.flipX = false;
        else if (speed < 0)
            sr.flipX = true;

        rb.velocity =
            new Vector2(speed * speedBoost, rb.velocity.y);
        if(!isJumping)
        anim.SetInteger("State", 1);
    }

    private void StopMoving()
    {
        rb.velocity = 
            new Vector2(0, rb.velocity.y);
        if (isJumping == false)
        {
            anim.SetInteger("State", 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrounD"))
            isJumping = false;
    }
}
