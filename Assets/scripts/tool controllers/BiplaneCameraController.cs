using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiplaneCameraController : MonoBehaviour
{
    public GameObject toFollow;

    Vector3 offset;

    BiplaneController biplaneController;

    // Start is called before the first frame update
    void Start()
    {
        offset = toFollow.transform.position - transform.position;
        biplaneController = toFollow.GetComponent<BiplaneController>();
    }

    // Update is called once per frame
    void Update()
    {
        // https://forum.unity.com/threads/camera-to-stay-behind-an-aircraft.42508/
        // https://answers.unity.com/questions/811809/aircraft-following-camera.html
        Vector3 targetPos = toFollow.transform.position + new Vector3(0, 5f, 0);
        Vector3 biplaneForward = biplaneController.getForward();

        // change camera view
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position = Vector3.Lerp(transform.position, targetPos + biplaneForward * 7.2f, 5f * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos - biplaneForward * 15f, 5f * Time.deltaTime);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, toFollow.transform.rotation, 5f * Time.deltaTime);

        transform.LookAt(toFollow.transform);
    }
}
