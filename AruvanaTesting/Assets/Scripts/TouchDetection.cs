using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchDetection : MonoBehaviour
{
    [SerializeField] private float speed=10f;
    [SerializeField] private float speedRotation=10f;  
    private Transform child; 
    private Coroutine zoomCoroutine,rotateCoroutine;
    [SerializeField]private Vector3 minScale, maxScale;
    private Color startColor;
    private Renderer renderObject; 
    private UnityTouch controlTouch;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField]private ParticleSystem fireEffect;
    [SerializeField]private AudioSource fireSound;
    private string statusName = "";
    private bool buttonTouch=false ;

    private bool isBlinking = false;
    private void Awake()
    {
        controlTouch = new UnityTouch();
    }
    void Start()
    { 
        controlTouch.Touch.PrimaryTouchContact.started += RotateStart;
        controlTouch.Touch.PrimaryTouchContact.canceled += RotateEnd;
        controlTouch.Touch.SecondaryTouchContact.started += ZoomStart;
        controlTouch.Touch.SecondaryTouchContact.canceled +=ZoomEnd;

        child = transform.GetChild(0);
        renderObject = child.GetComponent<Renderer>();
        startColor = renderObject.material.color;
        skinnedMeshRenderer = child.GetComponent<SkinnedMeshRenderer>();
        fireEffect.Stop(); 
    }

    private void ZoomEnd(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StopCoroutine(zoomCoroutine); 
    }

    private void ZoomStart(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        statusName = "zoom";
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void RotateEnd(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StopCoroutine(rotateCoroutine); 
    }

    private void RotateStart(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        statusName = "rotate";
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
        while (true && statusName== "zoom" && !isBlinking && !buttonTouch)
        {
            distance = Vector2.Distance(controlTouch.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controlTouch.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
            //PInfo.text = "D = " + distance.ToString() + ", PD = " + previousDistance.ToString();
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
        
        while (true && statusName=="rotate" && !isBlinking && !buttonTouch)
        {
            Vector2 currentPosition = controlTouch.Touch.PrimaryFingerPosition.ReadValue<Vector2>();
            Vector2 delta = currentPosition - previousPosition;
            if (delta.normalized.x!=0f)
            {
                targetRotation -=  delta.normalized.x * speedRotation* Vector3.up;
            }
            //PInfo.text = "prev = " + previousPosition.ToString() + "current = " + targetRotation.ToString();
            child.localRotation = Quaternion.Euler(targetRotation);
            previousPosition = currentPosition; 
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlinking)
        {
            renderObject.material.color = Color.Lerp(startColor, Color.white, Mathf.PingPong(Time.time * speed, 1));
        }

        if (child != null)
        {
            if (skinnedMeshRenderer.enabled == true)
            {
                fireEffect.Play();
                if (!fireSound.isPlaying)
                {
                    fireSound.Play();
                } 
            }
            else
            { 
                fireEffect.Stop();
                if (fireSound.isPlaying)
                {
                    fireSound.Stop();
                }
            }
        }
    }


    public void BlinkStart()
    {
        isBlinking = true;
    }

    public void BlinkEnd()
    {
        isBlinking = false;
        renderObject.material.color = startColor;
    }

    public void ButtonTouchStart()
    {
        buttonTouch = true;
    }
    public void ButtonTouchEnd()
    {
        buttonTouch = false;
    }

}
