using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adapted from: https://answers.unity.com/questions/622459/set-directionallight-rotation-for-daynight-per-tim.html
// this also looks interesting: https://gist.github.com/paulhayes/54a7aa2ee3cccad4d37bb65977eb19e2

// attach to DirectionalLight representing the sun
public class SunController : MonoBehaviour
{
    public GameObject player;

    public Color daytimeSkyColor = new Color(0.31f, 0.88f, 1f);
    public Color middaySkyColor = new Color(0.58f, 0.88f, 1f);
    public Color nighttimeSkyColor = new Color(0.04f, 0.19f, 0.27f);

    // TODO: play around with these values
    public const float daytimeRLSeconds = 10.0f * 60 / 3;
    public const float duskRLSeconds = 1.5f * 60 / 3;
    public const float nighttimeRLSeconds = 7.0f * 60 / 3;
    public const float sunsetRLSeconds = 1.5f * 60 / 3;
    public const float gameDayRLSeconds = daytimeRLSeconds + duskRLSeconds + nighttimeRLSeconds + sunsetRLSeconds;

    public const float startOfDaytime = 0;
    public const float startOfDusk = daytimeRLSeconds / gameDayRLSeconds;
    public const float startOfNighttime = startOfDusk + duskRLSeconds / gameDayRLSeconds;
    public const float startOfSunset = startOfNighttime + nighttimeRLSeconds / gameDayRLSeconds;

    private int radius = 800;
    private float timeRT = 0; // TODO: use current time instead? i.e. get curr time, extract hours and minutes, convert to seconds
    public float timeOfDay
    {
        get { return timeRT / gameDayRLSeconds;  }
        set { timeRT = value * gameDayRLSeconds; }
    }

    Color calculateSkyColor()
    {
        float time = timeOfDay;
        if (time <= 0.25f)
            return Color.Lerp(daytimeSkyColor, middaySkyColor, time / 0.25f);
        if (time <= 0.5f)
            return Color.Lerp(middaySkyColor, daytimeSkyColor, (time - 0.25f) / 0.25f);
        if (time <= startOfNighttime)
            return Color.Lerp(daytimeSkyColor, nighttimeSkyColor, (time - startOfDusk) / (startOfNighttime - startOfDusk));
        if (time <= startOfSunset)
            return nighttimeSkyColor;

        return Color.Lerp(nighttimeSkyColor, daytimeSkyColor, (time - startOfSunset) / (1.0f - startOfSunset));
    }

    void Start()
    {
    }

    void Update()
    {
        timeRT = (timeRT + Time.deltaTime) % gameDayRLSeconds;
        Camera.main.backgroundColor = calculateSkyColor();

        float sunAngle = timeOfDay * 360;
        Vector3 midpoint = player.transform.position;

        transform.position = midpoint + Quaternion.Euler(0, 0, sunAngle) * (radius * Vector3.right);
        transform.LookAt(midpoint);
    }
}
