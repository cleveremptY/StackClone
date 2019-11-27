using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject currentBlock;
    public GameObject lastBlock;
    public float startDistance;
    public float borderDistance;
    public float speedBlock = 10f;

    private int Score = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentBlock.transform.Translate(new Vector3(borderDistance, 0, 0) * Time.deltaTime * speedBlock);
        if (speedBlock > 0 && currentBlock.transform.position.x > lastBlock.transform.position.x + borderDistance)
        {
            speedBlock *= -1;
            Debug.Log("Direction change: Back");
        }
        if (speedBlock < 0 && currentBlock.transform.position.x < lastBlock.transform.position.x - borderDistance)
        {
            speedBlock *= -1;
            Debug.Log("Direction change: Foward");
        }

        if (Input.GetMouseButtonDown(0))
        {
            NewBlock();
            speedBlock = 0;
        }
         

    }

    private void NewBlock()
    {
        GameObject blockShard = Instantiate(currentBlock);
        blockShard.transform.localScale = new Vector3(Math.Abs(lastBlock.transform.position.x - blockShard.transform.position.x), blockShard.transform.localScale.y, blockShard.transform.localScale.z);
        
        currentBlock.transform.localScale = new Vector3(currentBlock.transform.localScale.x - Math.Abs(lastBlock.transform.position.x - currentBlock.transform.position.x), currentBlock.transform.localScale.y, currentBlock.transform.localScale.z);
        currentBlock.transform.position = new Vector3(lastBlock.transform.position.x * 0.5f + currentBlock.transform.position.x * 0.5f, currentBlock.transform.position.y, currentBlock.transform.position.z);

        int positionTemp = 1;
        if (currentBlock.transform.position.x < 0)
            positionTemp *= -1;
        blockShard.transform.position = new Vector3(lastBlock.transform.position.x + positionTemp * (currentBlock.transform.localScale.x * 0.5f + blockShard.transform.localScale.x),
            blockShard.transform.position.y,
            blockShard.transform.position.z);
        blockShard.AddComponent<Rigidbody>();
        blockShard.GetComponent<Rigidbody>().useGravity = true;
    }
}
