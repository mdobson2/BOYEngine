using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NewSpoolPopup : EditorWindow {

    static string spoolName;

    bool flag;

    public static void Init()
    {
        NewSpoolPopup window = ScriptableObject.CreateInstance<NewSpoolPopup>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
        spoolName = "";
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("This is the new spool creator", EditorStyles.boldLabel);

        GUILayout.Space(10);
        spoolName = EditorGUILayout.TextField("Spool Name: ", spoolName);

        if(flag)
        {
            EditorGUILayout.HelpBox("Your spool must have a valid name", MessageType.Error);
            GUILayout.Space(25);
        }
        else
        {
            GUILayout.Space(70);
        }

        EditorGUILayout.BeginHorizontal("box");
        {
            if(GUILayout.Button("Create"))
            {
                CreateNew();
            }
            if(GUILayout.Button("Cancel"))
            {
                this.Close();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void CreateNew()
    {
        spoolName.Trim();
        if(string.IsNullOrEmpty(spoolName))
        {
            flag = true;
        }
        else
        {
            Spool temp = new Spool();
            temp.name = spoolName;
            temp.stitchCollection = new Stitch[0];
            AssetDatabase.CreateFolder("Assets", spoolName);
            AssetDatabase.CreateAsset(temp, "Assets/" + spoolName + "/" + spoolName + ".asset");
            SpoolWindow[] windows = (SpoolWindow[])Resources.FindObjectsOfTypeAll(typeof(SpoolWindow));
            if (windows.Length != 0)
            {
                windows[0].workingSpool = (Spool)AssetDatabase.LoadAssetAtPath("Assets/" + spoolName + "/" + spoolName + ".asset",typeof(Spool));
            }
            this.Close();
        }
    }
}
