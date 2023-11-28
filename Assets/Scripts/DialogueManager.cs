using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public string[] dialogue;
}

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogue_label;
    public CharacterMovement pc_controller;
    public HUD game_hud;

    public string[] dialogue_string;
    private string dialogue_name;
    private int cur_dialogue_position = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadJSON(TextAsset json_asset)
    {
        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(json_asset.text);
        dialogue_string = dialogue.dialogue;
        dialogue_name = dialogue.name;
    }

    public void switchNextDialogue()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        cur_dialogue_position++;
        if (cur_dialogue_position == dialogue_string.Length)
        {
            closeDialogue();
            return;
        }

        dialogue_label.text = dialogue_string[cur_dialogue_position];
    }

    public void startDialogue()
    {
        game_hud.dialogue_on = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        cur_dialogue_position = 0;
        dialogue_label.text = dialogue_string[cur_dialogue_position];
    }

    public void closeDialogue()
    {
        game_hud.dialogue_on = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pc_controller.setControllable(true);
        gameObject.SetActive(false);

        print(dialogue_name);
        if (dialogue_name == "bitten_fish")
        {
            pc_controller.eatThatSpecificFish();
        }
    }
}
