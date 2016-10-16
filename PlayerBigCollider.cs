using UnityEngine;
using System.Collections;

public class PlayerBigCollider : MonoBehaviour {

	private PlayerAnimation playerAnimation;

	void Awake(){
		playerAnimation = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<PlayerAnimation >();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == Tags.obstacles && GameController.gameState == GameState.Playing && playerAnimation.animationState != AnimationState.Slide) {
			GameController.gameState = GameState.End;
		}
	}
}
