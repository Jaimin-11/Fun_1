using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelColliders _colliders;
    public WheelMeshes _wheelMeshes;

    [SerializeField] private float _maxHorsePower = 1025f;
    [SerializeField] private float _steerAngle = 30f;
    [SerializeField] private float _differentialRatio = 4.4f;
    [SerializeField] private int _currentGear = 0;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _speed;
    [SerializeField] private float _currentRPM;
    [SerializeField] private float _currentTorque;

    private float[] _gearRatios = new float[8] { 0f, 3.29f, 2.16f, 1.61f, 1.27f, 1.03f, 0.82f, 4.30f }; 

    private float gasInput;
    private float steeringInput;
    private Rigidbody _carRb;

    // Start is called before the first frame update
    void Start()
    {
        _carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SimulateCar();
    }

    void SimulateCar()
    {
        CheckInput();
        ApplyForce();
        ApplyTurning();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GearUp();
            Debug.Log("gear up");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GearDown();
            Debug.Log("gear down");
        }
    }

    void CheckInput()
    {
        _speed = _carRb.velocity.magnitude;
        _currentRPM = (_colliders.RLWheel.rpm + _colliders.RRWheel.rpm ) / 2f;
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
    }

    void GearUp()
    {
        if (_currentGear < 7)
        {
            _currentGear++;
        }
    }

    void GearDown()
    {
        if (_currentGear > 0)
        {
            _currentGear--;
        }
    }



    void ApplyForce()
    {
        _currentTorque = _motorTorque * _gearRatios[_currentGear] * _differentialRatio * gasInput;
        _colliders.RRWheel.motorTorque = _currentTorque;
        _colliders.RLWheel.motorTorque = _currentTorque;
    }

    void ApplyTurning()
    {
        _colliders.FRWheel.steerAngle = _steerAngle * steeringInput;
        _colliders.FLWheel.steerAngle = _steerAngle * steeringInput;
    }

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FRWheel;
        public WheelCollider FLWheel;
        public WheelCollider RRWheel;
        public WheelCollider RLWheel;
    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FRWheel;
        public MeshRenderer FLWheel;
        public MeshRenderer RRWheel;
        public MeshRenderer RLWheel;
    }
}
