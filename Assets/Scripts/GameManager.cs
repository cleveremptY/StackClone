using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Transform gameCamera;
    public Transform cameraStartPosition;

    public Text ScoreText;

    public GameObject currentBlock;
    public GameObject lastBlock;
    public GameObject blocksCategory;

    public GameObject prefabBlock;

    public float startDistance;
    public float borderDistance;
    public float speedBlock = 10f;

    private int Score = 0;
    private int colorValue;

    public UnityEvent onGameOver;

    public void NewGame()
    {
        GameObject newBlocksCategory = new GameObject();
        Destroy(blocksCategory);
        blocksCategory = newBlocksCategory;

        lastBlock = Instantiate(prefabBlock);
        lastBlock.transform.position = new Vector3(0, 0, 0);
        lastBlock.transform.parent = blocksCategory.transform;

        currentBlock = Instantiate(prefabBlock);
        currentBlock.transform.position = new Vector3(-20, 0.5f, 0);
        currentBlock.transform.parent = blocksCategory.transform;

        gameCamera.transform.position = cameraStartPosition.transform.position;
    }

    public void LoadLevel()
    {
        Score = 0;
        colorValue = Random.Range(0, 255);

        currentBlock.transform.position = new Vector3(startDistance, lastBlock.transform.localScale.y, 0);

        lastBlock.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(((colorValue + Score) / 100f) % 1f, 1f, 1f));
        currentBlock.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(((colorValue + Score + 1) / 100f) % 1f, 1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Score % 2 == 0)
            MoveByX();
        else
            MoveByZ();


        if (Input.GetMouseButtonDown(0))
        {
            Handheld.Vibrate();
            currentBlock.transform.position = new Vector3((float)Math.Round(currentBlock.transform.position.x, 1),
                currentBlock.transform.position.y,
                (float)Math.Round(currentBlock.transform.position.z, 1));
            if (!CheckGameOver())
            {
                if (lastBlock.transform.position.x != currentBlock.transform.position.x ||
                    lastBlock.transform.position.z != currentBlock.transform.position.z)
                    if (Score % 2 == 0)
                        SplitBlockByX();
                    else
                        SplitBlockByZ();
                else
                    Debug.Log("Perfect!");
                UpdateScore();
                NewBlock();
            }
        }      
    }

    private bool CheckGameOver()
    {
        if (Math.Abs(currentBlock.transform.position.x) > lastBlock.transform.localScale.x || Math.Abs(currentBlock.transform.position.z) > lastBlock.transform.localScale.z)
        {
            currentBlock.AddComponent<Rigidbody>();
            currentBlock.GetComponent<Rigidbody>().useGravity = true;

            gameCamera.transform.position = new Vector3(-10, gameCamera.transform.position.y - gameCamera.transform.position.y * 0.25f, -10);

            onGameOver.Invoke();
            return true;
        }
        else
            return false;
    }

    private void MoveByX()
    {
        currentBlock.transform.Translate(new Vector3(borderDistance, 0, 0) * Time.deltaTime * speedBlock);
        if (speedBlock > 0 && currentBlock.transform.position.x > lastBlock.transform.position.x + borderDistance)
            speedBlock *= -1;
        if (speedBlock < 0 && currentBlock.transform.position.x < lastBlock.transform.position.x - borderDistance)
            speedBlock *= -1;
    }

    private void MoveByZ()
    {
        currentBlock.transform.Translate(new Vector3(0, 0, borderDistance) * Time.deltaTime * speedBlock);
        if (speedBlock > 0 && currentBlock.transform.position.z > lastBlock.transform.position.z + borderDistance)
            speedBlock *= -1;
        if (speedBlock < 0 && currentBlock.transform.position.z < lastBlock.transform.position.z - borderDistance)
            speedBlock *= -1;
    }

    private void UpdateScore()
    {
        Score++;
        ScoreText.text = Convert.ToString(Score);
    }

    private void NewBlock()
    {
        startDistance *= -1;
        speedBlock *= -1;

        if (Score % 2 == 0)
        {
            startDistance *= -1;
            speedBlock *= -1;
        }

        lastBlock = currentBlock;
        currentBlock = Instantiate(currentBlock);
        currentBlock.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB(((colorValue + Score + 1) / 100f) % 1f, 1f, 1f));
        currentBlock.transform.parent = blocksCategory.transform;

        if (Score % 2 == 0)
            currentBlock.transform.position = new Vector3(startDistance,
                currentBlock.transform.position.y + lastBlock.transform.localScale.y,
                currentBlock.transform.position.z);
        else
            currentBlock.transform.position = new Vector3(currentBlock.transform.position.x,
                currentBlock.transform.position.y + lastBlock.transform.localScale.y,
                startDistance);

        gameCamera.position = new Vector3(lastBlock.transform.position.x - 7,
            gameCamera.position.y + lastBlock.transform.localScale.y,
            lastBlock.transform.position.z - 7);
    }

    private void SplitBlockByX()
    {
        GameObject blockShard = Instantiate(currentBlock);
        blockShard.transform.parent = blocksCategory.transform;

        blockShard.transform.localScale = new Vector3(Math.Abs(lastBlock.transform.position.x - blockShard.transform.position.x), 
            blockShard.transform.localScale.y, 
            blockShard.transform.localScale.z);
        
        currentBlock.transform.localScale = new Vector3(currentBlock.transform.localScale.x - Math.Abs(lastBlock.transform.position.x - currentBlock.transform.position.x), 
            currentBlock.transform.localScale.y, 
            currentBlock.transform.localScale.z);

        currentBlock.transform.position = new Vector3(lastBlock.transform.position.x * 0.5f + currentBlock.transform.position.x * 0.5f, 
            currentBlock.transform.position.y, 
            currentBlock.transform.position.z);

        int positionTemp = 1;
        if (currentBlock.transform.position.x < lastBlock.transform.position.x)
            positionTemp *= -1;
        blockShard.transform.position = new Vector3(lastBlock.transform.position.x + positionTemp * (currentBlock.transform.localScale.x * 0.5f + blockShard.transform.localScale.x),
            blockShard.transform.position.y,
            blockShard.transform.position.z);
        blockShard.AddComponent<Rigidbody>();
        blockShard.GetComponent<Rigidbody>().useGravity = true;
    }

    private void SplitBlockByZ()
    {
        GameObject blockShard = Instantiate(currentBlock);
        blockShard.transform.parent = blocksCategory.transform;

        blockShard.transform.localScale = new Vector3(blockShard.transform.localScale.x, 
            blockShard.transform.localScale.y,
            Math.Abs(lastBlock.transform.position.z - blockShard.transform.position.z));

        currentBlock.transform.localScale = new Vector3(currentBlock.transform.localScale.x, 
            currentBlock.transform.localScale.y,
            currentBlock.transform.localScale.z - Math.Abs(lastBlock.transform.position.z - currentBlock.transform.position.z));

        currentBlock.transform.position = new Vector3(currentBlock.transform.position.x, 
            currentBlock.transform.position.y,
            lastBlock.transform.position.z * 0.5f + currentBlock.transform.position.z * 0.5f);

        int positionTemp = 1;
        if (currentBlock.transform.position.z < lastBlock.transform.position.z)
            positionTemp *= -1;
        blockShard.transform.position = new Vector3(blockShard.transform.position.x,
            blockShard.transform.position.y,
            lastBlock.transform.position.z + positionTemp * (currentBlock.transform.localScale.z * 0.5f + blockShard.transform.localScale.z)); ;
        blockShard.AddComponent<Rigidbody>();
        blockShard.GetComponent<Rigidbody>().useGravity = true;
    }
}
