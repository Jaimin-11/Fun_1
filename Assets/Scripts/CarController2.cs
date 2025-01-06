using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    public WheelColliders _colliders;
    public WheelMeshes _wheelMeshes;

    [SerializeField] private AnimationCurve _powerCurve;

    [SerializeField] private int _currentGear = 0;
    
    [SerializeField] private float _maxHorsePower = 1025f;
    [SerializeField] private float _maxBrakePower = 2025f;
    [SerializeField] private float _maxRPM = 15000f;
    [SerializeField] private float _differentialRatio = 4.4f;
    [SerializeField] private float _steerAngle = 30f;
    [SerializeField] private float _idealRPM = 2000f;
    [SerializeField] private float _currentRPM;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _horsePower;

    private float[] _gearRatios = new float[8] { 0f, 3.29f, 2.16f, 1.61f, 1.27f, 1.03f, 0.82f, 4.30f };

    private float gasInput;
    private float steeringInput;
    private float brakeInput;
    private Rigidbody _carRb;

    // Start is called before the first frame update
    void Start()
    {
        _carRb = GetComponent<Rigidbody>();
        _powerCurve.AddKey(0, 0);
        _powerCurve.AddKey(1000, 100);
        _powerCurve.AddKey(10000, _maxHorsePower);
        _powerCurve.AddKey(15000, 524);

        _currentRPM = _idealRPM;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ApplyTurning();
        CalculateForce();
        ApplyForce();
        ApplyTransform();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        if (Input.GetKeyDown(KeyCode.X))
        {
            GearUp();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            GearDown();
        }
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

    void ApplyTurning()
    {
        _colliders.FRWheel.steerAngle = _steerAngle * steeringInput;
        _colliders.FLWheel.steerAngle = _steerAngle * steeringInput;
    }

    void CalculateForce()
    {
        if (Mathf.Abs(gasInput) == 0f)
        {
            _currentRPM = Mathf.Lerp(_currentRPM, _idealRPM, 0.2f);
        }
        else
        {
            _currentRPM = Mathf.Lerp(_currentRPM, _maxRPM, 0.5f);
        }
        _horsePower = _powerCurve.Evaluate(_currentRPM);
        _motorTorque = (_horsePower / _currentRPM) * _gearRatios[_currentGear] * _differentialRatio * 5252;
    }

    void ApplyForce()
    {
        // Motor torque apply
        _colliders.RRWheel.motorTorque = _motorTorque * gasInput;
        _colliders.RLWheel.motorTorque = _motorTorque * gasInput;

        // Brake torque
        _colliders.FRWheel.brakeTorque = _maxBrakePower * brakeInput;
        _colliders.FLWheel.brakeTorque = _maxBrakePower * brakeInput;
        _colliders.RRWheel.brakeTorque = _maxBrakePower * brakeInput;
        _colliders.RLWheel.brakeTorque = _maxBrakePower * brakeInput;
    }

    void ApplyTransform()
    {
        updateWheelMesh(_colliders.FRWheel, _wheelMeshes.FRWheel);
        updateWheelMesh(_colliders.FLWheel, _wheelMeshes.FLWheel);
        updateWheelMesh(_colliders.RRWheel, _wheelMeshes.RRWheel);
        updateWheelMesh(_colliders.RLWheel, _wheelMeshes.RLWheel);

        void updateWheelMesh(WheelCollider coll, MeshRenderer wheelMesh)
        {
            Quaternion quat;
            Vector3 pos;
            coll.GetWorldPose(out pos, out quat);
            wheelMesh.transform.position = pos;
            wheelMesh.transform.rotation = quat;
        }
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
