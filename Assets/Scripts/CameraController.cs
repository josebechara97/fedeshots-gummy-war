using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offSetToTarget;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("Player");
        this.offSetToTarget = this.transform.position - this.target.transform.position;
           

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            this.transform.position = this.target.transform.position + this.offSetToTarget;
        }
    }
}
