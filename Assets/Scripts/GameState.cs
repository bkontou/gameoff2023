using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    public int num_fish_eaten = 0;
    private int _num_scales_collected = 0;
    public int num_scales_collected
    {
        get => _num_scales_collected;
        set
        {
            _num_scales_collected = value;
            game_hud.setNumScalesText(value);   
        }
    }

    public TextAsset fish_dialogue;
    public TextAsset fish_guy_1_dialogue;
    public TextAsset fish_guy_2_dialogue;
    public TextAsset fish_guy_3_dialogue;
    public TextAsset horseshoe_crab_dialogue;
    public TextAsset blobfish_dialogue;
    public TextAsset crab_dialogue;
    public TextAsset fish_gang_dialogue;
    public TextAsset pufferfish_dialogue;
    public TextAsset sad_fish_dialogue;
    public TextAsset paranoid_fish_1_dialogue;
    public TextAsset paranoid_fish_2_dialogue;
    public HUD game_hud;

    public FishAI fish1;
    public FishAI fish2;
    public FishAI fish3;
    public FishAI fish4;
    public FishAI fish5;

    public CharacterMovement pc;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDifficultyHard()
    {
        fish1.attack_timeout = 1f;
        fish2.attack_timeout = 1f;
        fish3.attack_timeout = 1f;
        fish4.attack_timeout = 1f;
        fish5.attack_timeout = 1f;
        pc.BOOST_REFRESH_RATE = 0.5f;
        pc.HUNGER_RATE = 0.0625f;
    }

    public void changeDifficultyNormal()
    {
        fish1.attack_timeout = 2.5f;
        fish2.attack_timeout = 2.5f;
        fish3.attack_timeout = 2.5f;
        fish4.attack_timeout = 2.5f;
        fish5.attack_timeout = 2.5f;
        pc.BOOST_REFRESH_RATE = 1f;
        pc.HUNGER_RATE = 0.0125f;
    }

    public void changeDifficultyEasy()
    {
        fish1.attack_timeout = 4.5f;
        fish2.attack_timeout = 4.5f;
        fish3.attack_timeout = 4.5f;
        fish4.attack_timeout = 4.5f;
        fish5.attack_timeout = 4.5f;
        pc.BOOST_REFRESH_RATE = 2.5f;
        pc.HUNGER_RATE = 0.005f;
    }
}
