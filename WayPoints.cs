using UnityEngine;
using System.Collections;

// Add waypoints to the three forests and use plug-in iTween to concatenate all the waypoints
// The movements of the player and the generation of the obstacles are realized by locating these waypoints

public class WayPoints : MonoBehaviour {

	public Transform[] points;
	void OnDrawGizmos() {
		iTween.DrawPath (points);
	}

}
