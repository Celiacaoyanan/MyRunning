using UnityEngine;
using System.Collections;
using System.IO.Ports;

// Control the movements of the player

public enum MoveDir{ //move direction of the player
	None,
	Left,
	Right,
	Top,
	Bottom
}

public class PlayerMove : MonoBehaviour {

	public float moveSpeed=100;
	public float moveHorizontalSpeed=3;
	public bool isSliding=false;
	public float slideTime=1.4f;
	public bool isJumping=false;
	public float jumpHeight=20;
	public float jumpSpeed=20;
	public int nowLaneIndex = 1;//there are 3 lanes , number them "0,1,2", the default lane is No.1
	public int targetLaneIndex = 1;//the target lane the player will move to

	private bool isUp=true;
	private float slideTimer=0;
	private EnvGenerator envGenerator;
	private MoveDir moveDir = MoveDir.None;
	private Vector3 lastMouseDown=Vector3.zero;
	private float[]xOffset=new float[3]{-14,0,14};
	private float moveHorizontal = 0;//offsets on x-axis
	private Transform prisoner; 
	private float haveJumpHeight = 0;

	SerialPort stream = new SerialPort ("COM1", 9600);




	void Awake(){
		envGenerator = Camera.main.GetComponent<EnvGenerator> ();
		prisoner = this.transform.Find ("Prisoner").transform ;

		stream.Open ();
	}
	

	void Update () {
		if(GameController.gameState==GameState.Playing){
		Vector3 targetPos = envGenerator.forest1.GetNextTargetPoint ();
			targetPos = new Vector3 (targetPos.x + xOffset [targetLaneIndex], targetPos.y, targetPos.z);
		Vector3 moveDir = targetPos - transform.position;
		transform.position+=moveDir.normalized*moveSpeed*Time.deltaTime;

		MoveControl ();

	}
  }

	private void MoveControl(){
		MoveDir dir = GetMoveDir();

		if (targetLaneIndex != nowLaneIndex) {//when the target lane isnt the current lane, then the player will move
			float moveLength=Mathf.Lerp(0,moveHorizontal,moveHorizontalSpeed*Time.deltaTime);//use interpolation operation to get the distance to move
			transform.position=new Vector3(transform.position.x+moveLength,transform.position.y,transform.position.z);//get the current position, x add offset,y and z keep the same
			moveHorizontal-=moveLength;// minus the  moved distance
			if(Mathf.Abs (moveHorizontal) <0.5f){//check the remaining distance, if its very small, then we suppose the player has arrived
				transform.position = new Vector3 (transform.position.x + moveHorizontal, transform.position.y, transform.position.z);
				moveHorizontal = 0;
				nowLaneIndex = targetLaneIndex;
			}
		}

		if (isSliding) {
			slideTimer += Time.deltaTime;
			if (slideTimer > slideTime) {//timing is over
				slideTimer=0;//timer reset to 0
				isSliding=false;
			}
				
		}

		if (isJumping) {
			float yMove = jumpSpeed * Time.deltaTime;
			if (isUp) {
				prisoner.position = new Vector3 (prisoner.position.x, prisoner.position.y + yMove, prisoner.position.z);
				haveJumpHeight += yMove;
				if (Mathf.Abs (jumpHeight - haveJumpHeight) < 0.5f) { //if the distance between jumpHeight and haveJumpHeight is smaller than 0.5f, then we suppose that the player has arrived the target height
					prisoner.position = new Vector3 (prisoner.position.x, prisoner.position.y + jumpHeight - haveJumpHeight, prisoner.position.z);
					isUp = false;
					haveJumpHeight = jumpHeight;
				}
			} else {
				prisoner.position = new Vector3 (prisoner.position.x, prisoner.position.y - yMove, prisoner.position.z);
				haveJumpHeight -= yMove;
				if (Mathf.Abs (haveJumpHeight-0) < 0.5f) {
					prisoner.position = new Vector3 (prisoner.position.x, prisoner.position.y -( haveJumpHeight-0), prisoner.position.z);
					isJumping  = false;
					haveJumpHeight = 0;
				}
			}
	}
}
		


	MoveDir GetMoveDir(){

		string value = stream.ReadLine ();

		if (value=="Right") {
					if (targetLaneIndex < 2) {//only in lane 0 and lane 1, the player can move right
						targetLaneIndex++;
						moveHorizontal = 14;//the offset between adjacent lanes is 14
					}
			        stream.BaseStream.Flush();
					return MoveDir.Right;
		} else if (value=="Left") {
					if (targetLaneIndex >0) {
						targetLaneIndex--;
						moveHorizontal = -14;
					}
					return MoveDir.Left;
		} else if (value=="Jump") {
					if (isJumping == false) {
						isJumping = true;
						isUp = true;
						haveJumpHeight = 0;
					}
					return MoveDir.Top;
		} else if (value=="S"){
					isSliding = true; 
					slideTimer = 0;
					return MoveDir.Bottom;
				}
			
		return MoveDir.None;
	}
	/*MoveDir GetMoveDir(){

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (targetLaneIndex < 2) {
				targetLaneIndex++;
				moveHorizontal = 14;
			}
			return MoveDir.Right;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (targetLaneIndex >0) {
				targetLaneIndex--;
				moveHorizontal = -14;
			}
			return MoveDir.Left;
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (isJumping == false) {
				isJumping = true;
				isUp = true;
				haveJumpHeight = 0;
			}
			return MoveDir.Top;
		} else if (Input.GetKeyDown (KeyCode.DownArrow)){
			isSliding = true; 
			slideTimer = 0;
			return MoveDir.Bottom;
		}

		return MoveDir.None;
	}	*/


}