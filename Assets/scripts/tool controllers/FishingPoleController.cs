using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoleController : MonoBehaviour
{
    public Transform floater;

    bool hasBite;
    Vector3 floaterPositionOnBite;

    Transform hookedFish = null;

    LineRenderer fishingLine;

    public void toggleFishingLine()
    {
        fishingLine.enabled = !fishingLine.enabled;
    }

    public void registerHookedFish(Transform fish)
    {
        hookedFish = fish;
    }

    public void resetFishingPole()
    {
        floater.position = floaterPositionOnBite;
        fishingLine.enabled = false;

        if (hasBite)
        {
            transform.GetComponent<RopeControllerRealisticNoSpring>().toggleHasBite();
            transform.GetComponent<RopeControllerRealisticNoSpring>().setFloaterPos(Vector3.zero);
            hasBite = false;
        }

        if (hookedFish)
        {
            hookedFish.GetComponent<FishController>().resetFish();
            hookedFish = null;
        }
    }

    public void bite()
    {
        transform.GetComponent<RopeControllerRealisticNoSpring>().toggleHasBite();
        hasBite = true;
        floaterPositionOnBite = floater.position;
    }

    void Start()
    {
        fishingLine = transform.GetComponent<LineRenderer>();
        fishingLine.enabled = false;
        hasBite = false;
    }

    private void Update()
    {
        if (hasBite)
        {
            float oscillationSpeed = 8.0f;
            transform.GetComponent<RopeControllerRealisticNoSpring>().setFloaterPos(new Vector3(floater.position.x, floaterPositionOnBite.y + 2.0f*Mathf.Cos(oscillationSpeed * Time.time), floater.position.z));
        }
    }

}
