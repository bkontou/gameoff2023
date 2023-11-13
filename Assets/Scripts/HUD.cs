using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject pc_object;
    public RawImage hunger_img;
    public RawImage boost_img;
    public TextMeshProUGUI scales_collected_text;

    public Texture[] hunger_textures;
    public Texture[] boost_textures;

    public TextMeshProUGUI death_text;
    public GameObject restart_button;
    public GameObject quit_button;
    public GameObject pause_menu;

    private bool game_paused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (game_paused)
            {
                Cursor.visible = false;
                pc_object.GetComponent<CharacterMovement>().setControllable(true);
                pause_menu.SetActive(false);
                Time.timeScale = 1.0f;
                game_paused = false;
            }
            else {
                Cursor.visible = true;
                pc_object.GetComponent<CharacterMovement>().setControllable(false);
                pause_menu.SetActive(true);
                Time.timeScale = 0.0f;
                game_paused = true;
            }
        }
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
        Cursor.visible = true;
        death_text.alpha = 1;
        restart_button.SetActive(true);
        quit_button.SetActive(true);
    }

    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitToMain()
    {
        SceneManager.LoadScene("Scenes/Start");
    }

    public void unpause()
    {
        if (game_paused) {
            Cursor.visible = false;
            pc_object.GetComponent<CharacterMovement>().setControllable(true);
            game_paused = false;
            pause_menu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
