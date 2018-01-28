using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGlow : MonoBehaviour {
	[SerializeField] Light light;
    [SerializeField] AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	[SerializeField] float duration = 1;
    [SerializeField] float maxIntensity = 1;

	private float startTime;
    private bool playing = false;

    void Awake(){
        if (light == null)
        {
            light = GetComponent<Light>();
        }

        if (!playing)
        {
            Stop();
        }
	}
	
	void Update () {
        if (playing)
        {
            if (startTime == 0)
            {
		        startTime = Time.time;
            }

            var time = Time.time - startTime;
            var eval = intensityCurve.Evaluate(time % duration) * maxIntensity;
            light.intensity = eval;
        }
    }

    public void Play()
    {
        playing = true;
		startTime = Time.time;
    }

    public void Stop()
    {
        playing = false;
        light.intensity = 0;
    }

    public Light Light
    {
        get { return light; }
    }

    public float Duration
    {
        get { return duration; }
    }
}
