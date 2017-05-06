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
    }

    void OnGUI()
    {
        //if the spool list has changed
        if(previousSpool != workingSpool)
        {
            PopulateList();
        }

        //Draw the background
        GUI.DrawTexture(new Rect(0, 0, this.position.width - inspectorWidth, this.position.height), bckg);

        Rect rectManager = new Rect();

        rectManager = new Rect(10, 10, 100, 20);
        //used to add a new stitch to the window
        if(GUI.Button(rectManager,"Add Stitch"))
        {
            Stitch temp = new Stitch();
            int ID = myStitches.Count + 1;
            temp.stitchID = ID;
            temp.stitchName = "Stitch" + ID;
            temp.yarns = new Yarn[1];
            AssetDatabase.CreateAsset(temp, "Assets/" + workingSpool.name +"/" + temp.stitchName + ".asset");
            myStitches.Add(new StitchNode(new Rect(10, 40, 150, 150), myStitches.Count, temp));
            List<Stitch> tempList = new List<Stitch>();
            foreach(StitchNode node in myStitches)
            {
                tempList.Add(node.stitch);
            }
            workingSpool.stitchCollection = tempList.ToArray();
            //PopulateList();
        }

        rectManager.width = 250;
        rectManager.x = position.width - inspectorWidth - standardSpace - rectManager.width;
        workingSpool = (Spool)EditorGUI.ObjectField(rectManager, workingSpool, typeof(Spool), false);
        rectManager.width = 100;
        rectManager.x -= standardSpace + rectManager.width;
        if(GUI.Button(rectManager, "New Spool"))
        {
            NewSpoolPopup.Init();
        }
        

        BeginWindows();
        for(int i = 0; i < myStitches.Count; i++)
        {
            myStitches[i].rect = GUI.Window(i,myStitches[i].rect, myStitches[i].DrawGUI, myStitches[i].stitch.stitchName);
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
                    //TODO add in the draw curves here

                }
            }
        }
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

    void PopulateList()
    {
        myStitches.Clear();
        workingStitch = null;
        for(int i = 0; i < workingSpool.stitchCollection.Length; i++)
        {
            myStitches.Add(new StitchNode(new Rect(30,30,150,150),i, workingSpool.stitchCollection[i]));
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
}
