using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float FORCE_FACTOR = 10f;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player");
		int vec = player.GetComponent<Animator>().GetInteger("Vector");
		Vector2 forceVec = Vector3.zero;
		switch(vec) {
			case 0:
				forceVec = new Vector2(0f, -1f);
				break;
			case 1:
				forceVec = new Vector2(0f, 1f);
				break;
			case 2:
				forceVec = new Vector2(-1f, 0);
				break;
			case 3:
				forceVec = new Vector2(1f, 0);
				break;
		}
		rigidbody2D.AddForce(forceVec * FORCE_FACTOR, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.identity;
		
	}

	// Trigger
	void OnTriggerStay2D (Collider2D col) {
		if(col.gameObject.tag == "Wall") {
			Destroy(this.gameObject);
			player.GetComponent<Player>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
		}
	}
}
