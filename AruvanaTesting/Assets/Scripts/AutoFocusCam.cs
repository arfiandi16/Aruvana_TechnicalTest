using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Vuforia;

public class AutoFocusCam : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaStarted += OnSingleTapped;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnSingleTapped()
    {
        TriggerAutoFocus();
    }

    public void TriggerAutoFocus()
    {
        StartCoroutine(TriggerAutoFocusIfSet());
    }
    private IEnumerator TriggerAutoFocusIfSet()
    {
        yield return new WaitForSeconds(0.5f);
        VuforiaBehaviour.Instance.CameraDevice.SetFocusMode(FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
}
