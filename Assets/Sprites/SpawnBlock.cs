using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
//using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Blocks = new GameObject[9];
    public float[,] blockData;
    public GameObject blockPrefab;
    public int currentBlockType;
    public int currentBlockColor;
    public int currentBlockDirection;
    public float currentInterval;
    public int currentPosType;
    public Vector3 currentPosition;
    public Transform enemyControllerTransform;
    public EnemyController ecScript;

    // Start is called before the first frame update
    void Start()
    {  
        enemyControllerTransform = GameObject.Find("EnemyController").transform;

        // Read File
        TextAsset file = PlayerSetData.musicSheetDatafile;
        if (file != null)
        {
            // Split file into lines and columns
            string[] lines = file.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;

            // create 2D array to store data
            blockData = new float[rows, cols];

            // Loop through lines and columns
            for (int i = 0; i < rows; i++)
            {
                // Remove possible extra spaces and split on commas
                string[] values = lines[i].Trim().Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < cols; j++)
                {
                    float result;
                    // Try to parse into an integer and store into an array, throw an error if parsing fails
                    if (float.TryParse(values[j].Trim(), out result))
                    {
                        blockData[i, j] = result;
                    }
                }
            }
            
            ecScript.blockNumber = blockData.GetLength(0); 
        }

        StartCoroutine(SpawnBlocks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnBlocks(){
        /* 
         * BlockType: 0 - block, 1 - bomb
         * Color: 0 - blue, 1 - red
         * Direction: 0 - up, 1 - down, 2 - left, 3 - right
         * Interval: Block generation interval
         * PositionType: 0 - top right, 1 - top left, 2 - bottom left, 3 - bottom right
        */

        int rows = blockData.GetLength(0);
        for (int i = 0; i < rows; i++)
        {
            while (EnvironmentController.isPaused)
            {
                yield return null; // Pause the game if it is paused
            }
            
            currentBlockType = (int)blockData[i, 0];
            currentBlockColor = (int)blockData[i, 1];
            currentBlockDirection = (int)blockData[i, 2];
            currentInterval = blockData[i, 3];
            currentPosType = (int)blockData[i, 4];

            if(currentBlockType == 0 && currentBlockColor == 0){
                switch(currentBlockDirection){
                    case 0: blockPrefab = Blocks[0]; break;
                    case 1: blockPrefab = Blocks[1]; break;
                    case 2: blockPrefab = Blocks[2]; break; 
                    case 3: blockPrefab = Blocks[3]; break;
                }
            }
            
            if(currentBlockType == 0 && currentBlockColor == 1){
                switch(currentBlockDirection){
                    case 0: blockPrefab = Blocks[4]; break;
                    case 1: blockPrefab = Blocks[5]; break;
                    case 2: blockPrefab = Blocks[6]; break; 
                    case 3: blockPrefab = Blocks[7]; break;
                }
            }

            if(currentBlockType == 1){
                blockPrefab = Blocks[8];
            }

            switch(currentPosType){
                case 0: currentPosition = transform.position + new Vector3(0.1f, 0.1f, 0); break;
                case 1: currentPosition = transform.position + new Vector3(-0.1f, 0.1f, 0); break;
                case 2: currentPosition = transform.position + new Vector3(0.1f, -0.1f, 0); break;
                case 3: currentPosition = transform.position + new Vector3(-0.1f, -0.1f, 0); break;
            }

            // Wait for a specified time interval
            //Debug.Log("Spawning Block:" + currentInterval);
            yield return new WaitForSeconds(currentInterval);

            // Generate Blocks
            GameObject newBlock = Instantiate(blockPrefab, currentPosition, Quaternion.identity, enemyControllerTransform);

            if(currentBlockType == 0){newBlock.transform.Find("Body").GetComponent<Block>().posType = currentPosType;}
            if(currentBlockType == 1){newBlock.transform.Find("Body").GetComponent<Bomb>().posType = currentPosType;}

            if(currentPosType == 0 || currentPosType == 2){
                ecScript.rightObjList.Add(newBlock);
            }

            if(currentPosType == 1 || currentPosType == 3){
                ecScript.leftObjList.Add(newBlock);
            }
        }
    }
}
