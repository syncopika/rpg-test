using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public GameObject armRight;
    public GameObject armLeft;
    public GameObject lowerArmRight;
    public GameObject lowerArmLeft;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject footLeft;
    public GameObject footRight;
    public GameObject neck;

    public int animationNum;

    void animation1()
    {
        //armRight.transform.Rotate(new Vector3(1, 0, 0), armRight.transform.rotation.eulerAngles.x + 65 - (maxRotation * Mathf.Cos(Time.time * 3)));
        armRight.transform.Rotate(new Vector3(1, 0, 0), 0.3f * Mathf.Cos(Time.time * 7));
        footLeft.transform.Rotate(new Vector3(0, 0, 1), 0.5f * Mathf.Cos(Time.time * 5));
        leftHand.transform.Rotate(new Vector3(0, 0, 1), 0.5f * Mathf.Cos(Time.time * 3));
    }

    void animation2()
    {
        lowerArmRight.transform.Rotate(new Vector3(1, 0, 0), 0.1f * Mathf.Cos(Time.time * 10));
        leftHand.transform.Rotate(new Vector3(0, 1, 0), 0.5f * Mathf.Cos(Time.time * 5));
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        switch (animationNum)
        {
            case 1:
                animation1();
                break;
            case 2:
                animation2();
                break;
            default:
                animation1();
                break;
        }
    }
}
