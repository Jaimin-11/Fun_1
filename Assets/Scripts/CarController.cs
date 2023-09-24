using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject car_obj;
    public WheelCollider[] wheel_colliders;
    public GameObject[] tyres;
    public GameObject[] tyre_cover;

    //max horspower = 1025; so, at max 15000 rpm max torque will be '264 Nm' => as per max Gear ratio => input torque = '81.7 Nm'
    public float input_torque = 81.7f;
    public float max_brake_power = 160f;
    public float max_rpm = 15000f;
    public float min_rpm = 1000f;
    public float max_steer_angle = 35f;

    public int current_gear;
    public float current_rpm;
    public float current_speed;
    public float current_torque;

    private float forward_input;
    private float side_input;
    private bool is_brake_applied;

    private float[] gear_ratios = new float[8];
    private float[] speed_checkpoints = new float[7];

    Rigidbody car_rb;

    // Start is called before the first frame update
    void Start()
    {
        car_rb = car_obj.GetComponent<Rigidbody>();

        //setting gear ratios
        gear_ratios[0] = 0f;
        gear_ratios[1] = 3.23f;
        gear_ratios[2] = 2.19f;
        gear_ratios[3] = 1.71f;
        gear_ratios[4] = 1.39f;
        gear_ratios[5] = 1.16f;
        gear_ratios[6] = 0.93f;
        gear_ratios[7] = 2.37f;

        //setting current gear to '0'
        current_gear = 0;

        //setting is_brake_applied to 'false'
        is_brake_applied = false;

        //setting current_rpm to min_rpm
        current_rpm = min_rpm;
    }

    // Update is called once per frame
    void Update()
    {
        Monitoring();
        InputManagement();
        SteerManagement();
        GearManagement();
        ForceManagement();
        UpdateTierMovement();

    }

    private void InputManagement()
    {
        forward_input = Input.GetAxis("Vertical");
        side_input = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            is_brake_applied = true;
        }
        else
        {
            is_brake_applied = false;
        }
    }

    private void Monitoring()
    {
        //current_speed = ((2f * 3.14f * wc_RL.radius * wc_RL.rpm) / 60f) * 3.6f;
        current_speed = car_rb.velocity.magnitude * 3.6f;
        current_rpm = wheel_colliders[0].rpm;
    }

    private void SteerManagement()
    {
        for (int i = 0; i < 2; i++)
        {
            wheel_colliders[i].steerAngle = side_input * max_steer_angle;
        }
    }

    private void GearManagement()
    {
        if (Input.GetKeyDown(KeyCode.Z) && current_gear < 7)
        {
            current_gear++;
        }
        else if (Input.GetKeyDown(KeyCode.X) && current_gear > 0)
        {
            current_gear--;
        }
    }

    private void ForceManagement()
    {
        current_torque = input_torque * gear_ratios[current_gear];

        for (int i = 2; i < 4; i++)
        {
            wheel_colliders[i].motorTorque = forward_input * current_torque;
        }

        if (is_brake_applied)
        {
            //making motor torque to '0'
            for (int i = 2; i < 4; i++)
            {
                wheel_colliders[i].motorTorque = 0f;
            }

            //apply brake torque
            for (int i = 0; i < 4; i++)
            {
                wheel_colliders[i].brakeTorque = max_brake_power;
            }
        }
        else
        {
            //removing brake torque
            for (int i = 0; i < 4; i++)
            {
                wheel_colliders[i].brakeTorque = 0f;
            }
        }
    }

    private void UpdateTierMovement()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos;
            Quaternion rot;
            wheel_colliders[i].GetWorldPose(out pos, out rot);
            tyres[i].transform.SetPositionAndRotation(pos, rot);
        }
    }

}
