using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingPoleController : MonoBehaviour
{
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

    void Update()
    {
        
    }
}
