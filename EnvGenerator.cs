using UnityEngine;
using System.Collections;

// Generate the environment
// There are 3 types of forests, we generate environment by the joint of the three forests.
// There will only be 2 forests at the same time,one is which the player is in now, the other is which the player will be in later.
// When the player runs out of a forest, this forest will be removed, and use random number to generate the next forest

public class EnvGenerator : MonoBehaviour {

	public Forest forest1;
	public Forest forest2;
	public GameObject[] forests;
	public int forestCount=2;

	public void GenerateForest(){
		forestCount++;
		int type = Random.Range (0, 3);
		GameObject newForest = GameObject.Instantiate (forests [type], new Vector3 (0, 0, forestCount * 3000), Quaternion.identity) as GameObject;
		forest1 = forest2;
		forest2 = newForest.GetComponent<Forest> ();
	}
}
