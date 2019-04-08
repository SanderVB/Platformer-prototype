using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour {

    [SerializeField] bool scrolling;
    [SerializeField] bool parallax;
    [SerializeField] bool verticalMovement;
    [SerializeField] float backgroundSize;
    [SerializeField] float parallaxSpeed, parallaxVerticalSpeed;

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10f;
    private float lastCameraX, lastCameraY;
    private int leftIndex, rightIndex;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraX = cameraTransform.position.x;
        lastCameraY = cameraTransform.position.y;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            layers[i] = transform.GetChild(i);
        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void FixedUpdate()
    {
        if (parallax)
        {
            float deltaX = cameraTransform.position.x - lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }

        if(parallax && verticalMovement)
        {
            float deltaY = cameraTransform.position.y - lastCameraY;
            transform.position += Vector3.up * (deltaY * parallaxVerticalSpeed);
            lastCameraY = cameraTransform.position.y;
        }

        lastCameraX = cameraTransform.position.x;

        if (scrolling)
        {
            if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
                ScrollLeft();
            if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
                ScrollRight();
        }

    }

    private void ScrollLeft()
    {
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if(rightIndex<0)
            rightIndex = layers.Length - 1;
    }

    private void ScrollRight()
    {
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
            leftIndex = 0;
    }
}
