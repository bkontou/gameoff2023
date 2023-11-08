using UnityEngine;
using UnityEditor;

// draw a red circle around the scene cube
[CustomEditor(typeof(FishAI))]
public class CubeEditor : Editor
{
    void OnSceneGUI()
    {
        FishAI cubeExample = (FishAI)target;

        Handles.color = Color.red;
        Handles.DrawWireDisc(cubeExample.transform.position, new Vector3(0, 1, 0), cubeExample.AI_IDLE_RANGE);
    }
}