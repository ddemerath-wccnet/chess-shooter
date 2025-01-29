using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool spaceBuffer;
    Vector3 moveDir;
    public float speed = 1;
    public GameObject mouseDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.Space)) spaceBuffer = true;
        if (Input.GetKey(KeyCode.W)) moveDir += Vector3.up;
        if (Input.GetKey(KeyCode.A)) moveDir += Vector3.left;
        if (Input.GetKey(KeyCode.S)) moveDir += Vector3.down;
        if (Input.GetKey(KeyCode.D)) moveDir += Vector3.right;

        moveDir.Normalize();

        transform.position += speed * Time.deltaTime * moveDir;

        Vector3 target = Input.mousePosition;
        target.z = 10.0f;
        target = Camera.main.ScreenToWorldPoint(target);
        target = target - transform.position;
        target.z = Vector2.SignedAngle(Vector2.up, new Vector2(target.x, target.y).normalized);
        mouseDir.transform.eulerAngles = new Vector3(0, 0, target.z);
    }
}
