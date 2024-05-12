using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorller : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10f;

    public float offsetX = 100;
    public float offsetY = 100;
    public float offsetZ = 100; 
    public float offsetmX = 100;
    public float offsetmY = 100;
    public float offsetmZ = 100;

    public float zoomSpeed = 10f;
    // Update is called once per frame

    public Vector3 basePos;

    private void Start()
    {
      //  basePos = new Vector3(50, 20, 100);
    }
    public void Zoom()
    {
        float distance =Input.GetAxis("Mouse ScrollWheel")*-1*zoomSpeed;
        if(distance != 0)
        {
            transform.position+=new Vector3(0,distance,0);
        }
    }

    void Update()
    {

        Vector3 inputDir = new Vector3(0,0,0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        Vector3 moveDir = transform.forward * inputDir.z+transform.right*inputDir.x;
        transform.position += moveDir * _moveSpeed * Time.deltaTime;

        Zoom();
        
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, basePos.x - offsetmX, basePos.x+offsetX),
            Mathf.Clamp(transform.position.y, 5, offsetY),
            Mathf.Clamp(transform.position.z, basePos.z - offsetmZ, basePos.z+offsetZ)
            );
    }
}
