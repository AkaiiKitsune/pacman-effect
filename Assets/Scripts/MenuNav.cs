﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MenuNav : MonoBehaviour
{
    private int index = 1;
    public int maxindex;
    private InputDevice device;
    private Vector2 inputStick;
    private bool button;
    private bool trigger;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("KeepScore", 0);
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }


    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputStick);

        if (inputStick.y < -.15f)
        {
            if (index < maxindex)
            {
                index++;
                Vector3 position = GameObject.Find("Cursor").transform.position;
                position.y = GameObject.Find("menu" + index).transform.position.y;
                GameObject.Find("Cursor").transform.position = position;
            }
        }

        if (inputStick.y > .15f)
        {
            if (index > 1)
            {
                index--;
                Vector3 position = GameObject.Find("Cursor").transform.position;
                position.y = GameObject.Find("menu" + index).transform.position.y;
                GameObject.Find("Cursor").transform.position = position;
            }
        }

        device.TryGetFeatureValue(CommonUsages.primaryButton, out button);
        device.TryGetFeatureValue(CommonUsages.triggerButton, out trigger);
        if (trigger || button)
        {
            if (index == 1)
            {

                SceneManager.LoadScene("Pacman Parsed");


            }
            if (index == 2)
            {
                Debug.Log("QUIT!");
                Application.Quit();
            }
        }
    }
}



