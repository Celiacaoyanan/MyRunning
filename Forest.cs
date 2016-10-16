using UnityEngine;
using System.Collections;

public class Forest : MonoBehaviour {

	public GameObject[] obstacles;
	public float startLength=50;
	public float minLength=200;  // the minimum distance between 2 obstacles
	public float maxLength=400; // the maximum distance between 2 obstacles

	private Transform player;
	private WayPoints wayPoints;
	private int targetWayPointIndex;
	private EnvGenerator envGenerator;

	void Awake(){
		player = GameObject.FindGameObjectWithTag (Tags.player).transform;
		wayPoints = transform.Find ("waypoints").GetComponent<WayPoints> ();
		targetWayPointIndex = wayPoints.points.Length - 2;
		envGenerator = Camera.main.GetComponent<EnvGenerator>();
	}

	void Start(){
		GenerateObstacle ();
	}
		
	
	void Update () {

	}

	void GenerateObstacle(){
		float startZ = transform.position.z-3000;
		float endZ = startZ + 3000;
		float z = startZ+startLength;
		while (true) {
			z += Random.Range (200, 400);
			if (z > endZ) {  // beyond borders
				break;
			}  else {
				Vector3 position = GetWayPosByZ (z);
				//generate obstacles
				int obsIndex=Random.Range(0,obstacles.Length);
				GameObject go=GameObject.Instantiate (obstacles [obsIndex],position,Quaternion.identity) as GameObject;
				go.transform.parent = this.transform;
			}
		}

	}
	Vector3 GetWayPosByZ(float z){

		Transform[] points = wayPoints.points;
		int index = 0;
		for (int i = 0; i < points.Length - 1; i++) {
			if (z <= points [i].position.z && z >= points [i + 1].position.z) {
				index = i;
				break;
			}
		}
		return Vector3.Lerp(points[index+1].position,points[index].position,(z-points[index+1].position.z)/(points[index].position.z-points[index+1].position.z));

	}

	public Vector3 GetNextTargetPoint(){
		while(true){
		if (wayPoints.points [targetWayPointIndex].position.z - player.position.z < 10) {
			targetWayPointIndex--;
				if (targetWayPointIndex < 0) {
					envGenerator.GenerateForest ();
					Destroy (this.gameObject, 2);
					return envGenerator.forest1.GetNextTargetPoint ();
				}
		} else {
			return wayPoints.points [targetWayPointIndex].position;
		}
	}
}
}
