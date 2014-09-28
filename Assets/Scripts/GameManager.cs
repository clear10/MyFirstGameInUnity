using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public MapLoader mapLoader;
	public int count;

	private bool isGUI;
	private bool isClear;

	// Use this for initialization
	void Start () {
		mapLoader = GetComponent<MapLoader>();
		count = mapLoader.CreateStage(0);
		isGUI = false;
		isClear = false;
	}

	// Update is called once per frame
	void Update () {

	}

	void ShotEnemy () {
		count--;
		if(count <= 0) GameClear();
	}

	void GameClear () {
		isGUI = true;
		isClear = true;
	}

	void GameOver () {
		isGUI = true;
		isClear = false;
	}
	
	void OnGUI () {

		if(isGUI) {
			if(isClear) {
				GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "GameClear");
				if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 15, 100, 50), "Tweet")) {
					string text = "SnowFight!!をくりあ！";
					Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(text));
				}
			}
			else {
				GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "GameOver");
				if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 35, 100, 50), "Tweet")) {
					string text = "コレムズすぎ！絶対くりあできないよー！";
					Application.OpenURL("http://twitter.com/intent/tweet?text=" + WWW.EscapeURL(text));
				}
				if(GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 90, 100, 50), "Retry")) {
					Application.LoadLevel(Application.loadedLevel);
				}
			}
		}
	}
}
