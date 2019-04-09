using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleanorCamera : MonoBehaviour
{
    public GameObject ObjToFollow;

    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (ObjToFollow != null)
            _offset = transform.position - ObjToFollow.transform.position;
    }

    
    // Update is called once per frame
    void Update()
    {
        if (ObjToFollow == null) return;
        transform.LookAt(ObjToFollow.transform.position);
            transform.position = Vector3.Lerp(transform.position, ObjToFollow.transform.position + _offset, 0.1f);
    }
}
