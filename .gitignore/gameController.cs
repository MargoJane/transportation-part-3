using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour {
	public static Vector3 cubePosition;
	public static float xPosition;
	public static float yPosition;
	public static int numCubesX;
	public static int numCubesY;
	public static int numCubesXLine1;

	public static int maxX;
	public static int maxY;
	public static int [,] planeGrid = new int[maxX,maxY];

	public static GameObject myCube;
	public static GameObject firstCube;
	public GameObject cubePrefab;
	public static bool airplaneActivated;

	public static GameObject airplaneCube;
	public static GameObject dropOffCube;

	public static float turnTime;
	public static float timeSpace;
	public static int cargo;
	public static int maxCargo;
	public static int minCargo;
	public static int cargoPoints;

	public Text cargoAirplaneText;
	public Text cargoScoreText;

	// Use this for initialization
	void Start () {
		airplaneActivated = false;
		timeSpace = 1.5f;
		turnTime = timeSpace;
		cargo = 0;
		maxCargo = 90;
		minCargo = 0;
		cargoPoints = 0;

		numCubesX = 16;
		numCubesY = 9;
		xPosition = -22f;
		yPosition = 8f;

		yPosition -= 2;
		xPosition = -22f;

		//makes the rows of cubes
		for (int y = 0; y < numCubesY; y++) {
			for (int x = 0; x < numCubesX; x++) {
				cubePosition = new Vector3 (xPosition, yPosition, 0);
				myCube = Instantiate (cubePrefab, cubePosition, Quaternion.identity);
				myCube.GetComponent<Renderer> ().material.color = Color.white;
				//gets the position of each cube
				myCube.GetComponent<cubeScript> ().X = x;
				myCube.GetComponent<cubeScript> ().Y = y;
				xPosition += 3;
				//makes first cube airplane cube initially
				if (x == 0 && y == 0) {
					myCube.GetComponent<Renderer> ().material.color = Color.red;
					airplaneCube = myCube;
				}
				if(x==15 && y==8){
					myCube.GetComponent<Renderer> ().material.color = Color.black;
					dropOffCube = myCube;
				}
			}
			xPosition = -22f;
			yPosition -= 2;
		}
	}

	public static void processClick(GameObject clickedCube) {

		//checks if clickCube is the same as airPlane cube
		if (airplaneCube.GetComponent<cubeScript> ().X == clickedCube.GetComponent<cubeScript> ().X && airplaneCube.GetComponent<cubeScript> ().Y == clickedCube.GetComponent<cubeScript> ().Y) {
			//deactivates the airplane if activated airplane is clicked
			if (airplaneActivated == true) {				
				airplaneActivated = false;
				airplaneCube.GetComponent<Renderer> ().material.color = Color.red;
			}
			//activates the airplane if deactivated airplane is clicked
			else if (airplaneActivated == false) {
				airplaneActivated = true;
				airplaneCube.GetComponent<Renderer> ().material.color = Color.yellow;
			}
		//moves the active airplane to a cloud cube if an airplane is active and a cloud cube is clicked, and turns the old cube back to white
		} else {
			if (airplaneActivated == true) {
				clickedCube.GetComponent<Renderer> ().material.color = Color.yellow;
				if (airplaneCube.GetComponent<cubeScript> ().X == dropOffCube.GetComponent<cubeScript> ().X && airplaneCube.GetComponent<cubeScript> ().Y == dropOffCube.GetComponent<cubeScript> ().Y) {
					airplaneCube.GetComponent<Renderer> ().material.color = Color.black;
				} else {
					airplaneCube.GetComponent<Renderer> ().material.color = Color.white;
				}
				airplaneCube = clickedCube;
			}
		}
			
	}
	

	// Update is called once per frame
	void Update (){
		//checks if airplane is in initial spot and if it has 90 cargo tons. If it's trye it adds 10 cargo turnTime seconds
		if (airplaneCube.GetComponent<cubeScript> ().X == 0 && airplaneCube.GetComponent<cubeScript> ().Y == 0) {
			if (cargo < maxCargo) {
				if (Time.time > turnTime) {
					cargo += 10;
					cargoAirplaneText.text = "Cargo: " + cargo;
					turnTime += timeSpace;
				}
			} 
		//checks if airplane is in cargo drop off area, drops off cargo if so and adds score
		} else if (airplaneCube.GetComponent<cubeScript> ().X == 15 && airplaneCube.GetComponent<cubeScript> ().Y == 8){	
			if (cargo > minCargo) {
				cargoPoints = cargoPoints + cargo;
				cargoScoreText.text = "Score: " + cargoPoints;
				cargo = 0;
				turnTime = Time.time;
			}

		} else {
			turnTime = Time.time;
		}
	}
}
