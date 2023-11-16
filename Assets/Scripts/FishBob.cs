using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBob : MonoBehaviour
{
    public float fish_bob_amplitude = 0.5f;
    public float fish_bob_frequency = 60f;

    private Vector3 inital_pos;
    private float t = 0.0f;

    public Transform pc;

    // Start is called before the first frame update
    void Start()
    {
        inital_pos = transform.position;
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
    }
}
