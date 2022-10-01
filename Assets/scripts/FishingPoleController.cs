using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoleController : MonoBehaviour
{
    public Transform floater;

    LineRenderer fishingLine;

    public void toggleFishingLine()
    {
        fishingLine.enabled = !fishingLine.enabled;
    }

    void Start()
    {
        fishingLine = transform.GetComponent<LineRenderer>();
        fishingLine.enabled = false;
    }

}
