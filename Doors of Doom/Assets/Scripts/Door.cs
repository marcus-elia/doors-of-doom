using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float xWidth = 2f;
    public float zWidth = 4f;

    private bool isOpening = false;
    private bool isClosing = false;
    private bool isOpen = false;
    private bool isClosed = true;

    private float angle = 0;

    private float speed = 1f;

    private Vector3 pivot;

    // Start is called before the first frame update
    void Start()
    {
        pivot = transform.position - transform.right * xWidth / 2f - transform.forward * zWidth / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpening)
        {
            if (angle + speed > 90)
            {
                angle = 90;
                transform.RotateAround(pivot, Vector3.up, 90 - angle);
                isOpen = true;
                isOpening = false;
            }
            else
            {
                angle += speed;
                transform.RotateAround(pivot, Vector3.up, speed);
            }
        }
        if (isClosing)
        {
            if (angle - speed < 0)
            {
                angle = 0;
                transform.RotateAround(pivot, Vector3.up, 0 - angle);
                isClosed = true;
                isClosing = false;
            }
            else
            {
                angle -= speed;
                transform.RotateAround(pivot, Vector3.up, -speed);
            }
        }


    }

    public void Open()
    {
        if (isClosed)
        {
            isClosed = false;
            isOpening = true;
        }
    }
    public void Close()
    {
        if (isOpen)
        {
            isOpen = false;
            isClosing = true;
        }
    }
} 
