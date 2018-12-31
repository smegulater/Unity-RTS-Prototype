using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    float AliveTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AliveTime += Time.deltaTime;
        if(AliveTime > 3f)
        {
            Destroy(this);
        }
    }
}
