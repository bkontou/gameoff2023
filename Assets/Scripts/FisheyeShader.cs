using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FisheyeShader : MonoBehaviour
{

    [SerializeField]
    private Shader shader;
    private Material material;

    public float fisheye_power = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        // Create a new material with the supplied shader.
        material = new Material(shader);
        material.SetFloat("fisheye_power", fisheye_power);
    }

    // OnRenderImage() is called when the camera has finished rendering.
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        print("huh??");
        //Graphics.Blit(src, dst, material);
    }
}
