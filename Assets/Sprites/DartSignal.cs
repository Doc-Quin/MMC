using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DartSignal : MonoBehaviour
{
    public bool ifHit;
    public int focusBorderType;
    public GameObject focusObject;
    public Rigidbody dartBody;
    public int directionMessage;
    public float rotationSpeed; // 旋转速度
    public float initialSpeed;    // 初始飞行速度
    public float curveAmount;  // 弧线偏移量
    public int offsetDirection;
    public Camera camera;
    public GameObject XRController;
    public Vector3 focusPos;
    public EnemyController ecScript;

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = 100f;
        initialSpeed = 1.5f;
        curveAmount = 0.5f; // 调整曲线偏移量
        
        if(ifHit){

            ecScript = GameObject.Find("EnemyController").GetComponent<EnemyController>();

            if(focusBorderType == 0){
                focusObject = ecScript.currentRightObj;
            }

            if(focusBorderType == 1){
                focusObject = ecScript.currentLeftObj;
            }
        }
        

        // 给予飞镖一个初始速度
        Vector3 initialDirection = camera.transform.forward;
        dartBody.velocity = initialDirection * initialSpeed;

        // 根据 directionMessage 设置垂直速度
        Vector3 verticalSpeed = Vector3.zero;
        switch (directionMessage)
        {
            case 0: // 上
                verticalSpeed = camera.transform.up;
                break;
            case 1: // 下
                verticalSpeed = -camera.transform.up;
                break;
            case 2: // 左
                verticalSpeed = -camera.transform.right;
                break;
            case 3: // 右
                verticalSpeed = camera.transform.right;
                break;
        }

        // 给予飞镖一个较小的垂直速度
        dartBody.velocity += verticalSpeed * curveAmount * initialSpeed * 0.3f;

        // 获取偏移方向速度
        Vector3 offsetDir = Vector3.zero;
        switch (offsetDirection)
        {
            case 0: // 右上
                offsetDir = camera.transform.up + camera.transform.right;
                // 沿Z轴逆时针旋转45度
                transform.Rotate(0, 0, 45f);
                break;
            case 1: // 左上
                offsetDir = camera.transform.up - camera.transform.right;
                transform.Rotate(0, 0, -45f);
                break;
            case 2: // 左下
                offsetDir = -camera.transform.up - camera.transform.right;
                transform.Rotate(0, 0, 45f);
                break;
            case 3: // 右下
                offsetDir = -camera.transform.up + camera.transform.right;
                transform.Rotate(0, 0, -45f);
                break;
        }
        
        // 冻结刚体的Z轴旋转
        dartBody.constraints = RigidbodyConstraints.FreezeRotationZ;

        // 给予飞镖一个较小的偏移方向的速度
        dartBody.velocity += offsetDir.normalized * initialSpeed * curveAmount * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(dartBody != null){

            // 获取旋转后的 up 方向
            Vector3 rotatedUp = transform.TransformDirection(Vector3.up);
        
            // 添加扭矩
            dartBody.AddTorque(rotatedUp * rotationSpeed * Time.deltaTime, ForceMode.VelocityChange);

            if(focusObject != null){

                focusPos = focusObject.transform.position;
                // 计算飞向目标的方向
                Vector3 directionToTarget = (focusObject.transform.position - transform.position).normalized;

                // 逐渐调整飞镖的速度，以跟踪目标物体
                Vector3 newVelocity = Vector3.Lerp(dartBody.velocity, directionToTarget * initialSpeed, Time.deltaTime * 4); // Fast lerp speed.
                dartBody.velocity = newVelocity;
            }else{
                Vector3 directionToTarget = (XRController.transform.position + new Vector3(0, 0.5f, 3f) - transform.position).normalized;
                Vector3 newVelocity = Vector3.Lerp(dartBody.velocity, directionToTarget * initialSpeed, Time.deltaTime * 4); // Fast lerp speed.
                dartBody.velocity = newVelocity;
                if (transform.position.z >= 2)
                {        
                    Destroy(gameObject); // Destory dart object when it goes out of the screen.
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject != XRController){
            Destroy(gameObject);
        }
    }
}
