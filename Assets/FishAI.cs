using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishAI : MonoBehaviour
{

    public Transform pc;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir_to_pc = (pc.position - transform.position).normalized;
        dir_to_pc.y = 0;
       
        transform.position += 3.0f * Time.deltaTime * dir_to_pc;
    }
}
