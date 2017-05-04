using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StitchNode : NodeBaseClass {

    //public Stitch myStitch;

    public StitchNode(Rect r, int ID, Stitch stitch): base(r,ID, stitch)
    {
        //myStitch = stitch;
    }

    public override void DrawGUI(int winID)
    {
        BaseDraw();
    }

    public Performer[] getPerformers()
    {
        return stitch.performers;
    }

    public void setPerformers(Performer[] performers)
    {
        stitch.performers = performers;
    }
}
