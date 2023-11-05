using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class editor_script : MonoBehaviour
{
    // Start is called before the first frame update

    [MenuItem("NavMesh/Build With Slope 90")]
    public static void BuildSlope90()
    {
        SerializedObject obj = new SerializedObject(UnityEditor.AI.NavMeshBuilder.navMeshSettingsObject);
        SerializedProperty prop = obj.FindProperty("m_BuildSettings.agentSlope");
        prop.floatValue = 75.0f;
        obj.ApplyModifiedProperties();
        UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
