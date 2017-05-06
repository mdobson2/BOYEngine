using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoolWindowOptions : ScriptableObject {

    public Rect[] storedRects;

    public SpoolWindowOptions()
    {
        storedRects = new Rect[0];
    }

    public SpoolWindowOptions(Rect[] setup)
    {
        storedRects = setup;
    }
}
