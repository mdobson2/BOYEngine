using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpoolWindow : EditorWindow {

    public Texture2D bckg;
    public List<Stitch> myStitches = new List<Stitch>();

    public Spool workingSpool;

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
    }

    void OnGUI()
    {
        //Draw the background
        GUI.DrawTexture(new Rect(0, 0, this.position.width - 300, this.position.height), bckg);

        Rect rectManager = new Rect();
        //for each stitch on the screen
        for (int i = 0; i < myStitches.Count; i++)
        {
            //for each yarn on that stitch
            for(int j = 0; j < myStitches[i].yarns.Length; j++)
            {
                //change the color of the yarn based on the current node
                Color locColor = Color.black;
                switch(myStitches[i].status)
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

        rectManager = new Rect(10, 10, 100, 20);
        //used to add a new stitch to the window
        if(GUI.Button(rectManager,"Add Stitch"))
        {

        }

        rectManager.x += 110;
        rectManager.width = 250;
        //TODO add in the ability to choose a spool here
        workingSpool = (Spool)EditorGUI.ObjectField(rectManager, workingSpool, typeof(Spool), false);

        BeginWindows();
        for(int i = 0; i < myStitches.Count; i++)
        {

        }

        EndWindows();
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
}
