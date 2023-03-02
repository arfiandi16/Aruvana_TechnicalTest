using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class ShareScreenshot : MonoBehaviour
{

    private int width,height;

    [SerializeField] private AudioSource screenShotSound;
    [SerializeField] private TextMeshProUGUI info;

    private void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }
    private IEnumerator ssAndShare()
    {
        screenShotSound.Play();
        yield return new WaitForEndOfFrame();
        info.enabled = true;
        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        ss.Apply();

        string filePath = "/storage/emulated/0/DCIM/SSAruvana/";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        filePath += "capture" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        info.text = "Saved to "+filePath.ToString();
        File.WriteAllBytes(filePath,ss.EncodeToPNG()); 

        Destroy(ss); 
        new NativeShare().AddFile(filePath).SetSubject("Share ke medsos").SetText("Hasil screenshot hari ini").Share();
        yield return new WaitForSeconds(2f);
        info.enabled = false;
    }

    public void TakeScreenShot()
    {
        StartCoroutine(ssAndShare());
    }
}
