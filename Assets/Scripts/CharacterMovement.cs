using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Rigidbody character_body;
    public Camera pc_camera;
    public HUD game_HUD;
    public Light pc_area_light;
    public Light pc_lower_light;
    public DialogueManager dialogue_manager;
    public TextMeshProUGUI dialogue_label;
    public GameObject bitten_fish;
    public AudioSource munch_audio;

    private int _hunger_level = 6;
    public int hunger_level
    {
        get => _hunger_level;
        set
        {
            _hunger_level = value;
            if (_hunger_level < 0)
            {
                _hunger_level = 0;
            }
            if (_hunger_level > 6)
            {
                _hunger_level = 6;
            }
            game_HUD.setHungerLevel(_hunger_level);
        }
    }

    private int _boost_level = 5;
    public int boost_level
    {
        get => _boost_level;
        set
        {
            _boost_level = value;
            if (_boost_level < 0)
            {
                _boost_level = 0;
            }
            if (_boost_level > 5)
            {
                _boost_level = 5;
            }
            game_HUD.setBoostLevel(_boost_level);
        }
    }

    public float MAX_SPEED = 5.0f;
    public float SWIM_FORCE = 5.0f;
    public float LATERAL_SWIM_FORCE = 1.0f;
    public float BACKWARD_SWIM_FORCE = 1.0f;
    public float PC_CAMERA_SPEED = 1.0f;

    public float HUNGER_RATE = 0.5f; // HUNGER DECREMENT PER SEC
    private float hunger_rate_timer = 0;
    public float hunger_death_timer = 10.0f;
    public float pc_light_level = 10.0f;
    public float pc_lower_light_level = 0.25f;

    public float BOOST_REFRESH_RATE = 2.0f; // boost increment per sec
    private float boost_rate_timer = 0;
    public float boost_hunger_percentage = 0.25f;

    public float BOOST_SPEED = 5.0f;
    public float boost_duration = 1.0f;
    private bool boost_on = false;
    private bool boost_on_cooldown = false;
    private Vector3 boost_dir = Vector3.zero;
    private float boost_timer = 0.0f;
    private float boost_cooldown_timer = 0.0f;

    private float camera_pitch = 0.0f;
    private float camera_yaw = 0.0f;

    private bool controllable = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        hunger_level = 3;
        boost_level = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!controllable)
        {
            return;
        }

        // Uh oh! You may die!
        if (hunger_level == 0)
        {
            pc_area_light.intensity = Mathf.MoveTowards(pc_area_light.intensity, 0, (pc_light_level / hunger_death_timer) * Time.deltaTime);
            pc_lower_light.intensity = Mathf.MoveTowards(pc_lower_light.intensity, 0, (pc_lower_light_level / hunger_death_timer) * Time.deltaTime);

            if (pc_area_light.intensity <= 0)
            {
                controllable = false;
                GameState.Instance.game_hud.onPCDeath();
            }
        }

        camera_yaw = PC_CAMERA_SPEED * Input.GetAxis("Mouse X");
        camera_pitch -= PC_CAMERA_SPEED * Input.GetAxis("Mouse Y");
        print(camera_pitch);
        camera_pitch = Mathf.Clamp(camera_pitch, 12.0f, 70f);

        Vector3 camera_angles = pc_camera.transform.eulerAngles;
        camera_angles.x = camera_pitch;
        pc_camera.transform.eulerAngles = camera_angles;
        character_body.AddTorque(Vector3.up * PC_CAMERA_SPEED * Input.GetAxis("Mouse X"));
        //transform.eulerAngles = new Vector3(0.0f, camera_yaw, 0.0f);


        if (Input.GetKey(KeyCode.A))
        {
            character_body.AddForce(-LATERAL_SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            character_body.AddForce(-BACKWARD_SWIM_FORCE * transform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            character_body.AddForce(LATERAL_SWIM_FORCE * transform.right);
        }
        if (Input.GetKey(KeyCode.W))
        {
            character_body.AddForce(SWIM_FORCE * transform.forward);
        }

        if ((Input.GetKey(KeyCode.LeftShift)) && boost_on_cooldown)
        {
            print("Boost is on cooldown!");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit[] hits;
            Ray ray = pc_camera.ScreenPointToRay(new Vector2(0.5f * Screen.width, 0.5f * Screen.height));
            hits = Physics.RaycastAll(ray, 5.0f);
            handleInteractions(hits);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !boost_on && !boost_on_cooldown)
        {
            boost_on = true;
            boost_on_cooldown = true;
            boost_timer = 0.0f;
            boost_cooldown_timer = 0.0f;
            boost_dir = transform.forward;
            boost_level = 0;

            hunger_rate_timer += boost_hunger_percentage;
        }

        if (boost_level == 5)
        {
            boost_on_cooldown = false;
        }

        if (boost_on)
        {
            character_body.AddForce(BOOST_SPEED * boost_dir);
            //character_body.velocity = BOOST_SPEED * boost_dir;
            boost_timer += Time.deltaTime;

            if (boost_timer > boost_duration)
            {
                boost_on = false;
            }
        } else
        {
            character_body.velocity = Vector3.MoveTowards(
                character_body.velocity,
                Vector3.ClampMagnitude(character_body.velocity, MAX_SPEED),
                5 * Time.deltaTime);
            // character_body.velocity = Vector3.ClampMagnitude(character_body.velocity, MAX_SPEED);
        }

        // Update hunger
        if (hunger_rate_timer >= 1.0f)
        {
            hunger_level--;
            hunger_rate_timer = 0.0f;
        } else
        {
            hunger_rate_timer += HUNGER_RATE * Time.deltaTime;
        }

        // Update boost
        if (boost_rate_timer >= 1.0f)
        {
            boost_level++;
            boost_rate_timer = 0.0f;
        } else
        {
            boost_rate_timer += BOOST_REFRESH_RATE * Time.deltaTime;
        }
    }


    private void handleInteractions(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            string name = hit.transform.gameObject.name;
            print(name);
            switch (name)
            {
                case "DeadFish":
                    Destroy(hit.transform.gameObject);
                    GameState.Instance.num_fish_eaten++;
                    pc_area_light.intensity = pc_light_level;
                    pc_lower_light.intensity = pc_lower_light_level;
                    hunger_level = 6;
                    munch_audio.Play();
                    break;
                case "Scale":
                    Destroy(hit.transform.gameObject);
                    GameState.Instance.num_scales_collected++;
                    break;
                case "fish_bitten":
                    controllable = false;
                    dialogue_manager.loadJSON(GameState.Instance.fish_dialogue);
                    dialogue_manager.startDialogue();
                    //Cursor.lockState = CursorLockMode.Locked;
                    break;
                case "FishGuy1":
                    controllable = false;
                    dialogue_manager.loadJSON(GameState.Instance.fish_guy_1_dialogue);
                    dialogue_manager.startDialogue();
                    break;
                case "FishGuy2":
                    controllable = false;
                    dialogue_manager.loadJSON(GameState.Instance.fish_guy_2_dialogue);
                    dialogue_manager.startDialogue();
                    break;
                case "HorseshoeCrab":
                    controllable = false;
                    dialogue_manager.loadJSON(GameState.Instance.horseshoe_crab_dialogue);
                    dialogue_manager.startDialogue();
                    break;
                case "Blobfish":
                    controllable = false;
                    dialogue_manager.loadJSON(GameState.Instance.blobfish_dialogue);
                    dialogue_manager.startDialogue();
                    break;
                default:
                    break;
            }
        }
    }

    public void setControllable(bool c)
    {
        controllable = c;
    }

    public void eatThatSpecificFish()
    {
        Destroy(bitten_fish);
        munch_audio.Play();
        GameState.Instance.num_fish_eaten++;
        hunger_level = 6;
    }
}
