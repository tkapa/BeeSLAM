using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Player))]
[CanEditMultipleObjects]
public class PlayerEditor : Editor {


    public override void OnInspectorGUI()
    {
        Player p = (Player)target;

        //serializedObject.Update();

        EditorGUILayout.LabelField("Input");

        //Start horizontal Line
        EditorGUILayout.BeginVertical();

        p.throwInput = EditorGUILayout.TextField("Throw Input", p.throwInput);
        p.dodgeInput = EditorGUILayout.TextField("Dodge Input", p.dodgeInput);

        //End horizontal line
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        p.beer = (GameObject)EditorGUILayout.ObjectField("Beer Obj", p.beer, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
    }
}
