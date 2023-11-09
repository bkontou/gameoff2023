using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RawImage hunger_img;
    public RawImage boost_img;

    public Texture[] hunger_textures;
    public Texture[] boost_textures;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHungerLevel(int level)
    {
        hunger_img.texture = hunger_textures[level];
    }

    public void setBoostLevel(int level)
    {
        boost_img.texture = boost_textures[level];
    }
}
