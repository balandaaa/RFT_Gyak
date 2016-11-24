﻿using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Player : Character
{
	private Rigidbody2D rb;


	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;
	Enemy enemy;
	[SerializeField]
	private Text healthText;
	public override void Start(){
		base.Start ();

		rb = GetComponent<Rigidbody2D> ();
	}
	void Update(){
		
		healthText.text=health+"/100";
		HandleInput ();
		if (transform.position.y < -1) {
			//Application.LoadLevel (Application.loadedLevel);

		}
	}
	void FixedUpdate(){
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();
	
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleAttacks ();
		ResetValues ();
	}
	private void HandleInput(){
		if (Input.GetButtonDown("Attack")) {
			attack = true;
		}
		if (Input.GetButtonDown("Jump")) {
			jump = true;
		}
	}
		
	private void HandleMovement(float horizontal){
		if (isGrounded && jump) {
			isGrounded = false;
			rb.AddForce(new Vector2(0,jumpForce));
		}
		rb.velocity = new Vector2 (horizontal * movementSpeed, rb.velocity.y);
		myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
	}
	private void HandleAttacks(){
		if (attack) {
			//WeaponCollider.enabled = true;
			if (Random.Range (0f, 1.0f) > 0.5f) {
				myAnimator.SetTrigger ("attack");

			} else {
				myAnimator.SetTrigger ("special");

			}
		} else {
			//WeaponCollider.enabled = false;
		}
	}
	private void Flip(float horizontal){
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			ChangeDirection ();
		}
	}
	private void ResetValues(){
		attack = false;
		jump = false;
	}
	private bool IsGrounded(){
		if (rb.velocity.y<=0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders[i].gameObject != gameObject) {
						return true;
					}
				}
			}
		}
		return false;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Enemy") {
			health -= 20;
			//StartCoroutine(TakeDamage ());
		}
	}
	public override bool IsDead{
		get{
			return health <= 0;
		}
	}

	public override IEnumerator TakeDamage (){
		health -= 20;
		if (!IsDead) {
			/*if (Random.Range (0f, 1.0f) > 0.5f) {
				myAnimator.SetTrigger ("attack");

			} else {
				myAnimator.SetTrigger ("special");

			}*/
		} else {
			Debug.Log ("Ork meghalt");
			yield return null;
		}
	}

}
