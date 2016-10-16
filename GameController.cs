using UnityEngine;
using System.Collections;

// control the different UIs at different game states

public enum GameState{ // 3 game states
	Menu,
	Playing,
	End
}

public class GameController : MonoBehaviour {

	public static GameState gameState=GameState.Menu;
	public GameObject taptostartUI;
	public GameObject gameoverUI;

	void Update(){
		if (gameState == GameState.Menu) {
			if (Input.GetMouseButtonDown(0)) {
				gameState = GameState.Playing;
				taptostartUI.SetActive (false);
			}
		}
		if (gameState == GameState.End) {
			gameoverUI.SetActive (true);

			if (Input.GetMouseButtonDown (0)) {
				gameState = GameState.Menu;
				Application.LoadLevel (0);
			}
		}
	}
}
