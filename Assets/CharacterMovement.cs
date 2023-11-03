using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody character_body;
    public Camera pc_camera;

    public float SWIM_FORCE = 5.0f;
    public float PC_CAMERA_SPEED = 1.0f;

    private float camera_pitch = 0.0f;
    private float camera_yaw = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camera_yaw = PC_CAMERA_SPEED * Input.GetAxis("Mouse X");
        camera_pitch -= PC_CAMERA_SPEED * Input.GetAxis("Mouse Y");
        camera_pitch = Mathf.Clamp(camera_pitch, 10.0f, 39.9f);

        Vector3 camera_angles = pc_camera.transform.eulerAngles;
        camera_angles.x = camera_pitch;
        pc_camera.transform.eulerAngles = camera_angles;
        character_body.AddTorque(Vector3.up * PC_CAMERA_SPEED * Input.GetAxis("Mouse X"));
        //transform.eulerAngles = new Vector3(0.0f, camera_yaw, 0.0f);


        if (Input.GetKey(KeyCode.A))
        {
            character_body.AddForce(-SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            character_body.AddForce(-SWIM_FORCE * transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            character_body.AddForce(SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.W))
        {
            character_body.AddForce(SWIM_FORCE * transform.forward);
        }
    }
}
