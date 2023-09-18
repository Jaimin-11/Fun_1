using UnityEngine;

public class CarControllerMobile : MonoBehaviour
{
    public GameObject car_obj;
    public WheelCollider wc_FL;
    public WheelCollider wc_FR;
    public WheelCollider wc_RL;
    public WheelCollider wc_RR;

    //max horspower = 1025; so, at max 15000 rpm max torque will be '264 Nm' => as per max Gear ratio => input torque = '81.7 Nm'
    public float input_torque = 81.7f;
    public float max_brake_power = 160f;
    public float max_rpm = 15000f;
    public float min_rpm = 1000f;
    public float max_steer_angle = 35f;

    private float current_rpm;
    private float current_speed;
    private float current_torque;

    private float forward_input;
    private float side_input;
    private bool is_brake_applied;
    private int current_gear;

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
        SteerManagement();
        GearManagement();
        ForceManagement();
    }

    public void ForwardForce()
    {
        forward_input = 1f;
    }

    public void NoForce()
    {
        forward_input = 0f;
    }

    public void ReverseForce()
    {
        forward_input = -1f;
    }

    public void ApplyHandbrake()
    {
        is_brake_applied = true;
    }

    public void NoHandbrake()
    {
        is_brake_applied = false;
    }


    public void GearUp()
    {
        current_gear = current_gear + 1;
    }

    public void GearDown()
    {
        current_gear = current_gear - 1;
    }

    private void Monitoring()
    {
        //current_speed = ((2f * 3.14f * wc_RL.radius * wc_RL.rpm) / 60f) * 3.6f;
        current_speed = car_rb.velocity.magnitude * 3.6f;
        current_rpm = wc_RL.rpm;
    }

    private void SteerManagement()
    {
        wc_FL.steerAngle = side_input * max_steer_angle;
        wc_FR.steerAngle = side_input * max_steer_angle;
    }

    private void GearManagement()
    {
        if (Input.GetKeyDown(KeyCode.Z) && current_gear < 7)
        {
            GearUp();
        }
        else if (Input.GetKeyDown(KeyCode.X) && current_gear > 0)
        {
            GearDown();
        }
    }


    private void ForceManagement()
    {
        current_torque = input_torque * gear_ratios[current_gear];

        wc_RL.motorTorque = forward_input * current_torque;
        wc_RR.motorTorque = forward_input * current_torque;

        if (is_brake_applied)
        {
            //making motor torque to '0'
            wc_RL.motorTorque = 0f;
            wc_RR.motorTorque = 0f;

            //apply brake torque
            wc_FL.brakeTorque = max_brake_power;
            wc_FR.brakeTorque = max_brake_power;
            wc_RL.brakeTorque = max_brake_power;
            wc_RR.brakeTorque = max_brake_power;
        }
        else
        {
            //removing brake torque
            wc_FL.brakeTorque = 0;
            wc_FR.brakeTorque = 0;
            wc_RL.brakeTorque = 0;
            wc_RR.brakeTorque = 0;
        }
    }

}
