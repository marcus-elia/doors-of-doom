using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  A DoorWall is shaped like
 * 
 *  t t t t t t t t t t
 *  t t t t t t t t t t
 *  L L     m m     R R
 *  L L     m m     R R
 *  L L     m m     R R
 *  
 *  t = top face
 *  L = left face
 *  R = right face
 *  m = middle face
 */
public class DoorWall : MonoBehaviour
{
    // The total size of the wall
    private float width_ = 10f;
    private float height_ = 10f;

    private float doorWidth_ = 3f;
    private float doorHeight_ = 6f;
    private float edgeWidth_ = 1f;
    private float spaceBetweenDoors_; 

    // Quads
    private GameObject leftFace;
    private GameObject rightFace;
    private GameObject middleFace;
    private GameObject topFace;

    // The material needs to be passed in from the DoorManager
    private Material mat_;

    // Start is called before the first frame update
    void Start()
    {
        spaceBetweenDoors_ = width_ - 2 * doorWidth_ - 2 * edgeWidth_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaterial(Material inputMat)
    {
        mat_ = inputMat;
    }

    public void UpdateMaterial(Material inputMat)
    {
        SetMaterial(inputMat);
        leftFace.GetComponent<MeshRenderer>().material = mat_;
        rightFace.GetComponent<MeshRenderer>().material = mat_;
        topFace.GetComponent<MeshRenderer>().material = mat_;
        middleFace.GetComponent<MeshRenderer>().material = mat_;
    }

    public void CreateFaces()
    {
        this.CreateTopFace();
        this.CreateLeftFace();
        this.CreateRightFace();
        this.CreateMiddleFace();
    }

    public void CreateTopFace()
    {
        topFace = new GameObject();
        topFace.AddComponent<QuadCreator>();
        topFace.GetComponent<QuadCreator>().SetParameters(width_, height_ - doorHeight_, 0, doorHeight_ / height_, 1, 1, mat_);
        topFace.GetComponent<QuadCreator>().CreateMesh();
        topFace.transform.SetParent(transform);
        topFace.transform.localPosition = doorHeight_ * Vector3.up + width_ / 2 * Vector3.left;
    }

    public void CreateLeftFace()
    {
        leftFace = new GameObject();
        leftFace.AddComponent<QuadCreator>();
        leftFace.GetComponent<QuadCreator>().SetParameters(edgeWidth_, doorHeight_, 0, 0, edgeWidth_/width_, doorHeight_ / height_, mat_);
        leftFace.GetComponent<QuadCreator>().CreateMesh();
        leftFace.transform.SetParent(transform);
        leftFace.transform.localPosition = width_ / 2 * Vector3.left;
    }

    public void CreateRightFace()
    {
        rightFace = new GameObject();
        rightFace.AddComponent<QuadCreator>();
        rightFace.GetComponent<QuadCreator>().SetParameters(edgeWidth_, doorHeight_, // Face dimensions
            (width_ - edgeWidth_) / width_, 0, 1, doorHeight_ / height_,             // UVs
            mat_);
        rightFace.GetComponent<QuadCreator>().CreateMesh();
        rightFace.transform.SetParent(transform);
        rightFace.transform.localPosition = (width_ / 2 - edgeWidth_) * Vector3.right;
    }

    public void CreateMiddleFace()
    {
        // Make sure this is initialized first
        spaceBetweenDoors_ = width_ - 2 * doorWidth_ - 2 * edgeWidth_;

        middleFace = new GameObject();
        middleFace.AddComponent<QuadCreator>();
        middleFace.GetComponent<QuadCreator>().SetParameters(spaceBetweenDoors_, doorHeight_,                                        // Face dimensions
            (doorWidth_ + edgeWidth_) / width_, 0, (doorWidth_ + edgeWidth_ + spaceBetweenDoors_) / width_, doorHeight_ / height_,   // UVs
            mat_);
        middleFace.GetComponent<QuadCreator>().CreateMesh();
        middleFace.transform.SetParent(transform);
        middleFace.transform.localPosition = spaceBetweenDoors_ / 2 * Vector3.left;
    }
}
