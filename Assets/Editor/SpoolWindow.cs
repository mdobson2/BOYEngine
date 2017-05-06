using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SpoolWindow : EditorWindow {

    public Texture2D bckg;
    public List<StitchNode> myStitches = new List<StitchNode>();
    public Vector2 scrollPosition;

    public Spool previousSpool;
    public Spool workingSpool;

    //public Stitch stitch;
    public StitchNode workingStitch;
    public StitchEditor editor;

    //rectManagment
    float standardSpace = 10f;
    float inspectorWidth = 300f;
    float standardButtonWidth = 100f;
    float standardHeight = 18f;

    //nodeConnections
    public int nodeAttachID = -1;

    [MenuItem("Window/Spool Editor")]
	public static void GetWindow()
    {
        EditorWindow window = EditorWindow.GetWindow<SpoolWindow>();
        window.minSize = new Vector2(1200, 600);
        window.maxSize = new Vector2(1300, 700);
    }

    void OnEnable()
    {
        bckg = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Editor/Graphics/grid.jpg", typeof(Texture2D));
        editor = new StitchEditor();
        previousSpool = null;
    }

    void OnGUI()
    {
        //if the spool list has changed
        if(previousSpool != workingSpool)
        {
            if(previousSpool != null)
            {
                //if(workingSpool != null)
                //{
                //    //Debug.Log(workingSpool.name + " - " + previousSpool.name);
                //}
                //else
                //{
                //    //Debug.Log("null - " + previousSpool.name);
                //}
                SaveLayout(previousSpool);
            }
            else
            {
                Debug.Log(workingSpool.name + " - null");
            }
            //SaveLayout(previousSpool);
            PopulateList();
        }

        //Draw the background
        GUI.DrawTexture(new Rect(0, 0, this.position.width - inspectorWidth, this.position.height), bckg);

        Rect rectManager = new Rect();

        rectManager = new Rect(standardSpace, standardSpace, standardButtonWidth, standardHeight);
        //used to add a new stitch to the window
        if(GUI.Button(rectManager,"Add Stitch"))
        {
            Stitch temp = new Stitch();
            int ID = myStitches.Count;
            temp.stitchID = ID;
            int nameID = ID + 1;
            temp.stitchName = "Stitch" + nameID;
            temp.yarns = new Yarn[1];
            AssetDatabase.CreateAsset(temp, "Assets/" + workingSpool.name +"/" + temp.stitchName + ".asset");
            myStitches.Add(new StitchNode(new Rect(10, 40, 150, 150), myStitches.Count - 1, temp));
            List<Stitch> tempList = new List<Stitch>();
            foreach(StitchNode node in myStitches)
            {
                tempList.Add(node.stitch);
            }
            workingSpool.stitchCollection = tempList.ToArray();
            myStitches[myStitches.Count - 1].closeFunction += RemoveNode;
            myStitches[myStitches.Count - 1].nodeEditor = this;
        }
        //rectManager.x += rectManager.width + standardSpace;
        //if(GUI.Button(rectManager, "Test"))
        //{
        //    Debug.Log("Count number " + myStitches.Count);
        //}

        rectManager.width = 250;
        rectManager.x = position.width - inspectorWidth - standardSpace - rectManager.width;
        workingSpool = (Spool)EditorGUI.ObjectField(rectManager, workingSpool, typeof(Spool), false);
        rectManager.width = standardButtonWidth;
        rectManager.x -= standardSpace + rectManager.width;
        if(GUI.Button(rectManager, "New Spool"))
        {
            NewSpoolPopup.Init();
        }
        

        BeginWindows();
        if(myStitches.Count > 0)
        {
            for(int i = 0; i < myStitches.Count; i++)
            {
                myStitches[i].rect = GUI.Window(i,myStitches[i].rect, myStitches[i].DrawGUI, myStitches[i].stitch.stitchName);
            }
        }
        GUI.Window(1000, new Rect(position.width - inspectorWidth, 0, inspectorWidth, position.height), editor.DrawGUI, "Inspector");
        GUI.BringWindowToFront(1000);
        EndWindows();

        //for each stitch on the screen
        for (int i = 0; i < myStitches.Count; i++)
        {
            //Debug.Log(myStitches[i].stitch.name + " has " + myStitches[i].stitch.yarns.Length + " yarns");
            //for each yarn on that stitch
            if(myStitches[i].stitch.yarns.Length > 0)
            {
                for(int j = 0; j < myStitches[i].stitch.yarns.Length; j++)
                {
                    //change the color of the yarn based on the current node
                    Color locColor = Color.black;
                    switch(myStitches[i].stitch.status)
                    {
                        case Stitch.stitchStatus.regular:
                            locColor = Color.black;
                            break;
                        case Stitch.stitchStatus.auto:
                            locColor = Color.green;
                            break;
                    }
                    //Debug.Log(myStitches[i].stitch.name.ToString() + " is linked to " + myStitches[i].stitch.yarns[j].choiceStitch.name);
                    if (myStitches[i].stitch.yarns[j].choiceStitch != null)
                    {
                        //DrawNodeCurve(myStitches[i].rect, myStitches[myStitches[i].stitch.yarns[j].choiceStitch.stitchID].rect, locColor);
                        DrawNodeCurve(myStitches[i], myStitches[myStitches[i].stitch.yarns[j].choiceStitch.stitchID], j, locColor);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        SaveLayout(workingSpool);
    }

    //used to draw the curve from one stitch to the next
    void DrawNodeCurve(Rect start, Rect end, Color color)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + (start.height / 2) + 10, 0);
        Vector3 endPos = new Vector3(end.x, end.y + (end.height / 2) + 10, 0);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Color shadowCol = new Color(0, 0, 0, 0.06f);


        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 5);
    }

    //used to draw the curve from one stitch to the next
    void DrawNodeCurve(StitchNode start, StitchNode end, int yarnIndex , Color color)
    {
        //Vector3 startPos = new Vector3(start.x + start.width, start.y + (start.height / 2) + 10, 0);
        //Vector3 endPos = new Vector3(end.x, end.y + (end.height / 2) + 10, 0);
        Vector3 startPos = new Vector3(start.rect.x + start.rect.width, start.rect.y + start.GetStandardHeight() + ((yarnIndex) * start.choiceSpacing) + start.choiceSpacing / 2, 0);
        Vector3 endPos = new Vector3(end.rect.x, end.rect.y + end.GetStandardHeight() / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 100;
        Vector3 endTan = endPos + Vector3.left * 100;
        Color shadowCol = new Color(0, 0, 0, 0.06f);


        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 5);
    }

    void PopulateList()
    {
        //Debug.Log("Test");
        myStitches.Clear();
        workingStitch = null;
        if(workingSpool != null)
        {
            SpoolWindowOptions tempOpt = (SpoolWindowOptions)AssetDatabase.LoadAssetAtPath("Assets/" + workingSpool.name + "/" + workingSpool.name + "Options.asset", typeof(SpoolWindowOptions));
            if(workingSpool.stitchCollection.Length > 0)
            {
                Rect tempRect = new Rect();
                for(int i = 0; i < workingSpool.stitchCollection.Length; i++)
                {           
                    if(tempOpt.storedRects.Length > 0)
                    {
                        tempRect = tempOpt.storedRects[i];
                    }
                    else
                    {
                        tempRect = new Rect(30, 30, 150, 150);
                    }
                    myStitches.Add(new StitchNode(tempRect,i, workingSpool.stitchCollection[i]));
                }
            }
        }
        previousSpool = workingSpool;
    }

    public void PopulateInspector(StitchNode pWorkingStitch)
    {
        //Debug.Log("Receiving stitch information");
        //stitch = pWorkingStitch;
        workingStitch = pWorkingStitch;
        editor = (StitchEditor)Editor.CreateEditor(workingStitch);
        editor.PopulateInspector(workingStitch);
    }

    public void RemoveNode(int id)
    {
        for (int i = 0; i < myStitches.Count; i++)
        {
            for(int j = 0; j < myStitches[i].stitch.yarns.Length; j++)
            {
                if(myStitches[i].stitch.yarns[j].choiceStitch != null)
                {
                    if(myStitches[i].stitch.yarns[j].choiceStitch.stitchID == id)
                    {
                        myStitches[i].stitch.yarns[j].choiceStitch = null;
                    }
                }
            }
            myStitches[i].linkedNodes.RemoveAll(item => item.id == id);
        }
        myStitches[id].RemoveAsset(workingSpool.name);
        myStitches.RemoveAt(id);
        Stitch[] temp = new Stitch[myStitches.Count];
        for(int i = 0; i < temp.Length; i++)
        {
            myStitches[i].stitch.stitchID = i;
            temp[i] = myStitches[i].stitch;
        }
        workingSpool.stitchCollection = temp;
        UpdateNodeIDs();

    }

    public void UpdateNodeIDs()
    {
        Debug.Log("Updating IDs");
        for(int i = 0; i < myStitches.Count; i++)
        {
            myStitches[i].ReassignID(i);
        }
    }

    public void BeginAttachment(int winID)
    {
        nodeAttachID = winID;
    }

    public void EndAttachment(int winID)
    {
        if(nodeAttachID > -1)
        {
            myStitches[nodeAttachID].AttachComplete(myStitches[winID]);
        }
        nodeAttachID = -1;
    }

    public void SaveLayout(Spool saveSpool)
    {
        Debug.Log("Saving");
        Rect[] temp = new Rect[myStitches.Count];
        for(int i = 0; i < temp.Length; i++)
        {
            temp[i] = myStitches[i].rect;
        }
        AssetDatabase.CreateAsset(new SpoolWindowOptions(temp), "Assets/" + saveSpool.name + "/" + saveSpool.name + "Options.asset");
    }
}
