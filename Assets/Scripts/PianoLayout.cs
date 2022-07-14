using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoLayout : MonoBehaviour
{
    public int Octave;
    public Keyboard[] Keyboards = new Keyboard[12];
    public static PianoLayout[] PianoLayouts = new PianoLayout[3];

    private void Start()
    {
        PianoLayouts[Octave - 3] = this;
    }
}
