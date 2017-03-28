﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BatController : MonoBehaviour 
{
	private Rigidbody2D bRigidBody;
	private Animator bAnimator;
	private SpriteRenderer bRenderer;

	private BatGrapple bGrapple;
	private BatFeet bFeet;
	private BatInput bInput;

	private enum Stance {Normal, Idle, Run, Jump, Fall, Glide, Grapple};
	private Stance batStance;

	private float maxGrappleSpeed = 20f;

	private float moveSpeed = 4f;
	private float jumpForce = 6.5f;

	public Vector2 batDirection {get; private set;}

	void OnEnable()
	{
		bFeet = GetComponentInChildren<BatFeet> ();
		bRigidBody = GetComponent<Rigidbody2D> ();
		bAnimator = GetComponent<Animator> ();

		bRenderer = GetComponent<SpriteRenderer> ();
		bGrapple = GetComponent<BatGrapple>();
		bInput = GetComponent<BatInput> ();
	}

	void Start () 
	{
		batStance = Stance.Normal;
		batDirection = Vector2.right;
	}

	void Update () 
	{
		UpdateBatStance();
		UpdateMovement();
		UpdateAnimation ();
	}

	void UpdateBatStance()
	{
//		if(bInput.jump && batStance != Stance.Grapple)
//		{
//			if(bGrapple.Shoot())
//			{
//				batStance = Stance.Grapple;
//				return;
//			}
//		}
//
//		if(batStance == Stance.Grapple)
//		{
//			return;
//		}

		if(bInput.glide && batStance == Stance.Glide && !bFeet.isGrounded)
		{
			return;
		}

		if(bInput.glide && !bFeet.isGrounded)
		{
			batStance = Stance.Glide;
			bRigidBody.velocity = new Vector2(0, 0);
			return;
		}

		batStance = Stance.Normal;
	}

	void UpdateMovement()
	{
		bRigidBody.gravityScale = 2;

		if(batStance == Stance.Normal)
		{
			//bRigidBody.velocity = new Vector2 (moveSpeed, bRigidBody.velocity.y);

			if(bInput.jump)
			{
				if(bFeet.isGrounded)
				{
					bRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
				}
				else if(!bFeet.isGrounded && bFeet.canDoubleJump)
				{
					bRigidBody.velocity = new Vector2(0,0);
					bRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
					bFeet.canDoubleJump = false;
				}
			}
			return;
		}

		if(batStance == Stance.Glide)
		{
			bRigidBody.gravityScale = 0.25f;
			bRigidBody.velocity = new Vector2(0, bRigidBody.velocity.y);
		}
	}

	void GrappleEnded()
	{
		if(batStance == Stance.Grapple)
		{
			batStance = Stance.Normal;
			Debug.Log(bRigidBody.velocity.y);
			bRigidBody.velocity = new Vector2(bRigidBody.velocity.x, Mathf.Clamp(bRigidBody.velocity.y + 1, 0, maxGrappleSpeed));
		}
	}

	void UpdateAnimation() 
	{		
		bAnimator.SetBool ("isGrounded", bFeet.isGrounded);
		bAnimator.SetFloat ("velocityX", 1);
		bAnimator.SetFloat ("velocityY", bRigidBody.velocity.y);
		bAnimator.SetFloat("batGravity", bRigidBody.gravityScale);
		bAnimator.SetBool ("isGrappling", batStance == Stance.Grapple);

		//UpdateDirection
//		if (bRigidBody.velocity.x > 0) 
//		{
//			bRenderer.flipX = false;
//			batDirection = Vector2.right;
//		}
//		else if (bRigidBody.velocity.x < 0) 
//		{
//			bRenderer.flipX = true;
//			batDirection = Vector2.left;
//		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag == "danger")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.gameObject.tag == "BoundBottom")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}