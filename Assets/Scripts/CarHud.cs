using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarHud : MonoBehaviour
{
    public Text current_gear_text;

    private CarController controller_script;
    
    // Start is called before the first frame update
    void Start()
    {
        controller_script = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        current_gear_text.text = controller_script.current_gear.ToString() + "/0";
    }
}
