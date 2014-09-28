using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public enum UnitVector {
		UP,
		DOWN,
		LEFT,
		RIGHT,
	};

	public enum UnitAct {
		WAIT,
		MOVE,
		TURN,
	};

	public Vector3 pos;
	public int LIMMIT_BULLET = 2;
	public float SHOT_DELAY = 0.4f;
	public int currentBullet;
	public bool isShotEnable;
	public GameObject bulletPrefab;
	public UnitVector unitVector;
	public UnitAct unitAct;

	public bool isActing;

	// Use this for initialization
	void Start () {
		pos = transform.position;
		currentBullet = LIMMIT_BULLET;
		isShotEnable = true;
		unitVector = UnitVector.DOWN;
		unitAct = UnitAct.TURN;
		isActing = false;
	}

	void Update () {

		/*
		 * Up:		Z=90
		 * DOWN:	Z=270
		 * LEFT:	Z=180
		 * RIGHT:	Z=0
		 */

		float vec = transform.rotation.z;
		switch(unitVector) {
			case UnitVector.UP:
				vec = 90f;
				break;
			case UnitVector.DOWN:
				vec = 270f;
				break;
			case UnitVector.LEFT:
				vec = 180f;
				break;
			case UnitVector.RIGHT:
				vec = 0f;
				break;
		}

		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, vec));
	}

	void LateUpdate () {

		if(!isActing) {
			if(unitAct == UnitAct.WAIT) {
				isActing = true;
				StartCoroutine("SetNextAct", 3f);
			}
			else if(unitAct == UnitAct.MOVE) {
				Move();
			}
			else if(unitAct == UnitAct.TURN) {
				Turn();
				StartCoroutine("SetNextAct", 1f);
			}
		}

		if(SearchUnit()) {
			Shot();
		}
	}

	IEnumerator SetNextAct (float interval) {
		yield return new WaitForSeconds(interval);
		unitAct = (UnitAct)Mathf.RoundToInt(Random.Range(0f, 2.5f));
		isActing = false;
	}

	void Move () {
		isActing = true;
		Vector2 direction = Vector2.right;
		StartCoroutine("MoveCoroutine", direction);
	}

	IEnumerator MoveCoroutine (Vector2 direction) {
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		yield return 0;
		transform.Translate(direction / 8);
		pos = transform.position;
		StartCoroutine("SetNextAct", 1f);
	}

	void Turn () {
		isActing = true;
		UnitVector tmp;
		do {
			tmp = (UnitVector)Mathf.RoundToInt(Random.Range(0f, 1f) * 3);
		} while(tmp == unitVector);
		unitVector = tmp;
	}

	void Shot () {
		if(!isShotEnable) return;
		if(currentBullet <= 0) return;
		Vector3 ofs = Vector3.zero;
		switch(unitVector) {
			case UnitVector.UP:
				ofs = new Vector3(0f, 1f, 0);
				break;
			case UnitVector.DOWN:
				ofs = new Vector3(0f, -1f, 0);
				break;
			case UnitVector.LEFT:
				ofs = new Vector3(-1f, 0f, 0);
				break;
			case UnitVector.RIGHT:
				ofs = new Vector3(1f, 0f, 0);
				break;
		}

		GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position + ofs, Quaternion.identity);
		bullet.GetComponent<Bullet>().Initialize(ofs, this.gameObject);
		currentBullet--;
	}
	public void FillUpBullet () {
		currentBullet++;
	}

	bool SearchUnit () {

		Vector2 direction = Vector2.zero;
		switch(unitVector) {
			case UnitVector.UP:
				direction = Vector2.up;
				break;
			case UnitVector.DOWN:
				direction = -Vector2.up;
				break;
			case UnitVector.LEFT:
				direction = -Vector2.right;
				break;
			case UnitVector.RIGHT:
				direction = Vector2.right;
				break;
		}

		RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y), direction);
		//Debug.Log(hit.collider);
		if(hits.Length>0) {
			int n = hits.Length;
			for(int i = 0; i < n; i++) {
				if(hits[i].transform.gameObject.tag != this.gameObject.tag) {
					if(hits[i].transform.gameObject.tag != "Wall" && hits[i].transform.gameObject.tag != "Player" && hits[i].transform.gameObject.tag != "Bullet") {
						//Debug.Log("found");
						return true;
					}
				}
			}
		}

		return false;		// Not found!
	}

	// Trigger
	void OnTriggerStay2D (Collider2D col) {
		if(col.gameObject.tag == "Wall") {
			transform.position = pos;
			StopCoroutine("MoveCoroutine");
			StartCoroutine("SetNextAct", 0f);
		}
	}
}
