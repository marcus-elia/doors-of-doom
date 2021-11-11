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

    private static Vector3 leftItemSpawnPosition_ = new Vector3(leftDoorX, 2, hallwayEndZ - 1);
    private static Vector3 rightItemSpawnPosition_ = new Vector3(rightDoorX, 2, hallwayEndZ - 1);

    // Materials
    public Material stoneBrickUncolored;
    public Material woodDoorUncolored;
    public Material darkness;

    private GameObject doorWall_, leftDoor_, rightDoor_, leftHallway_, rightHallway_;

    // Game Management
    private GameState currentState = GameState.Choosing;
    public UIManager uiManager;
    private bool badGuySpawned_ = false;
    private bool snowballSpawned_ = false;
    private int level = 0;

    public Transform playerTransform;
    private Vector3 playerStartPosition_ = new Vector3(0, 1.5f, -4);
    public float playerLateralSpeed_ = 0.002f;
    public float playerForwardSpeed_ = 0.005f;
    private float playerRadius_ = 1f;

    private int numSnowballs_ = 0;

    private static float collisionDistance_ = 1.5f;
    private static float throwingForce_ = 15f;

    // Other prefabs
    public GameObject badGuyPrefab;
    private GameObject currentBadGuy_;
    public GameObject snowballPrefab;
    private GameObject currentSnowball_;
    public GameObject snowballProjectilePrefab;
    public static List<GameObject> thrownProjectiles = new List<GameObject>();

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
                playerTransform.Translate(-playerLateralSpeed_, 0, 0);
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
                playerTransform.Translate(playerLateralSpeed_, 0, 0);
            }
            else
            {
                playerTransform.position = new Vector3(rightDoorX, playerTransform.position.y, playerTransform.position.z);
                currentState = GameState.PlayerMoveForward;
            }
        }
        else if(currentState == GameState.PlayerMoveForward)
        {
            playerTransform.Translate(0, 0, playerForwardSpeed_);

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
            // Move the player forward
            playerTransform.Translate(0, 0, playerForwardSpeed_);

            // Check if the player has obtained a snowball
            if(snowballSpawned_ && Vector3.Distance(playerTransform.position, currentSnowball_.transform.position) < collisionDistance_)
            {
                Destroy(currentSnowball_);
                numSnowballs_++;
                UIManager.numSnowballs = numSnowballs_;
                snowballSpawned_ = false;
            }

            // If the player throws a snowball
            if(Input.anyKeyDown)
            {
                if(numSnowballs_ > 0)
                {
                    ThrowSnowball();
                    numSnowballs_--;
                }
            }

            // Check if any snowball hits the bad guy or a snowball
            for(int i = 0; i < thrownProjectiles.Count; i++)
            {
                if(badGuySpawned_ && Vector3.Distance(thrownProjectiles[i].transform.position, currentBadGuy_.transform.position) < collisionDistance_)
                {
                    Destroy(currentBadGuy_);
                    badGuySpawned_ = false;
                }

                else if (snowballSpawned_ && Vector3.Distance(thrownProjectiles[i].transform.position, currentSnowball_.transform.position) < collisionDistance_)
                {
                    Destroy(currentSnowball_);
                    snowballSpawned_ = false;
                }
            }

            // Check if the player has run into the bad guy
            if (badGuySpawned_ && Vector3.Distance(playerTransform.position, currentBadGuy_.transform.position) < collisionDistance_)
            {
                EndGame();
            }

            // Check if the player reached the end of the hallway
            if (playerTransform.position.z > hallwayEndZ)
            {
                this.SetupNextLevel();
            }
        }



    }

    private void SetupNextLevel()
    {
        level++;
        UIManager.level = level;
        currentState = GameState.Choosing;
        playerTransform.position = playerStartPosition_;
        thrownProjectiles = new List<GameObject>();
        Destroy(currentBadGuy_);
        badGuySpawned_ = false;
        Destroy(currentSnowball_);
        snowballSpawned_ = false;
    }

    private void SpawnBadGuy()
    {
        currentBadGuy_ = Instantiate(badGuyPrefab);
        currentSnowball_ = Instantiate(snowballPrefab);
        if(Random.Range(0, 2) == 0)
        {
            currentBadGuy_.transform.position = leftItemSpawnPosition_;
            currentSnowball_.transform.position = rightItemSpawnPosition_;
        }
        else
        {
            currentBadGuy_.transform.position = rightItemSpawnPosition_;
            currentSnowball_.transform.position = leftItemSpawnPosition_;
        }
        snowballSpawned_ = true;
        badGuySpawned_ = true;
    }

    public void ThrowSnowball()
    {
        GameObject newProj = Instantiate(snowballProjectilePrefab);
        newProj.transform.position = playerTransform.position + playerTransform.forward * playerRadius_;
        newProj.GetComponent<Rigidbody>().velocity = new Vector3(0, throwingForce_/3, 2*throwingForce_/3);
        thrownProjectiles.Add(newProj);
        UIManager.numSnowballs--;
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        uiManager.endScreen.SetActive(true);
        uiManager.SetResultString();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SetupNextLevel();
        level = 0;
        numSnowballs_ = 0;
        UIManager.level = 0;
        UIManager.numSnowballs = 0;
        uiManager.endScreen.SetActive(false);
    }
}
