using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorRandom : MonoBehaviour
{
    public float transitionTimeInSec = 2f;

    private bool changingColor = false;

    private Color color1;
    private Color color2;


    void Start()
    {
        StartCoroutine(beginToChangeColor());
    }

    IEnumerator beginToChangeColor()
    {
        Camera cam = Camera.main;
        color1 = Random.ColorHSV(Random.value, Random.value);
        color2 = Random.ColorHSV(Random.value, Random.value);

        while (true)
        {
            yield return lerpColor(cam, color1, color2, transitionTimeInSec);
            color1 = cam.backgroundColor;
            color2 = Random.ColorHSV(Random.value, Random.value);
        }
    }

    IEnumerator lerpColor(Camera targetCamera, Color fromColor, Color toColor, float duration)
    {
        if (changingColor)
            yield break;
        changingColor = true;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float colorTime = counter / duration;
            targetCamera.backgroundColor = Color.Lerp(fromColor, toColor, counter / duration);
            yield return null;
        }
        changingColor = false;
    }
}
