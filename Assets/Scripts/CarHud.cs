using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarHud : MonoBehaviour
{
    public Text current_gear_text;
    public Text current_speed_text;

    private CarController controller_script;
    private CarControllerMobile controller_script_mobile;
    
    // Start is called before the first frame update
    void Start()
    {
        controller_script = GetComponent<CarController>();
        controller_script_mobile = GetComponent<CarControllerMobile>();
    }

    // Update is called once per frame
    void Update()
    {
        current_gear_text.text = controller_script.current_gear.ToString() + "/0";
        current_speed_text.text = ((int)Math.Round(controller_script.current_speed)).ToString() + "km/h";
        
        current_gear_text.text = controller_script_mobile.current_gear.ToString() + "/0";
        current_speed_text.text = ((int)Math.Round(controller_script_mobile.current_speed)).ToString() + "km/h";
    }
}
