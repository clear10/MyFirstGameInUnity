using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private const int NUM_FRAME_ANIM = 8;	//const
	private enum PlayerVector {
		DOWN = 0,
		UP = 1,
		LEFT = 2,
		RIGHT = 3,
		UNKNOWN = 4,
	};

	public Vector3 pos;

	private Animator animator;
	private int frameCount;
	private float moveDef;
	private PlayerVector nextVector;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		if(animator == null) Debug.Log("Animator is Missing!!");
		frameCount = 0;
		moveDef = 1f / (float)NUM_FRAME_ANIM;
		nextVector = PlayerVector.UNKNOWN;

		Camera.main.gameObject.transform.parent = this.transform;
		Camera.main.gameObject.transform.localPosition = new Vector3(0f, 0f, -10f);
	}

	// Update is called once per frame
	void Update () {
		Move(ref animator, ref frameCount);
	}

	void Move (ref Animator anim, ref int frame) {

		PlayerVector playerVector = (PlayerVector)anim.GetInteger("Vector");
		bool isWalk = anim.GetBool("isWalk");
		const int N = 2;

		// First Frame
		if(frame == 0) {
			pos = transform.position;
			isWalk = true;
			if(nextVector != PlayerVector.UNKNOWN) {
				playerVector = nextVector;
				if(Input.GetKey(KeyCode.W)) {
					if(nextVector != PlayerVector.UP) isWalk = false;
				}
				else if(Input.GetKey(KeyCode.S)) {
					if(nextVector != PlayerVector.DOWN) isWalk = false;
				}
				else if(Input.GetKey(KeyCode.A)) {
					if(nextVector != PlayerVector.LEFT) isWalk = false;
				}
				else if(Input.GetKey(KeyCode.D)) {
					if(nextVector != PlayerVector.RIGHT) isWalk = false;
				}
				else {
					isWalk = false;
				}
			}
			else {
				isWalk = false;
			}
		}
		if(isWalk) {
			switch(playerVector) {
				case PlayerVector.UP:
					transform.Translate(new Vector3(0f, moveDef, 0f));
					break;
				case PlayerVector.DOWN:
					transform.Translate(new Vector3(0f, -moveDef, 0f));
					break;
				case PlayerVector.LEFT:
					transform.Translate(new Vector3(-moveDef, 0f, 0f));
					break;
				case PlayerVector.RIGHT:
					transform.Translate(new Vector3(moveDef, 0f, 0f));
					break;
			}
		}
		// Last N Frame
		if(frame >= NUM_FRAME_ANIM - N) {
			if(Input.GetKey(KeyCode.W)) {
				nextVector = PlayerVector.UP;
			}
			else if(Input.GetKey(KeyCode.S)) {
				nextVector = PlayerVector.DOWN;
			}
			else if(Input.GetKey(KeyCode.A)) {
				nextVector = PlayerVector.LEFT;
			}
			else if(Input.GetKey(KeyCode.D)) {
				nextVector = PlayerVector.RIGHT;
			}
		}

		// Update
		anim.SetInteger("Vector", (int)playerVector);
		anim.SetBool("isWalk", isWalk);
		frame = (frame + 1) % NUM_FRAME_ANIM;
	}

	void OnTriggerStay2D (Collider2D col) {
		if(col.gameObject.tag == "Wall") {
			animator.SetBool("isWalk", false);
			frameCount = 0;
			transform.position = pos;
		}
	}
}
