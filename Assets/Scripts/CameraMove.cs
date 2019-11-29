using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject gameCamera;
    public float moveSpeed = 1f;
    public float resizeSpeed = 0.25f;
    private GameObject newCameraPosition;
    private float newCameraSize;
    private bool isGo;
    private bool isResize;
    private bool isSmaller;
    
    public void SetNewCameraPosition(Vector3 position)
    {
        Destroy(newCameraPosition);
        newCameraPosition = new GameObject();
        newCameraPosition.name = "Camera move point";
        newCameraPosition.transform.position = position;
    }
    public void SetNewCameraSize(float size)
    {
        newCameraSize = size;
    }
    public void ActivateMove()
    {
        isGo = true;
    }

    public void DeactivateMove()
    {
        isResize = false;
    }

    public void ActivateResize()
    {
        isResize = true;
        if (gameCamera.GetComponent<Camera>().orthographicSize < newCameraSize)
            isSmaller = true;
        else
            isSmaller = false;
    }

    public void DeactivateResize()
    {
        isGo = false;
    }

    void Update()
    {
        if (isGo)
        {
            if (gameCamera.transform.position == newCameraPosition.transform.position)
            {
                isGo = false;
                Destroy(newCameraPosition);
            }
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, newCameraPosition.transform.position, Time.deltaTime * moveSpeed);
        }
        if (isResize)
        {
            if (!isSmaller)
            {
                if (gameCamera.GetComponent<Camera>().orthographicSize >= newCameraSize)
                    gameCamera.GetComponent<Camera>().orthographicSize -= newCameraSize * Time.deltaTime * moveSpeed;
                else
                    isResize = false;
            }
            else
            {
                if (gameCamera.GetComponent<Camera>().orthographicSize <= newCameraSize)
                    gameCamera.GetComponent<Camera>().orthographicSize += newCameraSize * Time.deltaTime * moveSpeed;
                else
                    isResize = false;
            }
        }
    }
}
