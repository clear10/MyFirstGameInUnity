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
	public int LIMMIT_BULLET = 3;
	public float SHOT_DELAY = 0.2f;
	public int currentBullet;
	public bool isShotEnable;
	public GameObject bulletPrefab;

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
		currentBullet = LIMMIT_BULLET;
		isShotEnable = true;

		Camera.main.gameObject.transform.parent = this.transform;
		Camera.main.gameObject.transform.localPosition = new Vector3(0f, 0f, -10f);
	}

	// Update is called once per frame
	void Update () {
		Move(ref animator, ref frameCount);
		Shot();
	}

	// Move
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

	// Shot
	void Shot () {
		if(!isShotEnable) return;
		if(!Input.GetKey(KeyCode.Space)) return;
		if(currentBullet > 0) {
			int vec = animator.GetInteger("Vector");
			Vector3 ofs = Vector3.zero;
			switch(vec) {
				case 0:
					ofs = new Vector3(0f, -1f, 0);
					break;
				case 1:
					ofs = new Vector3(0f, 1f, 0);
					break;
				case 2:
					ofs = new Vector3(-1f, 0, 0);
					break;
				case 3:
					ofs = new Vector3(1f, 0, 0);
					break;
			}
			/*GameObject bullet = (GameObject)*/
			
			GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + ofs, Quaternion.identity);
			bullet.GetComponent<Bullet>().Initialize(ofs, this.gameObject);
			//bullet.transform.parent = this.transform;
			currentBullet--;
			isShotEnable = false;
			StartCoroutine("ShotInterval");
		}
	}

	IEnumerator ShotInterval () {
		yield return new WaitForSeconds(SHOT_DELAY);
		isShotEnable = true;
	}

	public void FillUpBullet () {
		currentBullet++;
	}

	// Trigger
	void OnTriggerStay2D (Collider2D col) {
		if(col.gameObject.tag == "Wall") {
			animator.SetBool("isWalk", false);
			frameCount = 0;
			transform.position = pos;
		}
	}
}
