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

    public void CreateFaces()
    {
        //this.InitializeFaces();
        //this.AddTopFaceMesh();
        //topFace.GetComponent<Renderer>().material = mat_;
        //leftFace.GetComponent<MeshRenderer>().material = mat_;
        //rightFace.GetComponent<MeshRenderer>().material = mat_;
        //middleFace.GetComponent<MeshRenderer>().material = mat_;
        this.CreateTopFace();
    }

    public void CreateTopFace()
    {
        topFace = new GameObject();
        topFace.AddComponent<QuadCreator>();
        topFace.GetComponent<QuadCreator>().SetParameters(width_, height_ - doorHeight_, 0, doorHeight_ / height_, 1, 1, mat_);
        topFace.GetComponent<QuadCreator>().CreateMesh();
        topFace.transform.SetParent(transform);
        //topFace.transform.localScale = new Vector3(width_, height_ - doorHeight_, 1f);
        //topFace.transform.localPosition = (doorHeight_ + (height_ - doorHeight_) / 2f) * Vector3.up;
        topFace.transform.localPosition = doorHeight_ * Vector3.up + width_ / 2 * Vector3.left;
    }

    public void InitializeFaces()
    {
        topFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        topFace.transform.SetParent(transform);
        topFace.transform.localScale = new Vector3(width_, height_ - doorHeight_, 1f);
        topFace.transform.localPosition = (doorHeight_ + (height_ - doorHeight_) / 2f) * Vector3.up;

        leftFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        leftFace.transform.SetParent(transform);
        leftFace.transform.localScale = new Vector3(edgeWidth_, doorHeight_, 1f);
        leftFace.transform.localPosition = (doorHeight_  / 2f) * Vector3.up + (width_/2f - edgeWidth_/2f) * Vector3.left;

        rightFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        rightFace.transform.SetParent(transform);
        rightFace.transform.localScale = new Vector3(edgeWidth_, doorHeight_, 1f);
        rightFace.transform.localPosition = (doorHeight_ / 2f) * Vector3.up + (width_ / 2f - edgeWidth_ / 2f) * Vector3.right;

        spaceBetweenDoors_ = width_ - 2 * doorWidth_ - 2 * edgeWidth_;
        middleFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        middleFace.transform.SetParent(transform);
        middleFace.transform.localScale = new Vector3(spaceBetweenDoors_, doorHeight_, 1f);
        middleFace.transform.localPosition = (doorHeight_ / 2f) * Vector3.up;
    }

    private void AddTopFaceMesh()
    {
        MeshRenderer meshRenderer = topFace.GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mat_;
        
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width_, 0, 0),
            new Vector3(0, height_ - doorHeight_, 0),
            new Vector3(width_, height_ - doorHeight_, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        float u1, v1, u2, v2;
        u1 = 0f;
        v1 = (doorHeight_ / height_);
        u2 = 1f;
        v2 = 1f;
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(u1, v1),
            new Vector2(u2, v1),
            new Vector2(u1, v2),
            new Vector2(u2, v2)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
