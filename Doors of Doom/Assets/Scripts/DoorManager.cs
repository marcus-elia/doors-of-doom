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

    // Start is called before the first frame update
    void Start()
    {
        GameObject doorWall = new GameObject();
        doorWall.transform.position = new Vector3(0f, 0f, 5f);
        doorWall.AddComponent<DoorWall>();
        doorWall.GetComponent<DoorWall>().SetMaterial(stoneBrickUncolored);
        doorWall.GetComponent<DoorWall>().CreateFaces();

        GameObject leftDoor = new GameObject();
        leftDoor.transform.position = new Vector3(-2.5f, 0f, 5f - 0.25f);
        leftDoor.AddComponent<Door>();
        leftDoor.GetComponent<Door>().SetMaterial(woodDoorUncolored);
        leftDoor.GetComponent<Door>().CreateFaces();

        GameObject rightDoor = new GameObject();
        rightDoor.transform.position = new Vector3(2.5f, 0f, 5f - 0.25f);
        rightDoor.AddComponent<Door>();
        rightDoor.GetComponent<Door>().SetMaterial(woodDoorUncolored);
        rightDoor.GetComponent<Door>().CreateFaces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
