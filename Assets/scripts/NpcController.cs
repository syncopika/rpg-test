using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public GameObject armRight;
    public GameObject footLeft;
    public GameObject footRight;
    public float maxRotation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        armRight.transform.Rotate(new Vector3(1, 0, 0), armRight.transform.rotation.eulerAngles.x + 65 - (maxRotation * Mathf.Cos(Time.time * 3)));
        footLeft.transform.Rotate(new Vector3(0, 0, 1),  Mathf.Cos(Time.time * 5));
    }
}
