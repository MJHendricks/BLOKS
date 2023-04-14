using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Text timeCount;

    private TimeSpan time;
    private bool timeOn;
    private float elapsed;

    public float startTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        timeCount.text = "00:00";
        timeOn = false;
    }

    public void BeginTimer()
    {
        timeOn = true;
        startTime = Time.time;
        elapsed = 0f;
        StartCoroutine(UpdateTimer());

    }

    public void EndTimer()
    {
        timeOn = false;
    }

    private IEnumerator UpdateTimer()
    {
        while (timeOn) 
        {
            elapsed += Time.deltaTime;
            time = TimeSpan.FromSeconds(elapsed);
            string timePlayingString = time.ToString("mm' : 'ss");
            timeCount.text = timePlayingString;

            yield return null;
        }
    }

    public void ResetTimer()
    {
        timeCount.text = "00:00";
        timeOn = false;
        elapsed = 0f;
        BeginTimer();
    }
}