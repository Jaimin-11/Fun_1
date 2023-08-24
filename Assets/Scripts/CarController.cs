using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject car;
    public GameObject[] tier;
    public WheelCollider[] wheelcollider;
    public float[] gear_ratios = new float[7];



    public float max_horsepower;
    public float max_steer_angle;
    public float max_handbrake_force;
    public float max_rpm;

    private float current_speed;
    private int current_gear;
    private float current_rpm;
    private float current_torque;

    private float forward_input;
    private float side_input;
    private float handbrake_input = 0;

    // Start is called before the first frame update
    void Start()
    {
        gear_ratios[0] = 3.23f;
        gear_ratios[1] = 2.19f;
        gear_ratios[2] = 1.71f;
        gear_ratios[3] = 1.39f;
        gear_ratios[4] = 1.16f;
        gear_ratios[5] = 0.93f;
        gear_ratios[6] = 2.37f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        InputManagement();
        SteeringManagement(side_input);
        
        current_gear = GearManagement(current_rpm);

        if (current_gear == 0)
        {
            ApplyForces(forward_input, handbrake_input, gear_ratios[0]);

        }
        else
        {
            ApplyForces(forward_input, handbrake_input, gear_ratios[current_gear-1]);
        }

        Debug.Log(current_gear + " & " + current_rpm);

    }

    private void InputManagement()
    {
        forward_input = Input.GetAxis("Vertical");
        side_input = Input.GetAxis("Horizontal");
        handbrake_input = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void SteeringManagement(float inputS)
    {
        wheelcollider[0].steerAngle = max_steer_angle * inputS;
        wheelcollider[1].steerAngle = max_steer_angle * inputS;
    }

    private int GearManagement(float current_rpm)
    {
        //current_speed = (rpm/60)*3.6
        current_speed = (wheelcollider[2].rpm / 60) * 3.6f;

        if (forward_input > 0)
        {
            if (current_gear <=0)
            {
                current_gear++;
            }
            else if (current_rpm >= 8000)
            {
                current_gear++;
            }
        }
        if (handbrake_input > 0 || forward_input <= 0)
        {
            if (current_torque !> 0 && forward_input == 0)
            {
                if (current_rpm == 0)
                {
                    return 1;
                }
                //logic for reverse gear
            }
            else if (current_rpm <= 8000)
            {
                current_gear--;
            }
        }
        else
        {
            current_gear = 0;
        }

        return current_gear;
    }

    private void ApplyForces(float inputF, float inputB, float current_gear_ratio)
    {
         //current rpm count & current_torque
        current_rpm = wheelcollider[2].rpm;
        current_torque = inputF * ((max_horsepower * 7127) / max_rpm) * current_gear_ratio;

        // wheelcollider motor torque
        wheelcollider[2].motorTorque = current_torque;
        wheelcollider[3].motorTorque = current_torque;

        // wheelcollider handbrake force
        wheelcollider[0].brakeTorque = max_handbrake_force * inputB;
        wheelcollider[1].brakeTorque = max_handbrake_force * inputB;
        wheelcollider[2].brakeTorque = max_handbrake_force * inputB;
        wheelcollider[3].brakeTorque = max_handbrake_force * inputB;

    }

}
