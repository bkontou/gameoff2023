using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameMap : MonoBehaviour
{
    public Transform pc;
    public RawImage game_map_image;
    public RawImage pc_position_image;
    public RawImage blackout_image_prefab;
    public Canvas canvas_object;

    public int map_divisions = 10;
    public float map_size = 400f;
    public float map_update_polling_timer = 0.6f;

    private float blackout_image_expension_degree = 0.5f;

    private bool[,] visited_areas;
    private RawImage[,] blackout_images;
    private float _polling_timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        visited_areas = new bool[map_divisions, map_divisions];
        blackout_images = new RawImage[map_divisions, map_divisions];

        for (int i = 0; i < map_divisions; i++)
        {
            for (int j = 0; j < map_divisions; j++)
            {
                visited_areas[i,j] = false;
                
                RawImage blackout_obj = Instantiate<RawImage>(blackout_image_prefab);
                blackout_obj.transform.SetParent(game_map_image.transform);
                Vector2 blackout_obj_loc = index_to_map_position(new Vector2Int(i,j));
                blackout_obj.rectTransform.sizeDelta = new Vector2(map_size / map_divisions, map_size / map_divisions);
                blackout_obj.rectTransform.sizeDelta *= (1 + blackout_image_expension_degree);
                blackout_obj.rectTransform.anchoredPosition = blackout_obj_loc;
                //blackout_obj.rectTransform.rect.Set(blackout_obj_loc.x, blackout_obj_loc.y, map_size / map_divisions, map_size / map_divisions);
                print(blackout_obj.rectTransform.rect);

                blackout_images[i,j] = blackout_obj;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_polling_timer >= map_update_polling_timer)
        {
            update_map();
            _polling_timer = 0;
        }
        else
        {
            _polling_timer += Time.deltaTime;
        }
    }

    private void update_map()
    {
        Vector2Int pc_position_index = world_position_to_index(pc.position);
        visited_areas[pc_position_index.x, pc_position_index.y] = true;
        update_map_areas_around_pos(pc_position_index);
        update_map_image();
    }
    
    private void update_map_areas_around_pos(Vector2Int pos)
    {
        visited_areas[(int)Mathf.Clamp(pos.x + 1, 0, map_divisions - 1), pos.y] = true;
        visited_areas[(int)Mathf.Clamp(pos.x - 1, 0, map_divisions - 1), pos.y] = true;
        visited_areas[pos.x, (int)Mathf.Clamp(pos.y + 1, 0, map_divisions - 1)] = true;
        visited_areas[pos.x, (int)Mathf.Clamp(pos.y - 1, 0, map_divisions - 1)] = true;
    }

    private void update_map_image()
    {
        Vector2 pc_map_position = world_position_to_map_position(pc.position);
        pc_position_image.rectTransform.anchoredPosition = pc_map_position;

        Vector2 pc_dir = new Vector2(-pc.transform.forward.z, pc.transform.forward.x);
        float signed_angle = Vector2.SignedAngle(pc_dir, (Vector2) pc_position_image.rectTransform.up);
        pc_position_image.rectTransform.Rotate(Vector3.back, signed_angle);

        for (int i = 0; i < map_divisions; i++)
        {
            for(int j = 0; j < map_divisions; j++)
            {
                if (visited_areas[i,j]) {
                    blackout_images[i, j].color = Vector4.zero;
                }
            }
        }
    }

    private Vector2Int world_position_to_index(Vector3 position)
    {
        int array_index;

        int position_x = Mathf.FloorToInt((position.x / map_size) * map_divisions);
        int position_z = Mathf.FloorToInt((position.z / map_size) * map_divisions);

        return new Vector2Int((int) Mathf.Clamp(position_x, 0, map_divisions - 1), (int)Mathf.Clamp(position_z, 0, map_divisions - 1));

        //array_index = (map_divisions - 1) * position_x + map_divisions + position_z;

        //return (int) Mathf.Clamp(array_index, 0, map_divisions * map_divisions);
    }

    private Vector2 world_position_to_map_position(Vector3 position)
    {
        Vector2 map_position = Vector2.zero;

        float position_x_relative = (position.x / map_size);
        float position_z_relative = (position.z / map_size);

        Rect image_rect = game_map_image.rectTransform.rect;

        map_position = new Vector2(0.5f * image_rect.width - position_z_relative * image_rect.width, position_x_relative * image_rect.height - 0.5f * image_rect.height);

        return map_position;
    }

    private Vector2 index_to_map_position(Vector2Int index)
    {
        Vector2 map_position = Vector2.zero;

        float map_pos_x = ((float)index.x / map_divisions) * map_size + 0.5f * (map_size / map_divisions);
        float map_pos_z = ((float)index.y / map_divisions) * map_size + 0.5f * (map_size / map_divisions);

        map_position = world_position_to_map_position(new Vector3(map_pos_x, 0f, map_pos_z));

        return map_position;
    }
}
