using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody character_body;
    public Camera pc_camera;

    public float MAX_SPEED = 5.0f;
    public float SWIM_FORCE = 5.0f;
    public float LATERAL_SWIM_FORCE = 1.0f;
    public float BACKWARD_SWIM_FORCE = 1.0f;
    public float PC_CAMERA_SPEED = 1.0f;

    public float BOOST_SPEED = 5.0f;
    public float boost_cooldown = 1.0f;
    public float boost_duration = 1.0f;
    private bool boost_on = false;
    private bool boost_on_cooldown = false;
    private Vector3 boost_dir = Vector3.zero;
    private float boost_timer = 0.0f;
    private float boost_cooldown_timer = 0.0f;

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
            character_body.AddForce(-LATERAL_SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            character_body.AddForce(-BACKWARD_SWIM_FORCE * transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            character_body.AddForce(LATERAL_SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.W))
        {
            character_body.AddForce(SWIM_FORCE * transform.forward);
        }

        if ((Input.GetKey(KeyCode.LeftShift)) && boost_on_cooldown)
        {
            print("Boost is on cooldown!");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !boost_on && !boost_on_cooldown)
        {
            boost_on = true;
            boost_on_cooldown = true;
            boost_timer = 0.0f;
            boost_cooldown_timer = 0.0f;
            boost_dir = transform.forward;
        }

        if (boost_on_cooldown)
        {
            boost_cooldown_timer += Time.deltaTime;
            if (boost_cooldown_timer > boost_cooldown)
            {
                boost_on_cooldown = false;
            }
        }

        if (boost_on)
        {
            character_body.AddForce(BOOST_SPEED * boost_dir);
            //character_body.velocity = BOOST_SPEED * boost_dir;
            boost_timer += Time.deltaTime;

            if (boost_timer > boost_duration)
            {
                boost_on = false;
            }
        } else
        {
            character_body.velocity = Vector3.MoveTowards(character_body.velocity, Vector3.ClampMagnitude(character_body.velocity, MAX_SPEED), Time.deltaTime);
        }
    }
}
