using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public float roomWidth_ = 10f;
    public float roomDepth_ = 10f;
    public float roomHeight_ = 10f;

    public Material stoneBrickUncolored;
    public Material woodDoorUncolored;

    private GameObject doorWall_, leftDoor_, rightDoor_;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            leftDoor_.GetComponent<Door>().Toggle();
            rightDoor_.GetComponent<Door>().Toggle();
        }
    }
}
