using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpoolWindow : EditorWindow {

    List<Stitch> myStitches = new List<Stitch>();

    [MenuItem("Window/Spool Editor")]
	public static void GetWindow()
    {
        EditorWindow.GetWindow<SpoolWindow>();
    }

    void OnGUI()
    {
        BeginWindows();

        if(GUI.Button(new Rect(10,10,100,20),"Add Stitch"))
        {

        }

        EndWindows();
    }
}
