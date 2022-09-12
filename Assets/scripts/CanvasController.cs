using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    static CanvasController canvasInstance;

    private void Awake()
    {
        if (canvasInstance == null)
        {
            canvasInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (canvasInstance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
