using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FisheyeOnScreenImage : MonoBehaviour
{
    private void Awake()
    {
        Material material = GetComponent<RawImage>().material;
        material.SetTexture("texture", GetComponent<RawImage>().texture);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
