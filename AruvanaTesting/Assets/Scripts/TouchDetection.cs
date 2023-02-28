using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchDetection : MonoBehaviour, IPinchable
{
    [SerializeField] private float speed=10f;
    [SerializeField] private float speedRotation=10f;
    private string axis;
    private bool rotate;
    private Vector3 firstRotation;
    private Vector3 firstPosition;
    private float firstScale,currentScale;
    private Transform child;
    private float differenceScale;
    private Coroutine zoomCoroutine,rotateCoroutine;
    [SerializeField]private Vector3 minScale, maxScale;

    [SerializeField]private TextMeshProUGUI aaa,PInfo;
    private UnityTouch controlTouch; 
    private void Awake()
    {
        controlTouch = new UnityTouch();
    }
    void Start()
    {
        controlTouch.Touch.PrimaryFingerPosition.started += RotateStart;
        controlTouch.Touch.PrimaryFingerPosition.canceled += RotateEnd;
        controlTouch.Touch.SecondaryTouchContact.started += ZoomStart;
        controlTouch.Touch.SecondaryTouchContact.canceled +=ZoomEnd;

        child = transform.GetChild(0);
        firstRotation = child.transform.localEulerAngles;
        firstPosition = child.transform.localPosition; 
        
    }

    private void ZoomEnd(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StopCoroutine(zoomCoroutine);
    }

    private void ZoomStart(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void RotateEnd(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StopCoroutine(rotateCoroutine);
    }

    private void RotateStart(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        rotateCoroutine = StartCoroutine(Rotate());
    }

    private void OnEnable()
    {
        controlTouch.Enable();
    }
    private void OnDisable()
    {
        controlTouch.Disable();
    }

    IEnumerator ZoomDetection()
    {
        
        float distance = 0f; 
        float previousDistance = 0f;
        Vector3 targetScale = child.localScale;
        while (true)
        {
            distance = Vector2.Distance(controlTouch.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controlTouch.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            PInfo.text = "D = " + distance.ToString() + ", PD = " + previousDistance.ToString();
            if (distance != previousDistance)
            {
                if (distance > previousDistance)
                {
                    targetScale= child.localScale + Vector3.one * speed * Time.deltaTime;
                    //child.transform.localScale += Vector3.one * speed * Time.deltaTime;
                }

                else
                {
                    targetScale = child.localScale - Vector3.one * speed * Time.deltaTime;
                }
            }
            float x = Mathf.Clamp(targetScale.x, minScale.x, maxScale.x);
            float y = Mathf.Clamp(targetScale.y, minScale.y, maxScale.y);
            float z = Mathf.Clamp(targetScale.z, minScale.z, maxScale.z);
            child.localScale = new Vector3(x, y, z);    
            previousDistance = distance;
            yield return null;
        } 
        
    }
    IEnumerator Rotate()
    {
        if (controlTouch.Touch.SecondaryTouchContact.ReadValue<Boolean>()) yield break;
        Vector2 previousPosition = Vector2.zero;
        Vector3 targetRotation = child.localRotation.eulerAngles;
        
        while (true)
        {
            Vector2 currentPosition = controlTouch.Touch.PrimaryFingerPosition.ReadValue<Vector2>();
            Vector2 delta = currentPosition - previousPosition;
            if (delta.normalized.x!=0f)
            {
                targetRotation -=  delta.normalized.x * speedRotation* Vector3.up;
            }
            PInfo.text = "prev = " + previousPosition.ToString() + "current = " + targetRotation.ToString();
            child.localRotation = Quaternion.Euler(targetRotation);
            previousPosition = currentPosition; 
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
       // aaa.text = "D = " + distance.ToString() + ", PD = "+previousDistance.ToString();
        currentScale = transform.GetChild(0).localScale.x;
        differenceScale = currentScale - firstScale; 
        if (rotate)
        {
            if (axis.Equals("left"))
            {
                child.transform.Rotate(Vector3.up * speed*5 * Time.deltaTime);
            }
            else if (axis.Equals("right"))
            {
                child.transform.Rotate(Vector3.down* speed*5 * Time.deltaTime);
            }
            else if (axis.Equals("up"))
            {
                if (differenceScale <= 15f)
                {
                    child.transform.localScale += Vector3.one * speed * Time.deltaTime;
                }
                
            }
            else if (axis.Equals("down"))
            {
                if (differenceScale > 0f)
                {
                    child.transform.localScale -= Vector3.one * speed * Time.deltaTime;
                } 
            }
        }
    }

    public void RotateCube(string arah)
    {
        axis = arah;
        rotate = true;
    }

    public void StopRotate()
    {
        rotate = false;
    }

    public void OnPinchStart()
    {
        firstScale = child.localScale.x;
    }

    public void OnPinchUpdate(float scaleFactor)
    {
        throw new System.NotImplementedException();
    }
}
