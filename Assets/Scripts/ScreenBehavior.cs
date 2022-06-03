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
    public GameObject BotMessage; 
    public GameObject MeMessage; 
    private GameObject contactCanvas;
    private GameObject convCanvas;
    private Transform activeConv = null;
    
    public AudioClip notifSound;
    public AudioClip messageSound;
    
    private string[] users = new string[] {"Jessica" , "Connasse" , "Abdel"};



    void Start()
    {
        StartCoroutine(WaitAnimation());
    }
    
    IEnumerator WaitAnimation()
    {

        // yield return new WaitForSeconds(17); 
        yield return new WaitForSeconds(0); 
        GameObject.Find("Cover").SetActive(false);
        GameObject.Find("VideoCanvas").SetActive(false);
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
        triggerNotifs(newUser);

        conv.name = "conv_" + uniqId;
        conv.transform.Find("Scroll").Find("ConvName").GetComponent<UnityEngine.UI.Text>().text = user;
        conv.SetActive(false);
        conv.transform.SetParent(convCanvas.transform,false);
        
        


        
        var button = newUser.transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(delegate { openConv(uniqId,newUser); });
    }

    public void openConv(string id,GameObject user)
    {
        //remove notifs
        removeNotifs(user);
        foreach(Transform child in convCanvas.transform)
        {
            if (child.name == "conv_" + id) 
            {
                child.gameObject.SetActive(true);
                activeConv = child;
                addBotMessage(activeConv,"simply dummy text of the printing and typesetting industry.\r\n" +
                                         "Lorem Ipsum has been the industry's standard dummy");
                addMessage(activeConv,"There are many variations of passages of Lorem Ipsum available, \r\n" +
                                      "but the majority have suffered alteration in some form, by injected humour");
            }else {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void removeNotifs(GameObject user)
    {
        user.transform.Find("Button").Find("Notifs").gameObject.SetActive(false);
    }
    
    public void triggerNotifs(GameObject user)
    {
        user.transform.Find("Button").Find("Notifs").gameObject.SetActive(true);
        PlaySound(notifSound,0.08f);
    }

    public string generateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public void addBotMessage(Transform conv,string message)
    {
        if (conv != null)
        {
            GameObject botMessage = (GameObject)Instantiate(BotMessage);
            botMessage.transform.Find("BG").Find("Message").GetComponent<UnityEngine.UI.Text>().text = message;
            botMessage.transform.SetParent(conv.Find("Scroll").Find("panel"),false);
        }
    }
    
    public void addMessage(Transform conv,string message)
    {
        if (conv != null)
        {
            PlaySound(messageSound,0.5f);
            GameObject Message = (GameObject)Instantiate(MeMessage);
            Message.transform.Find("BG").Find("Message").GetComponent<UnityEngine.UI.Text>().text = message;
            Message.transform.SetParent(conv.Find("Scroll").Find("panel"),false);
        }
    }
    
    public void PlaySound(AudioClip soundClip, float volume = 1.0f)
    {
        GameObject.Find("screen").GetComponent<AudioSource>().PlayOneShot(soundClip, volume);
    }

}
