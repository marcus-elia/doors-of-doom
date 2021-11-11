using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Choosing, LeftChosen, RightChosen };

public class DoorManager : MonoBehaviour
{
    public float roomWidth_ = 10f;
    public float roomDepth_ = 10f;
    public float roomHeight_ = 10f;

    public float hallwayWidth_ = 4f;
    public float hallwayDepth_ = 10f;
    public float hallwayHeight_ = 7f;

    public Material stoneBrickUncolored;
    public Material woodDoorUncolored;
    public Material darkness;

    private GameObject doorWall_, leftDoor_, rightDoor_, leftHallway_, rightHallway_;

    private GameState currentState = GameState.Choosing;

    // Start is called before the first frame update
    void Start()
    {
        doorWall_ = new GameObject();
        doorWall_.transform.position = new Vector3(0f, 0f, 5f);
        doorWall_.AddComponent<DoorWall>();
        doorWall_.GetComponent<DoorWall>().SetMaterial(stoneBrickUncolored);
        doorWall_.GetComponent<DoorWall>().CreateFaces();

        leftDoor_ = new GameObject();
        leftDoor_.transform.position = new Vector3(-2.5f, 0f, 5f - 0.25f);
        leftDoor_.AddComponent<Door>();
        leftDoor_.GetComponent<Door>().SetMaterial(woodDoorUncolored);
        leftDoor_.GetComponent<Door>().CreateFaces();
        leftDoor_.GetComponent<Door>().SetPivot();

        rightDoor_ = new GameObject();
        rightDoor_.transform.position = new Vector3(2.5f, 0f, 5f - 0.25f);
        rightDoor_.AddComponent<Door>();
        rightDoor_.GetComponent<Door>().SetMaterial(woodDoorUncolored);
        rightDoor_.GetComponent<Door>().CreateFaces();
        rightDoor_.GetComponent<Door>().SetPivot();

        leftHallway_ = new GameObject();
        leftHallway_.transform.position = new Vector3(-2.5f, hallwayHeight_ / 2, roomDepth_ / 2 + hallwayDepth_ / 2);
        leftHallway_.AddComponent<Hallway>();
        leftHallway_.GetComponent<Hallway>().SetDimensions(hallwayWidth_, hallwayDepth_, hallwayHeight_);
        leftHallway_.GetComponent<Hallway>().SetMaterial(darkness);
        leftHallway_.GetComponent<Hallway>().CreateFaces();

        rightHallway_ = new GameObject();
        rightHallway_.transform.position = new Vector3(2.5f, hallwayHeight_ / 2, roomDepth_ / 2 + hallwayDepth_ / 2);
        rightHallway_.AddComponent<Hallway>();
        rightHallway_.GetComponent<Hallway>().SetDimensions(hallwayWidth_, hallwayDepth_, hallwayHeight_);
        rightHallway_.GetComponent<Hallway>().SetMaterial(darkness);
        rightHallway_.GetComponent<Hallway>().CreateFaces();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == GameState.Choosing)
        {
            // Toggle the left door
            if(Input.GetMouseButtonDown(0) || 
                Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) ||
                Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) ||
                Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T))
            {
                leftDoor_.GetComponent<Door>().Toggle();
            }
            // Toggle the right door
            else if(Input.GetMouseButtonDown(1) ||
                Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) ||
                Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Period) ||
                Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) ||
                Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyDown(KeyCode.Slash))
            {
                rightDoor_.GetComponent<Door>().Toggle();
            }
        }
        
        // Check if a door is open all the way
        if(leftDoor_.GetComponent<Door>().IsOpen())
        {
            currentState = GameState.LeftChosen;
            rightDoor_.GetComponent<Door>().Close();
        }
        else if(rightDoor_.GetComponent<Door>().IsOpen())
        {
            currentState = GameState.RightChosen;
            leftDoor_.GetComponent<Door>().Close();
        }
    }
}
