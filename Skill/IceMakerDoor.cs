using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMakerDoor : MonoBehaviour
{
    private Rigidbody myRid;
    private float fMaxDistance = 30f;
    private Vector3 InitPosition;
    private LayerMask iceMakerLayer;

    void Start()
    {
        myRid = GetComponent<Rigidbody>();
        myRid.isKinematic = true;
        myRid.useGravity = true;
        InitPosition = transform.position;
        iceMakerLayer = LayerMask.GetMask("IceBlockLayer");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("IceBlock"))
        {
            myRid.isKinematic = false;
            GameObject aa = collision.collider.gameObject;
        }
    }

    private void Update()
    {
        if (transform.position.y < InitPosition.y)
        {
            myRid.isKinematic = true;
            Vector3 _tempPos = transform.position;
            _tempPos.y = InitPosition.y;
            transform.position = _tempPos;
        }
    }
}