using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class PlayerEditor : Editor {

    public override void OnInspectorGUI()
    {
        Player p = (Player)target;

        EditorGUILayout.LabelField("Input");

        //Start horizontal Line
        EditorGUILayout.BeginHorizontal();

        //End horizontal line
        EditorGUILayout.EndHorizontal();
    }
}
