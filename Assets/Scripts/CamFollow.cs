using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //position update
        transform.position = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, target.transform.position.z + offset.z);
        transform.rotation = target.transform.rotation;
        //quaternion target_rotation = new Quaternion(transform.rotation.x, target.transform.rotation.y, transform.rotation.z, 1);
        //Vector3 current_rotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        //Vector3 target_rotation = new Vector3(transform.rotation.x, target.transform.rotation.y, transform.rotation.z);
        //transform.rotation = Quaternion.FromToRotation(current_rotation, target_rotation);
    }
}
