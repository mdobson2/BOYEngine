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
    public SpoolWindow nodeEditor;

    public string myString = "";

    public float standardSpacing = 20;
    public float choiceSpacing = 26;

    public NodeBaseClass(Rect r, int ID, Stitch pStitch)
    {
        id = ID;
        rect = r;
        stitch = pStitch;
    }
    public void BaseDraw()
    {
        if(nodeEditor == null)
        {
            CatchReferences();
        }
        if(closeFunction == null)
        {
            CatchReferences();
        }

        rect.height = GetFullHeight();

        GUILayout.BeginVertical("box");
        {
            GUILayout.Label(stitch.summary);
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical("box");
        {
            GUILayout.Label("# of Dialogs: " + stitch.dialogs.Length);
            GUILayout.Label("# of Performers: " + stitch.performers.Length);
        }
        GUILayout.EndVertical();
        if (GUILayout.Button("Edit"))
        {
            //SpoolWindow[] windows = (SpoolWindow[]) Resources.FindObjectsOfTypeAll(typeof(SpoolWindow));
            //if(windows.Length != 0)
            //{
            //    windows[0].PopulateInspector(new StitchNode(rect,id,stitch));
            //}
            nodeEditor.PopulateInspector(new StitchNode(rect, id, stitch));
        }
        for(int i = 0; i < stitch.yarns.Length; i++)
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label(stitch.yarns[i].choiceString);
            }
            GUILayout.EndVertical();
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

    public void CatchReferences()
    {
        SpoolWindow[] windows = (SpoolWindow[])Resources.FindObjectsOfTypeAll(typeof(SpoolWindow));
        if (windows.Length != 0)
        {
            nodeEditor = windows[0];
        }
        closeFunction = nodeEditor.RemoveNode;
    }

    public void RemoveAsset(string storyName)
    {
        AssetDatabase.DeleteAsset("Assets/" + storyName + "/" + stitch.name + ".asset");
    }

    public float GetStandardHeight()
    {
        return standardSpacing * 5.5f;
    }

    public float GetFullHeight()
    {
        return GetStandardHeight() + stitch.yarns.Length * choiceSpacing;
    }
}