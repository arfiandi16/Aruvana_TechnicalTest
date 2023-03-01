using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ShareScreenshot : MonoBehaviour
{

    private int width,height;

    [SerializeField] private AudioSource screenShotSound;

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }
    private IEnumerator ssAndShare()
    {
        screenShotSound.Play();
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        ss.Apply();

        string filePath = Application.persistentDataPath +"/capture" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        File.WriteAllBytes(filePath,ss.EncodeToPNG());


        Destroy(ss); 
        new NativeShare().AddFile(filePath).SetSubject("Share ke medsos").SetText("Hasil screenshot hari ini").Share();
    }

    public void TakeScreenShot()
    {
        StartCoroutine(ssAndShare());
    }
}
