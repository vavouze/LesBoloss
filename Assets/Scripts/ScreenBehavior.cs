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
    public GameObject Conversation; 
    private GameObject contactCanvas;
    private GameObject convCanvas;
    
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
        convCanvas = GameObject.Find("conv");
        string uniqId = generateID();
        
        GameObject newUser = (GameObject)Instantiate(UserContact);
        GameObject conv = (GameObject)Instantiate(Conversation);
        
        newUser.transform.Find("Button").Find("UserName").GetComponent<UnityEngine.UI.Text>().text = user;
        newUser.name = "user_" + uniqId;
        newUser.transform.SetParent(contactCanvas.transform,false);

        conv.name = "conv_" + uniqId;
        conv.transform.Find("Scroll").Find("ConvName").GetComponent<UnityEngine.UI.Text>().text = user;
        conv.SetActive(false);
        conv.transform.SetParent(convCanvas.transform,false);


        
        var button = newUser.transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(delegate { openConv(uniqId); });
    }

    public void openConv(string id)
    {
        foreach(Transform child in convCanvas.transform)
        {
            if (child.name == "conv_" + id) 
            {
                child.gameObject.SetActive(true);
            }else {
                child.gameObject.SetActive(false);
            }
        }
    }
    
    public string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }

}
