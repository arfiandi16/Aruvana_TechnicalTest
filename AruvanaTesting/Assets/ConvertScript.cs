using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConvertScript : MonoBehaviour
{

    public TextMeshProUGUI hasil;
    public TMP_Dropdown dropdownTime;
    public TMP_InputField inputJam,inputMenit,inputDetik;
    string str1;
    private string result="";
    string lastString = "";

    void print24(string str)
    {  
        int h1 = (int)str[1] - '0';
        int h2 = (int)str[0] - '0';
        int hh = (h2 * 10 + h1 % 10);

        int minute = (int)str[3] - '0';
        int second = (int)str[6] - '0';
        Debug.Log((int)str[1]-'0');
        Debug.Log(lastString + " " + str);
        if (lastString != str)
        {
            result = ""; 
            if (str[8] == 'A') //cek apakah array pada index ke 8 diawali A atau P
            {
                if(minute < 6 && second < 6)
                {
                    if (hh == 12)
                    {
                        result += "0";
                        for (int i = 2; i <= 7; i++)
                            result += str[i];
                    }
                    else if (hh < 12)
                    {
                        for (int i = 0; i <= 7; i++)
                            result += str[i];
                    }
                }
                
            }
             
            else // index ke 8 tandanya selain A
            {
                if (minute < 6 && second < 6) // apakah menit dan detik harus kurang dari 6 contoh 59, 5 berarti kurang dari 6,jika lebih maka tidak berjalan
                {
                    if (hh == 12)
                    {
                        result += "12";
                        for (int i = 2; i <= 7; i++)
                            result += str[i];
                    }
                    else if (hh < 12)
                    {
                        hh = hh + 12;
                        result += hh;
                        for (int i = 2; i <= 7; i++)
                            result += str[i];
                    }
                }
                    
            }
            hasil.text = result;
            lastString = str;
        } 
           
        
    }
    void Start()
    { 
    }

    // Arfiandi Wijatmiko
    void FixedUpdate()
    {
        //Debug.Log(dropdownTime.options[dropdownTime.value].text);
        if(inputJam.text.Length > 1 && inputMenit.text.Length > 1 && inputDetik.text.Length > 1)
        {
            str1 = inputJam.text+":"+ inputMenit.text+":"+ inputDetik.text + dropdownTime.options[dropdownTime.value].text.ToString();  
            print24(str1); 
        }
        
        
    }
}
