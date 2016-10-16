using UnityEngine;
using System.Collections;

// Control the playing of the animation when the player is doing movements

public enum AnimationState{
	Idle,  // when not move
	Run,
	TurnLeft,
	TurnRight,
	Slide,
	Jump,
	Death

}

public class PlayerAnimation : MonoBehaviour {
	private Animation animation;
	public AnimationState animationState=AnimationState.Idle;
	private PlayerMove playerMove;

	void Awake(){
		animation = transform.Find ("Prisoner").GetComponent<Animation>();
		playerMove = this.GetComponent<PlayerMove>();
	}
		
	void Update () {  // Judge the state of the game
		if (GameController.gameState == GameState.Menu) {
			animationState = AnimationState.Idle;
		} else if (GameController.gameState == GameState.Playing) {
			animationState = AnimationState.Run;
			if (playerMove.isJumping) {
				animationState = AnimationState.Jump;
			}
			if (playerMove.targetLaneIndex > playerMove.nowLaneIndex) {
				animationState = AnimationState.TurnRight;
			}
			if (playerMove.targetLaneIndex < playerMove.nowLaneIndex) {
				animationState = AnimationState.TurnLeft;
			}
			if (playerMove.isSliding) {
				animationState = AnimationState.Slide;
			}
		} else if (GameController.gameState == GameState.End) {
			animationState = AnimationState.Death;
		}
	}

	void LateUpdate(){  // play animation
		switch (animationState) {
		case AnimationState.Idle:
			PlayIdle();
			break;
		case AnimationState.Run:
			PlayAnimation ("run");
			break;
		case AnimationState.TurnLeft:
			animation ["left"].speed = 2;//speed up, 2 times of the former speed
			PlayAnimation ("left");
			break;
		case AnimationState.TurnRight:
			animation ["right"].speed = 2;//speed up
			PlayAnimation ("right");
			break;
		case AnimationState.Slide:
			PlayAnimation ("slide");
			break;
		case AnimationState.Jump:
			PlayAnimation ("jump");
			break;
		case AnimationState.Death:
			PlayDeath();
			break;

		}
	}
	private void PlayIdle(){
		if (animation.IsPlaying ("Idle_1") == false && animation.IsPlaying ("Idle_2") == false) {//judge whether the 2 animation are playing now
			animation.Play("Idle_1");
			animation.PlayQueued("Idle_2");//put the later animation in the queue to make a animation group,when the 2 animation are played , the conditions in if is satisfied again
		}
	}

	private void PlayAnimation(string animName){
		if (animation.IsPlaying (animName) == false) {
			animation.Play (animName);
		}		
	}

	private bool havePlayDeath=false;
	private void PlayDeath(){
		if (!animation.IsPlaying ("Death") && havePlayDeath == false) {
			animation.Play ("death");
			havePlayDeath = true;
		}
	}

}
