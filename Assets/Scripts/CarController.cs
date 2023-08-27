using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject car_obj;
    public WheelCollider[] wheelcolliders = new WheelCollider[4];


    public float max_rpm = 15000f;
    public float max_horsepower = 1025f;
    public float max_brake_force = 1500f;
    public float max_steer_angle = 35f;

    private float[] gear_ratios = new float[8];
    private float[] speed_checkpoints = new float[7];

    public float current_rpm;
    private float forward_input;
    private float steer_input;
    private float brake_input;
    private float current_torque;

    Rigidbody car_rb;

    // Start is called before the first frame update
    void Start()
    {
        car_rb = car_obj.GetComponent<Rigidbody>();

        //setting gear ratios
        //gear_ratios[0] = 0f;
        //gear_ratios[1] = 3.23f;
        //gear_ratios[2] = 2.19f;
        //gear_ratios[3] = 1.71f;
        //gear_ratios[4] = 1.39f;
        //gear_ratios[5] = 1.16f;
        //gear_ratios[6] = 0.93f;
        //gear_ratios[7] = 2.37f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        InputManagement();
        SteeringManagement();
        ForceManagement();
    }

    void InputManagement()
    {
        forward_input = Input.GetAxis("Vertical");
        steer_input = Input.GetAxis("Horizontal");
        brake_input = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    void SteeringManagement()
    {
        wheelcolliders[0].steerAngle = steer_input * max_steer_angle;
        wheelcolliders[1].steerAngle = steer_input * max_steer_angle;
    }

    void ForceManagement()
    {
        // set logic for default 1000f rpm if no input given and if given get current

        current_rpm = wheelcolliders[2].rpm;

        //if current_rpm is greater than 15000 then no torque force applied
        if (current_rpm > 15000f)
        {
            Debug.Log("max reached");
        }
        //current_torque = current_rpm > 15000f ? 0 : max_horsepower * 7127f / current_rpm;
        current_torque = current_rpm > 15000f ? 0 : max_horsepower * 7127f ;

        for (int i = 0; i < 4; i++)
        {
            wheelcolliders[i].brakeTorque = 0;
        }

        wheelcolliders[2].motorTorque = forward_input * current_torque * Time.deltaTime;
        wheelcolliders[3].motorTorque = forward_input * current_torque * Time.deltaTime;


        //Brake management
        if (brake_input != 0)
        {
            wheelcolliders[2].motorTorque = 0;
            wheelcolliders[3].motorTorque = 0;

            for (int i = 0; i < 4; i++)
            {
                wheelcolliders[i].brakeTorque = max_brake_force * 7127f * Time.deltaTime;
            }
        }
    }

}
