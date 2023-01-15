using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampController : MonoBehaviour
{
    public Light sun;
    public ParticleSystem ps;
    public Light lamplight;

    // Start is called before the first frame update
    void Start()
    {
        ps.Stop();
        lamplight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // get time. if dark, turn on light and particle system
        float timeOfDay = sun.GetComponent<SunController>().timeOfDay;
        float startOfSunset = SunController.startOfSunset;
        float startOfNighttime = SunController.startOfNighttime;

        //Debug.Log("startOfSunset: " + startOfSunset + ", startOfNighttime: " + startOfNighttime);
        if (timeOfDay <= startOfSunset && timeOfDay >= startOfNighttime)
        {
            ps.Play();
            lamplight.enabled = true;
        }
        else
        {
            // turn it off
            if (!ps.isStopped)
            {
                ps.Stop();
            }

            if (lamplight.enabled)
            {
                lamplight.enabled = false;
            }
        }

    }
}
