using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Choosing, PlayerMoveLeft, PlayerMoveRight, PlayerMoveForward, PlayerInHallway };

public class DoorManager : MonoBehaviour
{
    // Physical setup of the scene
    private static float roomWidth_ = 10f;
    private static float roomDepth_ = 10f;
    private static float roomHeight_ = 10f;

    private static float hallwayWidth_ = 4f;
    private static float hallwayDepth_ = 10f;
    private static float hallwayHeight_ = 7f;

    private static float leftDoorX = -2.5f;
    private static float rightDoorX = 2.5f;
    private static float hallwayEndZ = roomDepth_/2 + hallwayDepth_ - 2f;

    private static Vector3 leftBadGuySpawn = new Vector3(leftDoorX, 3, hallwayEndZ - 1);
    private static Vector3 rightBadGuySpawn = new Vector3(rightDoorX, 3, hallwayEndZ - 1);

    // Materials
    public Material stoneBrickUncolored;
    public Material woodDoorUncolored;
    public Material darkness;

    private GameObject doorWall_, leftDoor_, rightDoor_, leftHallway_, rightHallway_;

    // Game Management
    private GameState currentState = GameState.Choosing;
    private int level = 0;

    public Transform playerTransform;
    private Vector3 playerStartPosition_ = new Vector3(0, 1.5f, -4);
    public float playerLateralSpeed = 0.002f;
    public float playerForwardSpeed = 0.005f;

    // Other prefabs
    public GameObject badGuyPrefab;
    private GameObject currentBadGuy_;

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

            // Check if a door is open all the way
            if (leftDoor_.GetComponent<Door>().IsOpen())
            {
                currentState = GameState.PlayerMoveLeft;
                rightDoor_.GetComponent<Door>().Close();
            }
            else if (rightDoor_.GetComponent<Door>().IsOpen())
            {
                currentState = GameState.PlayerMoveRight;
                leftDoor_.GetComponent<Door>().Close();
            }
        }
        else if (currentState == GameState.PlayerMoveLeft)
        {
            if(playerTransform.position.x > leftDoorX)
            {
                playerTransform.Translate(-playerLateralSpeed, 0, 0);
            }
            else
            {
                playerTransform.position = new Vector3(leftDoorX, playerTransform.position.y, playerTransform.position.z);
                currentState = GameState.PlayerMoveForward;
            }
        }
        else if (currentState == GameState.PlayerMoveRight)
        {
            if (playerTransform.position.x < rightDoorX)
            {
                playerTransform.Translate(playerLateralSpeed, 0, 0);
            }
            else
            {
                playerTransform.position = new Vector3(rightDoorX, playerTransform.position.y, playerTransform.position.z);
                currentState = GameState.PlayerMoveForward;
            }
        }
        else if(currentState == GameState.PlayerMoveForward)
        {
            playerTransform.Translate(0, 0, playerForwardSpeed);

            if(playerTransform.position.z > roomDepth_ / 2)
            {
                leftDoor_.GetComponent<Door>().Close();
                rightDoor_.GetComponent<Door>().Close();
                currentState = GameState.PlayerInHallway;
                SpawnBadGuy();
            }

            
        }
        else if(currentState == GameState.PlayerInHallway)
        {
            playerTransform.Translate(0, 0, playerForwardSpeed);
            if (playerTransform.position.z > hallwayEndZ)
            {
                this.SetupNextLevel();
            }
        }



    }

    private void SetupNextLevel()
    {
        level++;
        currentState = GameState.Choosing;
        playerTransform.position = playerStartPosition_;
        Destroy(currentBadGuy_);

    }

    private void SpawnBadGuy()
    {
        currentBadGuy_ = Instantiate(badGuyPrefab);
        if(Random.Range(0, 2) == 0)
        {
            currentBadGuy_.transform.position = leftBadGuySpawn;
        }
        else
        {
            currentBadGuy_.transform.position = rightBadGuySpawn;
        }
    }
    
}
