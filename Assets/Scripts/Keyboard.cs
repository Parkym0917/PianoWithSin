using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int Scale;
    public PianoLayout PianoLayout;

    private KeyboardPress lastKeyboardPress;
    private RawImage KeyboardImage;
    private Color originalColor;

    private void Awake()
    {
        TryGetComponent(out KeyboardImage);
        originalColor = KeyboardImage.color;
        transform.parent.TryGetComponent(out PianoLayout);
        PianoLayout.Keyboards[Scale - 1] = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(lastKeyboardPress != null)
            AudioPlayer.PianoPressInfo.Remove(lastKeyboardPress);
        lastKeyboardPress = new KeyboardPress
        {
            Keyboard = this,
            PressTime = AudioSettings.dspTime
        };
        AudioPlayer.PianoPressInfo.Add(lastKeyboardPress);
        KeyboardImage.color = originalColor == Color.black? new Color(originalColor.r + 0.2f, originalColor.g + 0.2f, originalColor.b + 0.2f):new Color(originalColor.r - 0.2f, originalColor.g - 0.2f, originalColor.b - 0.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        AudioPlayer.PianoPressInfo.Remove(lastKeyboardPress);
        KeyboardImage.color = originalColor;
        lastKeyboardPress = null;
    }
}
