using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : MonoBehaviour
{
    private float depth_;
    private float height_;
    private float width_;

    private Material mat_;

    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject ceiling;
    private GameObject floor;
    private GameObject backWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDimensions(float width, float depth, float height)
    {
        width_ = width; depth_ = depth; height_ = height;
    }
    public void SetMaterial(Material input)
    {
        mat_ = input;
    }

    public void CreateFaces()
    {
        this.CreateLeftWall();
        this.CreateRightWall();
        this.CreateBackWall();
        this.CreateCeiling();
        this.CreateFloor();
    }

    public void CreateLeftWall()
    {
        leftWall = new GameObject();
        leftWall.AddComponent<QuadCreator>();
        leftWall.GetComponent<QuadCreator>().SetParameters(depth_, height_, 0, 0, 1, 1, mat_);
        leftWall.GetComponent<QuadCreator>().CreateMesh();
        leftWall.transform.SetParent(transform);
        leftWall.transform.localPosition = height_/2 * Vector3.down + width_ / 2 * Vector3.left + depth_/2 * Vector3.back;
        leftWall.transform.Rotate(Vector3.up, -90f);
    }

    public void CreateRightWall()
    {
        rightWall = new GameObject();
        rightWall.AddComponent<QuadCreator>();
        rightWall.GetComponent<QuadCreator>().SetParameters(depth_, height_, 0, 0, 1, 1, mat_);
        rightWall.GetComponent<QuadCreator>().CreateMesh();
        rightWall.transform.SetParent(transform);
        rightWall.transform.localPosition = height_ / 2 * Vector3.down + width_ / 2 * Vector3.right + depth_ / 2 * Vector3.forward;
        rightWall.transform.Rotate(Vector3.up, 90f);
    }

    public void CreateBackWall()
    {
        backWall = new GameObject();
        backWall.AddComponent<QuadCreator>();
        backWall.GetComponent<QuadCreator>().SetParameters(width_, height_, 0, 0, 1, 1, mat_);
        backWall.GetComponent<QuadCreator>().CreateMesh();
        backWall.transform.SetParent(transform);
        backWall.transform.localPosition = height_ / 2 * Vector3.down + width_ / 2 * Vector3.left + depth_ / 2 * Vector3.forward;
    }

    public void CreateCeiling()
    {
        ceiling = new GameObject();
        ceiling.AddComponent<QuadCreator>();
        ceiling.GetComponent<QuadCreator>().SetParameters(width_, depth_, 0, 0, 1, 1, mat_);
        ceiling.GetComponent<QuadCreator>().CreateMesh();
        ceiling.transform.SetParent(transform);
        ceiling.transform.localPosition = height_ / 2 * Vector3.up + width_ / 2 * Vector3.left + depth_ / 2 * Vector3.forward;
        ceiling.transform.Rotate(Vector3.right, -90f);
    }

    public void CreateFloor()
    {
        floor = new GameObject();
        floor.AddComponent<QuadCreator>();
        floor.GetComponent<QuadCreator>().SetParameters(width_, depth_, 0, 0, 1, 1, mat_);
        floor.GetComponent<QuadCreator>().CreateMesh();
        floor.transform.SetParent(transform);
        floor.transform.localPosition = height_ / 2 * Vector3.down + width_ / 2 * Vector3.left + depth_ / 2 * Vector3.back;
        floor.transform.Rotate(Vector3.right, 90f);
    }
}
