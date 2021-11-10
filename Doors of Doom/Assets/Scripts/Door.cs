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

    private bool isOpening_ = false;
    private bool isClosing_ = false;
    private bool isOpen_ = false;
    private bool isClosed_ = true;

    private float angle_ = 0;

    private float speed_ = 1f;

    private Vector3 pivot_;

    // Start is called before the first frame update
    void Start()
    {
        pivot_ = transform.position - transform.right * width_ / 2f - transform.forward * height_ / 2f;
        this.CreateFaces();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening_)
        {
            if (angle_ + speed_ > 90)
            {
                angle_ = 90;
                transform.RotateAround(pivot_, Vector3.up, 90 - angle_);
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
            if (angle_ - speed_ < 0)
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
    public void CreateFaces()
    {
        topFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        topFace.transform.SetParent(transform);
        topFace.transform.localScale = new Vector3(width_, depth_, 1f);
        topFace.transform.localPosition = height_ / 2f * Vector3.up;
        topFace.transform.Rotate(Vector3.right, 90f);

        bottomFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        bottomFace.transform.SetParent(transform);
        bottomFace.transform.localScale = new Vector3(width_, depth_, 1f);
        bottomFace.transform.localPosition = height_ / 2f * Vector3.down;
        bottomFace.transform.Rotate(Vector3.right, -90f);

        backFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backFace.transform.SetParent(transform);
        backFace.transform.localScale = new Vector3(width_, height_, 1f);
        backFace.transform.localPosition = depth_ / 2f * Vector3.forward;
        backFace.transform.Rotate(Vector3.up, 180f);

        frontFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        frontFace.transform.SetParent(transform);
        frontFace.transform.localScale = new Vector3(width_, height_, 1f);
        frontFace.transform.localPosition = depth_ / 2f * Vector3.back;
        // Don't need to rotate this one

        rightFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        rightFace.transform.SetParent(transform);
        rightFace.transform.localScale = new Vector3(depth_, height_, 1f);
        rightFace.transform.localPosition = width_ / 2f * Vector3.right;
        rightFace.transform.Rotate(Vector3.up, -90f);

        leftFace = GameObject.CreatePrimitive(PrimitiveType.Quad);
        leftFace.transform.SetParent(transform);
        leftFace.transform.localScale = new Vector3(depth_, height_, 1f);
        leftFace.transform.localPosition = width_ / 2f * Vector3.left;
        leftFace.transform.Rotate(Vector3.up, 90f);
    }

    public void Open()
    {
        if (isClosed_)
        {
            isClosed_ = false;
            isOpening_ = true;
        }
    }
    public void Close()
    {
        if (isOpen_)
        {
            isOpen_ = false;
            isClosing_ = true;
        }
    }
} 
