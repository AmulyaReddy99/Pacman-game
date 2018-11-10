using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public class Ghost : MonoBehaviour {

	public float speed = 4f;

	private Rigidbody2D rb;

	public Sprite lookLeftSprite;
	public Sprite lookRightSprite;
	public Sprite lookUpSprite;
	public Sprite lookDownSprite;
	Vector2[] destinations = new Vector2[]{
		new Vector2( 1, 29 ),
		new Vector2( 26, 29 ),
		new Vector2( 26, 1 ),
		new Vector2( 1, 1 ),
		new Vector2( 6, 16 )
	};

	public int destinationIndex;
	Vector2 moveVect;
	public SpriteRenderer sr;
	public bool isGhostBlue = false;
	public Sprite blueGhost;
	public float startWaitTime = 0;
	public float waitTimeAfterEaten = 4.0f;
	public float cellXPos = 0;
	public float cellYPos = 0;

	void Awake(){
		rb = GetComponent<Rigidbody2D> ();
		sr = gameObject.GetComponent<SpriteRenderer>();
	}

	void Start(){
		Invoke ("StartMoving", startWaitTime);
	}

	void StartMoving(){
		transform.position = new Vector2 (13.5f, 18.5f);
		float xDest = destinations[destinationIndex].x;
		if(transform.position.x > xDest){
			rb.velocity = new Vector2 (-1, 0) * speed;
		} else {
			rb.velocity = new Vector2 (1, 0) * speed;
		}

	}
	GameObject pacmanGO = null;
	public void ResetGhostAfterEaten(GameObject pacman){
		transform.position = new Vector2(cellXPos,cellYPos);
		rb.velocity = Vector2.zero;
		pacmanGO = pacman;
		Invoke ("StartMovingAfterEaten", waitTimeAfterEaten);


	}

	void StartMovingAfterEaten(){
		transform.position = new Vector2 (13.5f, 18.5f);
		float xDest = destinations[destinationIndex].x;
		if(transform.position.x > xDest){
			rb.velocity = new Vector2 (-1, 0) * speed;
		} else {
			rb.velocity = new Vector2 (1, 0) * speed;
		}
	}

	void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "Point") {
			moveVect = GetNewDirection(col.transform.position);
			transform.position = new Vector2 ((int)col.transform.position.x + .5f, 
				(int)col.transform.position.y + .5f);

			if (moveVect.x != 2) {
				if (moveVect == Vector2.right) {
					rb.velocity = moveVect * speed;
					if (!isGhostBlue) {
						sr.sprite = lookRightSprite;
					}

				} else if (moveVect == Vector2.left) {
					rb.velocity = moveVect * speed;
					if (!isGhostBlue) {
						sr.sprite = lookLeftSprite;
					}
				} else if (moveVect == Vector2.up) {
					rb.velocity = moveVect * speed;
					if (!isGhostBlue) {
						sr.sprite = lookUpSprite;
					}

				} else if (moveVect == Vector2.down) {
					rb.velocity = moveVect * speed;
					if (!isGhostBlue) {
						sr.sprite = lookDownSprite;
					}

				}
			}

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
			
	}

	Vector2 GetNewDirection(Vector2 pointVect){

		float xPos = (float)Math.Floor(Convert.ToDouble(transform.position.x));
		float yPos = (float)Math.Floor(Convert.ToDouble(transform.position.y));

		pointVect.x = (float)Math.Floor(Convert.ToDouble(pointVect.x));
		pointVect.y = (float)Math.Floor(Convert.ToDouble(pointVect.y));

		Vector2 dest = destinations[destinationIndex];

		if (pacmanGO != null) {
			dest = pacmanGO.transform.position;
		}

		if(((pointVect.x + 1) == dest.x) && ((pointVect.y + 1) == dest.y)){
			destinationIndex = (destinationIndex == 4) ? 0 : 			
				destinationIndex + 1; 

		}

		dest = destinations[destinationIndex];

		if (pacmanGO != null) {
			dest = pacmanGO.transform.position;
		}

		Vector2 newDir = new Vector2(2,0);

		Vector2 prevDir = rb.velocity.normalized;

		Vector2 oppPrevDir = prevDir * -1;

		Vector2 goRight = new Vector2(1,0);
		Vector2 goLeft = new Vector2(-1,0);
		Vector2 goUp = new Vector2(0,1);
		Vector2 goDown = new Vector2(0,-1);

		float destXDist = dest.x - xPos;
		float destYDist = dest.y - yPos;

		if (destYDist > 0 && destXDist < 0){

			if (pointVect.x == 5 && pointVect.y == 15) {

				if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				}

			} else if (destYDist > destXDist) {

				if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}


			} else if (destYDist < destXDist) {

				if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			}

		}

		// Upper Right

		if (destYDist > 0 && destXDist > 0){

			if (destYDist > destXDist) {

				if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			} else if (destYDist < destXDist) {

				if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			}
				
		}

		// Lower Right

		if (destYDist < 0 && destXDist > 0){

			if (destYDist > destXDist) {

				if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			} else if (destYDist < destXDist) {

				if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			}

		}

		// Lower Left

		if (destYDist < 0 && destXDist < 0){

			if (destYDist > destXDist) {

				if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			} else if (destYDist < destXDist) {

				if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
					newDir = goDown;
				} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
					newDir = goLeft;
				} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
					newDir = goRight;
				} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
					newDir = goUp;
				} else if (canIMoveInDirection (oppPrevDir, pointVect)) {
					newDir = oppPrevDir;
				}

			}

		}
			
		// Ys Equal and Want to go Right
		// Done because the above don't test for if Xs & Ys are equal

		if ((int)(dest.y) == (int)(yPos)
			&& destXDist > 0){

			Debug.Log ("5");

			if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
				newDir = goRight;
			} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
				newDir = goUp;
			} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
				newDir = goDown;
			} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
				newDir = goLeft;
			}

		}

		// Ys Equal and Want to go Left

		if ((int)(dest.y) == (int)(yPos)
			&& destXDist < 0){

			Debug.Log ("6");

			if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
				newDir = goLeft;
			} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
				newDir = goUp;
			} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
				newDir = goDown;
			} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
				newDir = goRight;
			}

		}

		// Xs Equal and Want to go Up

		if ((int)(dest.x) == (int)(xPos)
			&& destYDist > 0) {

			Debug.Log ("7");

			if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
				newDir = goUp;
			} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
				newDir = goRight;
			} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
				newDir = goLeft;
			} else if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
				newDir = goDown;
			}
		}
			

		// Xs Equal and Want to go Down

		if ((int)(dest.x) == (int)(xPos)
			&& destYDist < 0) {

			Debug.Log ("8");

			if (canIMoveInDirection (goDown, pointVect) && goDown != oppPrevDir) {
				newDir = goDown;
			} else if (canIMoveInDirection (goRight, pointVect) && goRight != oppPrevDir) {
				newDir = goRight;
			} else if (canIMoveInDirection (goLeft, pointVect) && goLeft != oppPrevDir) {
				newDir = goLeft;
			} else if (canIMoveInDirection (goUp, pointVect) && goUp != oppPrevDir) {
				newDir = goUp;
			}

		}
		return newDir;
	}

	bool canIMoveInDirection(Vector2 dir, Vector2 pointVect){

		// Ghost position
		Vector2 pos = transform.position;
		Transform point = GameObject.Find ("GBGrid").GetComponent<Gameboard> ().gBPoints [(int)pointVect.x, (int)pointVect.y];

		if (point != null) {

			GameObject pointGO = point.gameObject;

			Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint> ().vectToNextPoint;
			Debug.Log ("Checking Vects " + dir);
			foreach (Vector2 vect in vectToNextPoint) {
				Debug.Log ("Check " + vect);
				if (vect == dir) {
					return true;
				} 
			}
		} 
		return false;
	}

	public void TurnGhostBlue(){
		StartCoroutine (TurnGhostBlueAndBack ());
	}

	IEnumerator TurnGhostBlueAndBack(){
		isGhostBlue = true;
		sr.sprite = blueGhost;
		yield return new WaitForSeconds( 6.0f );
		isGhostBlue = false;

	}

}
