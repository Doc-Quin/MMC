using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] Blocks = new GameObject[9];
    public int[,] blockData;
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

        // 读取文件内容
        TextAsset file = Resources.Load<TextAsset>("data");
        if (file != null)
        {
            // 按行分割文件内容
            string[] lines = file.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            int rows = lines.Length;
            int cols = lines[0].Split(',').Length;

            // 创建二维数组
            blockData = new int[rows, cols];

            // 解析文件内容并存储到二维数组中
            for (int i = 0; i < rows; i++)
            {
                // 去除可能的额外空格并按逗号分割
                string[] values = lines[i].Trim().Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < cols; j++)
                {
                    int result;
                    // 尝试解析为整数并存储到数组中，如果解析失败则抛出错误
                    if (int.TryParse(values[j].Trim(), out result))
                    {
                        blockData[i, j] = result;
                    }
                }
            }
        }
        StartCoroutine(SpawnBlocks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnBlocks(){
        /* Type: 0 - block, 1 - bomb
         * Color: 0 - red, 1 - blue
         * Direction: 0 - up, 1 - down, 2 - left, 3 - right
         * Interval: 方块生成间隔时间
         * PositionType: 0 - top left, 1 - top right, 2 - bottom left, 3 - bottom right
        */
        int rows = blockData.GetLength(0);
        for (int i = 0; i < rows; i++)
        {
            currentBlockType = blockData[i, 0];
            currentBlockColor = blockData[i, 1];
            currentBlockDirection = blockData[i, 2];
            currentInterval = blockData[i, 3];
            currentPosType = blockData[i, 4];

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

            // 等待指定的时间间隔
            yield return new WaitForSeconds(currentInterval);

            // 生成方块
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
