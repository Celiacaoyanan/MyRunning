using UnityEngine;
using System.Collections;

// Set the camera position to catch the move of the player

public class FollowPlayer : MonoBehaviour {

	private Transform player;
	private Vector3 offset=Vector3.zero;

	public float moveSpeed=1;

	void Awake(){
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		offset = transform.position - player.position;

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetPos = player.position + offset;
		transform.position = Vector3.Lerp (transform.position, targetPos, moveSpeed * Time.deltaTime);
	}
}
