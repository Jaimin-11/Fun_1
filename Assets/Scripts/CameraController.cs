using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform _target;
    public Camera _camera;
    public Vector3 _offset;
    public float _camSpeed;

    private Rigidbody _targetRB;

    // Start is called before the first frame update
    void Start()
    {
        _targetRB = _target.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 _targetForward = (_targetRB.velocity + _target.transform.forward).normalized;
        transform.position = Vector3.Slerp(transform.position,
            _target.position + _target.transform.TransformVector(_offset)
            + _targetForward * (-5f),
            _camSpeed * Time.deltaTime);
        transform.LookAt(_target);
    }
}
