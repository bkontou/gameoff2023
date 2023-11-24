using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FishBob : MonoBehaviour
{
    public float fish_bob_amplitude = 0.5f;
    public float fish_bob_frequency = 60f;
    public float max_rotation_angle = 30f;
    public bool is_backwards = false;
    public bool auto_fwd_dir = true;

    private Vector3 inital_pos;
    public Vector3 initial_fwd_dir;
    private Vector3 initial_up_dir;
    private float t = 0.0f;

    public float hmmm;
    public Transform pc;

    // Start is called before the first frame update
    void Start()
    {
        inital_pos = transform.position;
        if (auto_fwd_dir )
        {
            initial_fwd_dir = Quaternion.Euler(transform.eulerAngles) * transform.forward;
        }
        initial_up_dir = Quaternion.Euler(transform.eulerAngles) * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (t > (1f / fish_bob_frequency))
        {
            t = 0.0f;
        } else
        {
            t += Time.deltaTime;
        }
        float om = 2 * Mathf.PI * fish_bob_frequency;
        float phase = 0.0f;
        transform.position = inital_pos + fish_bob_amplitude * Mathf.Sin(om * t + phase) * Vector3.up;


        //transform.LookAt(pc.position);
        Vector3 dir_to_pc = (transform.position - pc.position).normalized;
        dir_to_pc.y = 0.0f;
        Vector3 cur_fwd_dir = Quaternion.Euler(transform.eulerAngles) * transform.forward;
        float total_angle_to_pc = Vector3.SignedAngle(-initial_fwd_dir, dir_to_pc, Vector3.up);
        
        float angle_to_pc = Vector3.SignedAngle(-cur_fwd_dir, dir_to_pc, Vector3.up);

        if (is_backwards)
        {
            angle_to_pc = Vector3.SignedAngle(cur_fwd_dir, dir_to_pc, Vector3.up);
        }

        float angle_to_rotate;
        hmmm = angle_to_pc;
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
