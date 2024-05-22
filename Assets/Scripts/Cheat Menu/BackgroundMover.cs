using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RawImage img;
    [SerializeField] private float x, y, moveSpeed, rotateSpeed, scaleDamper;

    void Update()
    {
        img.uvRect = new Rect(new Vector2(x/100*Mathf.Sin(Time.time*moveSpeed/10), y/100*Mathf.Sin(Time.time*moveSpeed/10)), img.uvRect.size);
        rectTransform.Rotate(new Vector3(0, 0, rotateSpeed/100));

        // 0.375 and 1.875 are to keep it in the smaller range [1.5, 2.25]
        float scaleStep = 0.1f * Mathf.Cos(Time.time / scaleDamper) + 1.875f;
        rectTransform.localScale = new Vector3(scaleStep, scaleStep, 1);
    }
}
