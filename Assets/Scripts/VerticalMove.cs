using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMove : MonoBehaviour
{
    public float move_frequency = 1.0f;
    public float move_amplitude = 0.1f;
    private float _t;
    private Vector3 _position;

    // Start is called before the first frame update
    void Start()
    {
        _position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_t > (1f / move_frequency))
        {
            _t = 0.0f;
        }
        else
        {
            _t += Time.deltaTime;
        }
        float om = 2 * Mathf.PI * move_frequency;
        float phase = 0.0f;
        transform.position = _position + move_amplitude * Mathf.Sin(om * _t + phase) * Vector3.up;
    }
}
