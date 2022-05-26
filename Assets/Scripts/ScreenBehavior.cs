using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class ScreenBehavior : MonoBehaviour 
{
    // Start is called before the first frame update

    public GameObject UserContact;
    private GameObject contactCanvas;
    private int canvasUserHeigth = 295;
    private int nbConv = 0;
    
    private string[] users = new string[] {"Jessica" , "Connasse" , "Abdel"};



    void Start()
    {
        foreach (var user in users)
        {
            addConversation(user);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addConversation(string user)
    {
        contactCanvas = GameObject.Find("contacts");
        RectTransform canvasRect = contactCanvas.GetComponent<RectTransform> ();
        GameObject newUser = (GameObject)Instantiate(UserContact);
        newUser.transform.Find("panel").Find("Button").Find("UserName").GetComponent<UnityEngine.UI.Text>().text = user;
        newUser.transform.SetParent(contactCanvas.transform,false);
        newUser.transform.localPosition  = new Vector3(0, (canvasRect.rect.height / 2) -(nbConv * canvasUserHeigth / 2) - 75, 0); 
        
        var button = newUser.transform.Find("panel").Find("Button").GetComponent<Button>();
        button.onClick.AddListener(openConv);
        
        nbConv += 1;
    }

    public void openConv()
    {
        Debug.Log("aze");
    }

}
