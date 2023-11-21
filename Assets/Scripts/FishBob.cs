using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBob : MonoBehaviour
{
    public float fish_bob_amplitude = 0.5f;
    public float fish_bob_frequency = 60f;
    public float max_rotation_angle = 30f;

    private Vector3 inital_pos;
    private Vector3 initial_fwd_dir;
    private Vector3 initial_up_dir;
    private float t = 0.0f;

    public Transform pc;

    // Start is called before the first frame update
    void Start()
    {
        inital_pos = transform.position;
        initial_fwd_dir = Quaternion.Euler(transform.eulerAngles) * transform.forward;
        initial_up_dir = Quaternion.Euler(transform.eulerAngles) * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > 2 * Mathf.PI)
        {
            t = 0.0f;
        } else
        {
            t += Time.deltaTime;
        }
        transform.position = inital_pos + fish_bob_amplitude * Mathf.Sin(fish_bob_frequency * t) * Vector3.up;


        //transform.LookAt(pc.position);
        Vector3 dir_to_pc = (transform.position - pc.position).normalized;
        dir_to_pc.y = 0.0f;
        Vector3 cur_fwd_dir = Quaternion.Euler(transform.eulerAngles) * transform.forward;
        float total_angle_to_pc = Vector3.SignedAngle(-initial_fwd_dir, dir_to_pc, Vector3.up);
        float angle_to_pc = Vector3.SignedAngle(-cur_fwd_dir, dir_to_pc, Vector3.up);
        print(angle_to_pc.ToString() + transform.gameObject.name);
        float angle_to_rotate;

        if (max_rotation_angle >= 180)
        {
            angle_to_rotate = Mathf.Lerp(0f, angle_to_pc, 0.1f);
        }
        else
        {
            float damping_val = Mathf.Clamp01(-Mathf.Abs(total_angle_to_pc / max_rotation_angle) + 1.0f);
            angle_to_pc *= damping_val;
            if (damping_val > 0.0f)
            {
                angle_to_rotate = Mathf.Lerp(0f, angle_to_pc, 0.1f);
            }
            else
            {
                angle_to_rotate = 0f;
            }
        }

        transform.Rotate(Vector3.forward, angle_to_rotate);

        //transform.SetPositionAndRotation(transform.position, Quaternion.AngleAxis(angle_to_pc, Vector3.up));
    }
}
