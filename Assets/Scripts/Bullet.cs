using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float FORCE_FACTOR = 10f;

	public GameObject parent;


	// Use this for initialization
	void Start () {
	}

	public void Initialize (Vector2 force, GameObject parent) {
		this.parent = parent;
		rigidbody2D.AddForce(force * FORCE_FACTOR, ForceMode2D.Impulse);
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.identity;

	}

	// Trigger
	void OnTriggerEnter2D (Collider2D col) {
		if(col.gameObject.tag == "Wall") {
			if(parent.tag == "Player") parent.GetComponent<Player>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			if(parent.tag == "Friend") parent.GetComponent<Friend>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			if(parent.tag == "Enemy") parent.GetComponent<Enemy>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			Destroy(this.gameObject);
		}
		else if(col.gameObject.tag == "Enemy") {
			if(parent.tag == "Player") {
				Destroy(col.gameObject);
				Destroy(this.gameObject);
				parent.GetComponent<Player>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
				GameObject.FindWithTag("GameController").GetComponent<GameManager>().SendMessage("ShotEnemy", SendMessageOptions.DontRequireReceiver);
			}
			else if(parent.tag == "Friend") {
				Destroy(col.gameObject);
				Destroy(this.gameObject);
				parent.GetComponent<Friend>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
				GameObject.FindWithTag("GameController").GetComponent<GameManager>().SendMessage("ShotEnemy", SendMessageOptions.DontRequireReceiver);
			}
			//if(this.parent.tag == "Enemy") parent.GetComponent<Enemy>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
		}
		else if(col.gameObject.tag == "Friend") {
			//if(parent.tag == "Player") parent.GetComponent<Player>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			//if(parent.tag == "Friend") parent.GetComponent<Friend>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			if(parent.tag == "Enemy") {
				Destroy(col.gameObject);
				Destroy(this.gameObject);
				parent.GetComponent<Enemy>().SendMessage("FillUpBullet", SendMessageOptions.DontRequireReceiver);
			}
		}
		else if(col.gameObject.tag == "Player") {
			if(parent.tag == "Enemy") {
				GameObject.FindWithTag("GameController").GetComponent<GameManager>().SendMessage("GameOver", SendMessageOptions.DontRequireReceiver);
				Destroy(col.gameObject);
				Destroy(this.gameObject);
			}
		}
	}
}
