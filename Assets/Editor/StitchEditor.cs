using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StitchNode))]
public class StitchEditor : Editor {

    SerializedObject workingStitch;

    Vector2 scrollView;

    public virtual void DrawGUI(int winID)
    {
        if(workingStitch != null)
        {
            workingStitch.Update();
            scrollView = EditorGUILayout.BeginScrollView(scrollView);
            {
                EditorGUILayout.BeginVertical("box");
                {
                    SerializedProperty stitchID = workingStitch.FindProperty("stitchID");
                    EditorGUILayout.PropertyField(stitchID);

                    SerializedProperty stitchName = workingStitch.FindProperty("stitchName");
                    EditorGUILayout.PropertyField(stitchName);

                    SerializedProperty summary = workingStitch.FindProperty("summary");
                    EditorGUILayout.PropertyField(summary);

                    SerializedProperty status = workingStitch.FindProperty("status");
                    EditorGUILayout.PropertyField(status);

                    SerializedProperty background = workingStitch.FindProperty("background");
                    EditorGUILayout.PropertyField(background);

                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                {
                    SerializedProperty performers = workingStitch.FindProperty("performers");
                    EditorGUILayout.PropertyField(performers, true);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                {
                    SerializedProperty dialogs = workingStitch.FindProperty("dialogs");
                    EditorGUILayout.PropertyField(dialogs, true);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical("box");
                {
                    SerializedProperty yarns = workingStitch.FindProperty("yarns");
                    EditorGUILayout.PropertyField(yarns, true);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();

            workingStitch.ApplyModifiedProperties();
        }
    }

    public void PopulateInspector(StitchNode pWorkingStitch)
    {
        //Debug.Log("Receiving stitch information");
        workingStitch = new SerializedObject(pWorkingStitch.stitch);
    }
}
