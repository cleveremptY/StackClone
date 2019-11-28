using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject gameCamera;
    public float moveSpeed = 1f;
    private GameObject newCameraPosition;
    private bool isGo;
    
    public void SetNewCameraPosition(Vector3 position)
    {
        newCameraPosition = new GameObject();
        newCameraPosition.name = "Camera move point";
        newCameraPosition.transform.position = position;
    }
    public void ActivateMove()
    {
        isGo = true;
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
    }
}
