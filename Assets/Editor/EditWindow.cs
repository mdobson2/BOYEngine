using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditWindow : EditorWindow
{

    NodeBaseClass myNode;

    public static EditWindow ShowWindow()
    {
        EditWindow eW = EditorWindow.GetWindow<EditWindow>();
        return eW;
    }

    public void setNode(NodeBaseClass node)
    {
        myNode = node;
    }

    void OnGUI()
    {
        if (myNode != null)
        {
            myNode.myString = GUILayout.TextField(myNode.myString);
        }
    }
}