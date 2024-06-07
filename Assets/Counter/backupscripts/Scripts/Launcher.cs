using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float turnSpeed = 5f;
    private float rotationLimit = 45f;

    private float horizontalInput;
    private float verticalInput;

    private float horizontalRotation;
    private float verticalRotation;


    // Start is called before the first frame update
    void Start()
    {
        horizontalRotation = 0;
        verticalRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Mathf.Clamp locks the value between two given values
        horizontalRotation += (Time.deltaTime * turnSpeed * horizontalInput);
        horizontalRotation = Mathf.Clamp(horizontalRotation, -rotationLimit, rotationLimit);

        verticalRotation += (Time.deltaTime * -turnSpeed * verticalInput);
        verticalRotation = Mathf.Clamp(verticalRotation, -rotationLimit, rotationLimit);

        //set the position as our clamped rotation values
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, horizontalRotation, verticalRotation);

    }
}
