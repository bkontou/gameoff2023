using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RawImage hunger_img;
    public RawImage boost_img;
    public TextMeshProUGUI scales_collected_text;

    public Texture[] hunger_textures;
    public Texture[] boost_textures;

    public TextMeshProUGUI death_text;
    public GameObject restart_button;
    public GameObject quit_button;

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

    public void setNumScalesText(int num)
    {
        scales_collected_text.text = "x " + num.ToString();
    }

    public void onPCDeath() 
    {
        death_text.alpha = 1;
        restart_button.SetActive(true);
        quit_button.SetActive(true);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
