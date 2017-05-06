using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBaseClass : Editor
{
    public int id;
    public Rect rect;
    public delegate void voidFunction(int id);
    public voidFunction closeFunction;
    public Stitch stitch;

    public List<NodeBaseClass> linkedNodes = new List<NodeBaseClass>();

    public string myString = "";

    public NodeBaseClass(Rect r, int ID, Stitch pStitch)
    {
        id = ID;
        rect = r;
        stitch = pStitch;
    }
    public void BaseDraw()
    {
        GUILayout.Label(stitch.summary);

        if (GUILayout.Button("Edit"))
        {
            SpoolWindow[] windows = (SpoolWindow[]) Resources.FindObjectsOfTypeAll(typeof(SpoolWindow));
            if(windows.Length != 0)
            {
                windows[0].PopulateInspector(new StitchNode(rect,id,stitch));
            }

        }

        Color temp = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;

        if (GUI.Button(new Rect(rect.width - 18, -1, 18, 18), "X"))
        {
            closeFunction(id);
        }
        GUI.backgroundColor = temp;
        GUI.DragWindow();
    }

    public virtual void DrawGUI(int winID)
    {
        GUILayout.Label("You forgot to override");
    }

    public void ReassignID(int newID)
    {
        id = newID;
    }

    public virtual void AttachComplete(NodeBaseClass winID)
    {
        linkedNodes.Add(winID);
    }
}