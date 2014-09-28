using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject wallPrefab;
	public GameObject enemyPrefab;
	public GameObject friendPrefab;
	public GameObject playerPrefab;

	//public GameObject cameraObject;

	private int count;

	void Start () {
		//cameraObject = Camera.main.gameObject;
	}

	void DestroyStage () {
		GameObject stage = GameObject.FindWithTag("Stage");
		if(stage != null) {
			Destroy(stage);
		}
	}

	public int CreateStage (int stageNo) {
		TextAsset stageData = (TextAsset)Instantiate(Resources.Load("Stages/stage" + stageNo.ToString(), typeof(TextAsset)));
		float width = tilePrefab.transform.localScale.x;
		float height = tilePrefab.transform.localScale.y;
		string[] lines = stageData.text.Split("\n"[0]);
		this.DestroyStage();
		GameObject stage = new GameObject(); ;
		stage.tag = "Stage";
		stage.name = "Stage" + stageNo.ToString();
		stage.transform.parent = GameObject.Find("root").transform;
		int x = 0, y = 0;
		count = 0;
		for(y = 0; y < lines.Length; y++) {
			string line = lines[y];
			for(x = 0; x < line.Length - 1; x++) {
				Vector3 pos = new Vector3(x * width, -y * height, 0);
				char c = line[x];
				if(c != '\0') {
					/*
					 * ステージ文字解析＆ステージ生成
					 */
					GameObject tile = (GameObject)Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);
					tile.transform.parent = stage.transform;
					if(c == "#"[0]) {
						GameObject wall = (GameObject)Instantiate(wallPrefab, pos, wallPrefab.transform.rotation);
						wall.transform.parent = stage.transform;
					}
					else if(c == "o"[0]) {
						GameObject friend = (GameObject)Instantiate(friendPrefab, pos, friendPrefab.transform.rotation);
						friend.transform.parent = GameObject.Find("root").transform;
					}
					else if(c == "e"[0]) {
						GameObject enemy = (GameObject)Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
						enemy.transform.parent = GameObject.Find("root").transform;
						count++;
					}
					else if(c == "i"[0]) {
						GameObject player = (GameObject)Instantiate(playerPrefab, pos, Quaternion.identity);
						player.transform.parent = GameObject.Find("root").transform;
					}
				}
			}
		}
		// 最後の行はEOFしか無いので引く
		y--;
		//Debug.Log("y=" + y.ToString() + ", x=" + x.ToString() + ", " + count.ToString());
		/*
		cameraObject.transform.position = new Vector3((float)(x + 1f) * width / 2f, (float)(-(y - 1f)) * height / 2f, -10f);
		cameraObject.camera.orthographicSize = (y) * height / 2;
		 */
		return count;
	}
}
