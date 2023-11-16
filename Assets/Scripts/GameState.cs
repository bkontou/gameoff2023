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
    public TextAsset horseshoe_crab_dialogue;
    public HUD game_hud;

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
}
