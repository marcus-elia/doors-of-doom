using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Dimensions of the door
    private float width_ = 3f;
    private float height_ = 6f;
    private float depth_ = 0.5f;

    // Quads
    private GameObject topFace;
    private GameObject bottomFace;
    private GameObject frontFace;
    private GameObject backFace;
    private GameObject leftFace;
    private GameObject rightFace;

    private Material mat_;

    private bool isOpening_ = false;
    private bool isClosing_ = false;
    private bool isOpen_ = false;
    private bool isClosed_ = true;

    private float angle_ = 0;

    // The negative makes it open ccw
    private float speed_;

    private Vector3 pivot_;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening_)
        {
            if (angle_ + speed_ < -90)
            {
                angle_ = -90;
                transform.RotateAround(pivot_, Vector3.up, -90 - angle_);
                isOpen_ = true;
                isOpening_ = false;
            }
            else
            {
                angle_ += speed_;
                transform.RotateAround(pivot_, Vector3.up, speed_);
            }
        }
        if (isClosing_)
        {
            if (angle_ - speed_ > 0)
            {
                angle_ = 0;
                transform.RotateAround(pivot_, Vector3.up, 0 - angle_);
                isClosed_ = true;
                isClosing_ = false;
            }
            else
            {
                angle_ -= speed_;
                transform.RotateAround(pivot_, Vector3.up, -speed_);
            }
        }
    }

    // ===================================
    //
    //      Initialization Functions
    //
    // ===================================

    public void SetMaterial(Material inputMat)
    {
        mat_ = inputMat;
    }
    public void SetSpeed(float input)
    {
        speed_ = input;
    }
    public void SetPivot()
    {
        pivot_ = pivot_ = transform.position + transform.right * width_ / 2f + transform.forward * depth_/2;
    }
    public void CreateFaces()
    {
        this.CreateFrontFace();
        this.CreateBackFace();
        this.CreateRightFace();
        this.CreateLeftFace();
        this.CreateTopFace();
        this.CreateBottomFace();
    }

    public void CreateFrontFace()
    {
        frontFace = new GameObject();
        frontFace.AddComponent<QuadCreator>();
        frontFace.GetComponent<QuadCreator>().SetParameters(width_, height_, 0, 0, width_ / (width_ + depth_), 1, mat_);
        frontFace.GetComponent<QuadCreator>().CreateMesh();
        frontFace.transform.SetParent(transform);
        frontFace.transform.localPosition = width_ / 2 * Vector3.left;
    }

    public void CreateRightFace()
    {
        rightFace = new GameObject();
        rightFace.AddComponent<QuadCreator>();
        rightFace.GetComponent<QuadCreator>().SetParameters(depth_, height_, width_ / (width_ + depth_), 0, 1, 1, mat_);
        rightFace.GetComponent<QuadCreator>().CreateMesh();
        rightFace.transform.SetParent(transform);
        rightFace.transform.localPosition = width_ / 2f * Vector3.right;
        rightFace.transform.Rotate(Vector3.up, -90f);
    }

    public void CreateBackFace()
    {
        backFace = new GameObject();
        backFace.AddComponent<QuadCreator>();
        backFace.GetComponent<QuadCreator>().SetParameters(width_, height_, 0, 0, width_ / (width_ + depth_), 1, mat_);
        backFace.GetComponent<QuadCreator>().CreateMesh();
        backFace.transform.SetParent(transform);
        backFace.transform.localPosition = depth_ * Vector3.forward + width_ / 2 * Vector3.right;
        backFace.transform.Rotate(Vector3.up, 180f);
    }

    public void CreateLeftFace()
    {
        leftFace = new GameObject();
        leftFace.AddComponent<QuadCreator>();
        leftFace.GetComponent<QuadCreator>().SetParameters(depth_, height_, width_ / (width_ + depth_), 0, 1, 1, mat_);
        leftFace.GetComponent<QuadCreator>().CreateMesh();
        leftFace.transform.SetParent(transform);
        leftFace.transform.localPosition = width_ / 2f * Vector3.left + depth_ * Vector3.forward;
        leftFace.transform.Rotate(Vector3.up, 90f);
    }

    public void CreateTopFace()
    {
        topFace = new GameObject();
        topFace.AddComponent<QuadCreator>();
        topFace.GetComponent<QuadCreator>().SetParameters(width_, depth_, 0, 0, 1, 0.1f, mat_);
        topFace.GetComponent<QuadCreator>().CreateMesh();
        topFace.transform.SetParent(transform);
        topFace.transform.localPosition = height_ * Vector3.up + width_/2 * Vector3.left;
        topFace.transform.Rotate(Vector3.right, 90f);
    }

    public void CreateBottomFace()
    {
        bottomFace = new GameObject();
        bottomFace.AddComponent<QuadCreator>();
        bottomFace.GetComponent<QuadCreator>().SetParameters(width_, depth_, 0, 0, 1, 0.1f, mat_);
        bottomFace.GetComponent<QuadCreator>().CreateMesh();
        bottomFace.transform.SetParent(transform);
        bottomFace.transform.localPosition = width_ / 2 * Vector3.left + depth_ * Vector3.forward;
        bottomFace.transform.Rotate(Vector3.right, -90f);
    }

    // ===============================================
    //
    //              Door Interactions
    //
    // ===============================================

    public void Open()
    {
        isClosed_ = false;
        isOpening_ = true;
        isClosing_ = false;
    }
    public void Close()
    {
        isOpen_ = false;
        isClosing_ = true;
        isOpening_ = false;
    }

    public void Toggle()
    {
        if (isClosed_ || isClosing_)
        {
            isClosing_ = false;
            Open();
        }
        else if(isOpen_ || isOpening_)
        {
            isOpening_ = false;
            Close();
        }
    }

    public bool IsOpen()
    {
        return isOpen_;
    }
    public bool IsClosed()
    {
        return isClosed_;
    }
} 
