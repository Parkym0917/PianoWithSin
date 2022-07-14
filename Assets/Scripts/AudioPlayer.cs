using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private bool running;
    private double sampleRateDelta;
    private double startDspTime;

    public static List<KeyboardPress> PianoPressInfo = new();
    
    private void Awake()
    {
        sampleRateDelta = 1.0/AudioSettings.outputSampleRate;
        running = true;
        startDspTime = AudioSettings.dspTime;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
            PianoLayout.PianoLayouts[1].Keyboards[0].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.A))
            PianoLayout.PianoLayouts[1].Keyboards[0].OnPointerUp(null);
        
        if(Input.GetKeyDown(KeyCode.S))
            PianoLayout.PianoLayouts[1].Keyboards[2].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.S))
            PianoLayout.PianoLayouts[1].Keyboards[2].OnPointerUp(null);
        
        if(Input.GetKeyDown(KeyCode.D))
            PianoLayout.PianoLayouts[1].Keyboards[4].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.D))
            PianoLayout.PianoLayouts[1].Keyboards[4].OnPointerUp(null);
        
        if(Input.GetKeyDown(KeyCode.F))
            PianoLayout.PianoLayouts[1].Keyboards[5].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.F))
            PianoLayout.PianoLayouts[1].Keyboards[5].OnPointerUp(null);
        
        if(Input.GetKeyDown(KeyCode.G))
            PianoLayout.PianoLayouts[1].Keyboards[7].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.G))
            PianoLayout.PianoLayouts[1].Keyboards[7].OnPointerUp(null);
        
        if(Input.GetKeyDown(KeyCode.H))
            PianoLayout.PianoLayouts[1].Keyboards[9].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.H))
            PianoLayout.PianoLayouts[1].Keyboards[9].OnPointerUp(null);
        if(Input.GetKeyDown(KeyCode.J))
            PianoLayout.PianoLayouts[1].Keyboards[11].OnPointerDown(null);
        if(Input.GetKeyUp(KeyCode.J))
            PianoLayout.PianoLayouts[1].Keyboards[11].OnPointerUp(null);
    }

    private float GetFrequency(int octave, int scale)
    {
        return Mathf.Pow(2, octave-1) * 55 * Mathf.Pow(2, (scale-10)/12f);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running) return;
        
        var currentTime = (AudioSettings.dspTime - startDspTime);
        
        // n++이 아닌 n+=2인 이유는 사운드를 출력하는 방식이
        // 스테레오가 아닌 모노이기 때문
        // 모노는 출력이 두개임
        for (var n = 0; n < data.Length; n += 2)
        {
            var r = 0f;
            for (var i = 0; i < PianoPressInfo.Count; i++)
            {
                if (i >= PianoPressInfo.Count) return;
                var currentPiano = PianoPressInfo[i];
                var x = (currentTime-currentPiano.PressTime) * GetFrequency(currentPiano.Keyboard.PianoLayout.Octave, currentPiano.Keyboard.Scale) * 2;
                var v = (float)(Math.Pow(Math.Sin(x * Math.PI), 3) + Math.Sin(Math.PI * (x + 0.666)));
                r += v;
            }
            data[n] = r / PianoPressInfo.Count;
            data[n + 1] = r / PianoPressInfo.Count;
            currentTime += sampleRateDelta;
        }
        
    }
}
