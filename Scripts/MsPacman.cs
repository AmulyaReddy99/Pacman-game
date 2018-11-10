using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MsPacman : MonoBehaviour {
	public float speed = 0.4f;
	private Rigidbody2D rb;
	public Sprite pausedSprite;
	SoundManager soundManager;
	public AudioClip eatingGhost;
	public AudioClip pacmanDies;
	public AudioClip powerupEating;

	Gameboard gameBoard;
	Ghost redGhostScript;
	Ghost pinkGhostScript;
	Ghost blueGhostScript;
	Ghost orangeGhostScript;

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		gameBoard = FindObjectOfType(typeof(Gameboard)) as Gameboard; 
		GameObject redGhostGO = GameObject.Find ("RedGhost");
		GameObject pinkGhostGO = GameObject.Find ("PinkGhost");
		GameObject blueGhostGO = GameObject.Find ("BlueGhost");
		GameObject orangeGhostGO = GameObject.Find ("OrangeGhost");

		redGhostScript = (Ghost) redGhostGO.GetComponent(typeof(Ghost));
		pinkGhostScript = (Ghost) pinkGhostGO.GetComponent(typeof(Ghost));
		blueGhostScript = (Ghost) blueGhostGO.GetComponent(typeof(Ghost));
		orangeGhostScript = (Ghost) orangeGhostGO.GetComponent(typeof(Ghost));
	}

	void Start(){
		rb.velocity = new Vector2 (-1, 0) * speed;
		soundManager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	void FixedUpdate(){

		float horzMove = Input.GetAxisRaw ("Horizontal");
		float vertMove = Input.GetAxisRaw ("Vertical");
		Vector2 moveVect;

		var localVelocity = transform.InverseTransformDirection(rb.velocity);

		if (Input.GetKeyDown ("a")) {
			if (localVelocity.x > 0 && gameBoard.IsValidSpace(transform.position.x - 1,transform.position.y)) {

				moveVect = new Vector2 (horzMove, 0);
				transform.position = new Vector2 ((int)transform.position.x + .5f, 
					(int)transform.position.y + .5f);
				rb.velocity = moveVect * speed;
				transform.localScale = new Vector2 (1, 1);
				transform.localRotation = Quaternion.Euler (0, 0, 0);
			} else {
				moveVect = new Vector2 (horzMove, 0);

				if (canIMoveInDirection (moveVect)) {

					transform.position = new Vector2 ((int)transform.position.x + .5f, 
						(int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;
					transform.localScale = new Vector2 (1, 1);
					transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
			}


		} else if (Input.GetKeyDown ("d")) {
			if (localVelocity.x < 0 && gameBoard.IsValidSpace(transform.position.x + 1,transform.position.y)) {

				moveVect = new Vector2 (horzMove, 0);

				transform.position = new Vector2 ((int)transform.position.x + .5f, 
					(int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (-1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 0);

			} else {
				moveVect = new Vector2 (horzMove, 0);
				if (canIMoveInDirection (moveVect)) {
					transform.position = new Vector2 ((int)transform.position.x + .5f, 
						(int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;
					transform.localScale = new Vector2 (-1, 1);
					transform.localRotation = Quaternion.Euler (0, 0, 0);
				}
			}
				
		} else if (Input.GetKeyDown ("w")){
			if (localVelocity.y > 0 && gameBoard.IsValidSpace(transform.position.x,transform.position.y + 1)) {

				moveVect = new Vector2 (0, vertMove);
				transform.position = new Vector2 ((int)transform.position.x + .5f, 
					(int)transform.position.y + .5f);
				rb.velocity = moveVect * speed;
				transform.localScale = new Vector2 (1, 1);
				transform.localRotation = Quaternion.Euler (0, 0, 270);

			} else {

				moveVect = new Vector2 (0, vertMove);

				if (canIMoveInDirection (moveVect)) {

					transform.position = new Vector2 ((int)transform.position.x + .5f, 
						(int)transform.position.y + .5f);
					rb.velocity = moveVect * speed;
					transform.localScale = new Vector2 (1, 1);
					transform.localRotation = Quaternion.Euler (0, 0, 270);

				}
			}

		} else if (Input.GetKeyDown ("s")){
			if (localVelocity.y < 0 && gameBoard.IsValidSpace(transform.position.x,transform.position.y - 1)) {
				moveVect = new Vector2 (0, vertMove);

				transform.position = new Vector2 ((int)transform.position.x + .5f, 
					(int)transform.position.y + .5f);

				rb.velocity = moveVect * speed;

				transform.localScale = new Vector2 (1, 1);

				transform.localRotation = Quaternion.Euler (0, 0, 90);

			} else {

			moveVect = new Vector2 (0, vertMove);

				if (canIMoveInDirection (moveVect)) {

					transform.position = new Vector2 ((int)transform.position.x + .5f, 
						(int)transform.position.y + .5f);

					rb.velocity = moveVect * speed;

					transform.localScale = new Vector2 (1, 1);

					transform.localRotation = Quaternion.Euler (0, 0, 90);
				}
			} 
		}
		UpdateEatingAnimation();

		if (transform.position.y == 2.5) {
			transform.position = new Vector2 (transform.position.x, 3.5f);
		}

	}

	bool canIMoveInDirection(Vector2 dir){

		Vector2 pos = transform.position;

		Transform point = GameObject.Find ("GBGrid").GetComponent<Gameboard> ().gBPoints [(int)pos.x, (int)pos.y];

		if (point != null) {

			GameObject pointGO = point.gameObject;

			Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint> ().vectToNextPoint;

			foreach (Vector2 vect in vectToNextPoint) {
				if (vect == dir) {
					return true;
				} 
			}
		} 
		return false;
	}
	void OnTriggerEnter2D(Collider2D col){

		bool hitAWall = false;

		if (col.gameObject.tag == "Point") {

			Vector2[] vectToNextPoint = col.GetComponent<TurningPoint> ().vectToNextPoint;
			if (Array.Exists (vectToNextPoint, element => element == rb.velocity.normalized)) {
				hitAWall = false;
			} else {
				hitAWall = true;
			}
			transform.position = new Vector2 ((int)col.transform.position.x + .5f, 
				(int)col.transform.position.y + .5f);

			if (hitAWall) 
				rb.velocity = Vector2.zero;		

		}
		if (col.gameObject.tag == "Pill") {

			SoundManager.Instance.PlayOneShot (SoundManager.Instance.powerupEating);
			redGhostScript.TurnGhostBlue ();
			pinkGhostScript.TurnGhostBlue ();
			blueGhostScript.TurnGhostBlue ();
			orangeGhostScript.TurnGhostBlue ();

			IncreaseTextUIScore (50);

			Destroy(col.gameObject);

		}
			
		Vector2 pmMoveVect = new Vector2(0,0);

		if (transform.position.x < 2 && transform.position.y == 15.5) {
			transform.position = new Vector2 (24.5f, 15.5f);
			pmMoveVect = new Vector2(-1,0);
			rb.velocity = pmMoveVect * speed;
		} else if (transform.position.x > 25 && transform.position.y == 15.5) {
			transform.position = new Vector2 (2f, 15.5f);
			pmMoveVect = new Vector2(1,0);
			rb.velocity = pmMoveVect * speed;
		}
			
		if (col.gameObject.tag == "Dot") {
			ADotWasEaten (col);
		}

		if (col.gameObject.tag == "Ghost") {
			String ghostName = col.GetComponent<Collider2D>().gameObject.name;
			AudioSource audioSource = soundManager.GetComponent<AudioSource>();
			if (ghostName == "RedGhost") {
				if (redGhostScript.isGhostBlue) {
					redGhostScript.ResetGhostAfterEaten (gameObject);
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.eatingGhost);
					IncreaseTextUIScore (400);
				} else {
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.pacmanDies);
					audioSource.Stop ();
					Destroy (gameObject);
				}
			} else if (ghostName == "PinkGhost") {
				if (pinkGhostScript.isGhostBlue) {
					pinkGhostScript.ResetGhostAfterEaten (gameObject);
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.eatingGhost);
					IncreaseTextUIScore (400);
				} else {
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.pacmanDies);
					audioSource.Stop ();
					Destroy (gameObject);
				}
			} else if (ghostName == "BlueGhost") {
				if (blueGhostScript.isGhostBlue) {
					blueGhostScript.ResetGhostAfterEaten (gameObject);
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.eatingGhost);
					IncreaseTextUIScore (400);
				} else {
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.pacmanDies);
					audioSource.Stop ();
					Destroy (gameObject);
				}
			} else if (ghostName == "OrangeGhost") {
				if (orangeGhostScript.isGhostBlue) {
					orangeGhostScript.ResetGhostAfterEaten (gameObject);
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.eatingGhost);
					IncreaseTextUIScore (400);
				} else {
					SoundManager.Instance.PlayOneShot (SoundManager.Instance.pacmanDies);
					audioSource.Stop ();
					Destroy (gameObject);
				}
			}

		}

	}

	void UpdateEatingAnimation(){
		if (rb.velocity == Vector2.zero) {
			GetComponent<Animator>().enabled = false;
			GetComponent<SpriteRenderer>().sprite = pausedSprite;

			soundManager.PausePacman();

		} else {
			GetComponent<Animator>().enabled = true;
			soundManager.UnPausePacman();
		}
	}

	void ADotWasEaten(Collider2D col){
		IncreaseTextUIScore (10);
		Destroy (col.gameObject);
	}
	void IncreaseTextUIScore(int points){
		Text textUIComp = GameObject.Find("Score").GetComponent<Text>();
		int score = int.Parse(textUIComp.text);
		score += points;
		textUIComp.text = score.ToString();
	}

}
	