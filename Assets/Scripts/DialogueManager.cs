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

    public TextAsset dialogue_asset;


    public string[] dialogue_string;
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
    }

    public void switchNextDialogue()
    {
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
        gameObject.SetActive(true);
        cur_dialogue_position = 0;
        dialogue_label.text = dialogue_string[cur_dialogue_position];
    }

    public void closeDialogue()
    {
        pc_controller.setControllable(true);
        gameObject.SetActive(false);

        if (dialogue_asset.name == "fish")
        {
            pc_controller.eatThatSpecificFish();
        }
    }
}
