using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    public float fish_spawn_height = 0.0f;
    public float fish_spawn_chance = 0.25f;
    public float spawn_timer_timeout = 1.0f;
    private float spawn_timer = 0.0f;
    public GameObject[] fish_objects;
    public GameObject[] spawners;
    private GameObject[] spawns;

    // Start is called before the first frame update
    void Start()
    {
        spawns = new GameObject[spawners.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn_timer >= spawn_timer_timeout)
        {
            spawnFish();
            spawn_timer = 0.0f;
        } else
        {
            spawn_timer += Time.deltaTime;
        }
    }

    void spawnFish()
    {
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawns[i] != null)
            {
                continue;
            }

            if (Random.value < 1.0f - fish_spawn_chance)
            {
                continue;
            }

            Vector3 spawn_pos = spawners[i].transform.position;
            spawn_pos.y = fish_spawn_height;

            print("spawning fish!");
            GameObject fish = Instantiate(fish_objects[Random.Range(0, fish_objects.Length)], spawn_pos, Random.rotation);
            fish.name = "DeadFish";
            spawns[i] = fish;
        }
    }
}
