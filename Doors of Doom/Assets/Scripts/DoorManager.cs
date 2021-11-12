using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { StartScreen, Choosing, PlayerMoveLeft, PlayerMoveRight, PlayerMoveForward, PlayerInHallway, EndScreen };

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

    private static Vector3 leftItemSpawnPosition_ = new Vector3(leftDoorX, 2.5f, hallwayEndZ - 2);
    private static Vector3 rightItemSpawnPosition_ = new Vector3(rightDoorX, 2.5f, hallwayEndZ - 2);

    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject backWall;
    public GameObject floor;
    public GameObject ceiling;

    // Materials
    public Material stoneBrickUncolored;
    public Material woodDoorUncolored;
    public Material darkness;
    public Material stoneBrickBlue;
    public Material stoneBrickDark;
    public Material stoneBrickGreen;
    public Material stoneBrickRed;

    private GameObject doorWall_, leftDoor_, rightDoor_, leftHallway_, rightHallway_;

    // Game Management
    private GameState currentState = GameState.StartScreen;
    public UIManager uiManager;
    private bool badGuySpawned_ = false;
    private bool snowballSpawned_ = false;
    private int level = 0;

    public Transform playerTransform;
    private Vector3 playerStartPosition_ = new Vector3(0, 1.5f, -4);
    private float playerRadius_ = 1f;

    private int numSnowballs_ = 0;

    private static float collisionDistance_ = 1.5f;

    public GameObject instructionsUI;
    public GameObject ingameUI;

    // Speeds that vary by platforms
    // For the Unity editor =====================================
    //private static float throwingForce_ = 25f;
    //private float playerLateralSpeed_ = 0.002f;
    //private float playerForwardSpeed_ = 0.0085f;
    //private float doorSpeed_ = -0.1f;
    // For Windows build ========================================
    private static float throwingForce_ = 8*25f;
    private float playerLateralSpeed_ = 8*0.002f;
    private float playerForwardSpeed_ = 8*0.0085f;
    private float doorSpeed_ = -8 * 0.1f;

    // Other prefabs
    public GameObject badGuyPrefab;
    public GameObject badGuyPrefab2;
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
        leftDoor_.GetComponent<Door>().SetSpeed(doorSpeed_);

        rightDoor_ = new GameObject();
        rightDoor_.transform.position = new Vector3(2.5f, 0f, 5f - 0.25f);
        rightDoor_.AddComponent<Door>();
        rightDoor_.GetComponent<Door>().SetMaterial(woodDoorUncolored);
        rightDoor_.GetComponent<Door>().CreateFaces();
        rightDoor_.GetComponent<Door>().SetPivot();
        rightDoor_.GetComponent<Door>().SetSpeed(doorSpeed_);

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
        if(currentState == GameState.StartScreen)
        {
            if(Input.anyKeyDown)
            {
                currentState = GameState.Choosing;
                instructionsUI.SetActive(false);
                ingameUI.SetActive(true);
            }
        }
        else if(currentState == GameState.Choosing)
        {
            // Toggle the left door
            if(LeftUserInput())
            {
                leftDoor_.GetComponent<Door>().Toggle();
            }
            // Toggle the right door
            else if(RightUserInput())
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
                currentState = GameState.EndScreen;
                EndGame();
            }

            // Check if the player reached the end of the hallway
            if (playerTransform.position.z > hallwayEndZ)
            {
                this.SetupNextLevel();
            }

            // Move the bad guy closer
            if (badGuySpawned_)
            {
                currentBadGuy_.transform.Translate(0, 0, playerForwardSpeed_);
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
        currentState = GameState.Choosing;
        this.SetRandomMaterial();
    }

    private void SpawnBadGuy()
    {
        // Create the bad guy and the snowball
        if(Random.Range(0, 2) == 0)
        {
            currentBadGuy_ = Instantiate(badGuyPrefab);
        }
        else
        {
            currentBadGuy_ = Instantiate(badGuyPrefab2);
        }
        currentSnowball_ = Instantiate(snowballPrefab);

        // Randomly choose which side to put them on
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
        newProj.GetComponent<Rigidbody>().velocity = new Vector3(0, throwingForce_/2, throwingForce_/3);
        thrownProjectiles.Add(newProj);
        UIManager.numSnowballs--;
    }

    public void SetRandomMaterial()
    {
        Material newMat;
        int r = Random.Range(0, 6);
        switch (r)
        {
            case 1:
                newMat = stoneBrickBlue;
                break;
            case 2:
                newMat = stoneBrickDark;
                break;
            case 3:
                newMat = stoneBrickGreen;
                break;
            case 4:
                newMat = stoneBrickRed;
                break;
            default:
                newMat = stoneBrickUncolored;
                break;
        }
        doorWall_.GetComponent<DoorWall>().UpdateMaterial(newMat);
        leftWall.GetComponent<Renderer>().material = newMat;
        rightWall.GetComponent<Renderer>().material = newMat;
        backWall.GetComponent<Renderer>().material = newMat;
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

    public void Quit()
    {
        Application.Quit();
    }

    public static bool LeftUserInput()
    {
        return Input.GetMouseButtonDown(0) ||
                Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F) ||
                Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) ||
                Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Q) ||
                Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.T) ||
                Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) ||
                Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftAlt) ||
                Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.CapsLock) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.BackQuote);
    }

    public static bool RightUserInput()
    {
        return Input.GetMouseButtonDown(1) ||
                Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) ||
                Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Period) ||
                Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) ||
                Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Semicolon) || Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Alpha7) ||
                Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Minus) ||
                Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.Quote) || Input.GetKeyDown(KeyCode.LeftBracket) || Input.GetKeyDown(KeyCode.RightBracket) ||
                Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightShift) ||
                Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Backslash);
    }
}
