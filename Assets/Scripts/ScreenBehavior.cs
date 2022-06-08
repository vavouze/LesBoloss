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
    public GameObject Choices;
    private GameObject contactCanvas;
    private GameObject convCanvas;
    private Transform activeConv = null;
    private string index = "Start";
    private Boolean isLastMessageBot = true;
    Dictionary<string, string[]> arbre2Decision = new Dictionary<string, string[]>();
   

    Dictionary<string, string[]> phraseBot = new Dictionary<string, string[]>();
    

    public AudioClip notifSound;
    public AudioClip messageSound;

    private float actualScrollSize = 0;
    private string[] users = new string[] {"Marina" , "Maeva"};



    void Start()
    {
        StartCoroutine(WaitAnimation());
        arbre2Decision.Add("Oui ça va ^^ et toi ça va ?", new string[] { "Start", "Marina-01-01" });
        arbre2Decision.Add("Salut, pas grand chose toi aussi tu habites sur Rouen ?", new string[] { "Start", "Marina-01-02" });
        arbre2Decision.Add("Oui, je suis en école de design et toi tu fais quoi ?", new string[] { "Start", "Marina-01-03" });

        arbre2Decision.Add("Ca va, toi aussi tu habites sur Rouen ?", new string[] { "Marina-01-01", "Marina-01-02" });

        arbre2Decision.Add("Ouah mais tu serais pas quelqu'un qui as de l'ambition toi ? xD", new string[] { "Marina-01-02", "Marina-02-01" });
        arbre2Decision.Add("J'ai la ref xD", new string[] { "Marina-01-02", "Marina-02-02" });

        arbre2Decision.Add("Tu habites dans le coin ?", new string[] { "Marina-01-03", "Marina-02-04" });
        arbre2Decision.Add("Ah ouais ? Tu faisais quoi ?", new string[] { "Marina-01-03", "Marina-02-03" });

        arbre2Decision.Add("Ah ouais ? Tu faisais quoi ? ", new string[] { "Marina-02-01", "Marina-02-03" });

        arbre2Decision.Add("N/A-Marina-02-02", new string[] { "Marina-02-02", "Maeva-01-01" });

        arbre2Decision.Add("Tu avais pas dit que tu avais arrété ?", new string[] { "Marina-02-03", "Maeva-01-01" });
        arbre2Decision.Add("N/A-Marina-02-03", new string[] { "Marina-02-03", "Marina-03-01" });

        arbre2Decision.Add("Ah ouais à ce point là, ça t'as beaucoup marqué. J'espère que tu trouveras quelque-chose qui te plaîra plus...", new string[] { "Marina-02-04", "Marina-03-02" });
        arbre2Decision.Add("Mais attends... du coup tu était dans l'université de ta mère ?", new string[] { "Marina-02-04", "Marina-03-03" });

        arbre2Decision.Add("N/A-Marina-03-01", new string[] { "Marina-03-01", "Maeva-01-01" });
        arbre2Decision.Add("N/A-Marina-03-02", new string[] { "Marina-03-02", "Maeva-01-01" });
        arbre2Decision.Add("N/A-Marina-03-03", new string[] { "Marina-03-03", "Maeva-01-01" });
        phraseBot.Add("Start", new string[] { "Salut ça va ?\nTu fais quoi dans la vie ?" });
        phraseBot.Add("Marina-01-01", new string[] { "Ouais ouais ^^\nCa peut aller, et toi ça va ?" });
        phraseBot.Add("Marina-01-03", new string[] { "Ah oui je vois très bien où est ton école.\nMoi j'ai arrété les cours l'année dernière." });
        phraseBot.Add("Marina-01-02", new string[] { "Haha oui mais je compte pas vivre ici toute ma vie.\nJe compte bientôt aller à Dreux" });
        phraseBot.Add("Marina-02-01", new string[] { "Et arrête si ça se trouve c'est très bien Dreux.\nAprès c'est vrai que j'ai arrété les cours depuis 1 an." });
        phraseBot.Add("Marina-02-02", new string[] { "Donc tu es forcement quelqu'un de bien !\n" });//, "Et tu fais quoi dans la vie ?\n", "Je faisais de la psycho jusqu'y il a un an avec ma mère" });
        phraseBot.Add("Marina-02-03", new string[] { "Des cours de psychologies à domicile, pour mieux m'intégrer avec les gens et mieux les comprendre haha :)\nJe faisais ça avec ma mère, elle est prof à l'université.\nJ'en ai un peu marre.\nMais je n'ai pas encore trouvé ce que je voulais faire de ma vie.\nEnfin bref on est pas là pour parler pour moi." });
        phraseBot.Add("Marina-02-04", new string[] { "Actuellement oui, après avec ma mère on a beaucoup déménagés.\nOn habite du côté de la place de la Rougemare.\n", "Ouais elle fait un taff qui l'oblige à se déplacer ?", "Elle fait de la recherche sur le cerveau des gens !", "Quoi ?", "Non je rigole, elle étudie les interactions sociales des personnes et les personnes atteintes de troubles/désordres mentaux.", "Ca a l'air super interessant pourquoi tu as décidé d'arréter ?" });
        phraseBot.Add("Marina-03-01", new string[] { "C'est compliqué mais oui on a arrêtés les cours..." });
        phraseBot.Add("Marina-03-02", new string[] { "" });
        phraseBot.Add("Marina-03-03", new string[] { "Non non depuis le lycée elle est ma prof à domicile" });
        phraseBot.Add("Maeva-01-01", new string[] { "Salut, tu es mon premier match de toute ma vie :p" });
    }
    
    IEnumerator WaitAnimation()
    {
        //yield return new WaitForSeconds(17); 
        yield return new WaitForSeconds(1);
        GameObject.Find("Cover").SetActive(false);
        GameObject.Find("VideoCanvas").SetActive(false);
        foreach (var user in users)
        {
            addConversation(user);
        }
    }

    IEnumerator annonce(string annonce)
    {
        GameObject.Find("annonce").SetActive(true);
        GameObject.Find("annoncetext").GetComponent<UnityEngine.UI.Text>().text = annonce;
        float timePassed = 0;
        while (timePassed < 5)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(FadeTo(0.0f, 1.0f));
    }
    
    IEnumerator FadeTo(float aValue, float aTime) 
    {
        var image = GameObject.Find("annonce").GetComponent<Image>();
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (aTime * Time.deltaTime));
            yield return null;
        }
        GameObject.Find("annonce").SetActive(false);
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

    public void creationAnswerIntoConv(Transform conv)
    {
        GameObject choice;
        if (conv != null)
        {
            RectTransform panelButtons = (RectTransform)conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons");
            foreach (Transform child in panelButtons)
            {
                Destroy(child.gameObject);
            }
            foreach (var item in arbre2Decision)
            {
                if (item.Value[0] == index)
                {
                    choice = (GameObject)Instantiate(Choices);
                    RectTransform transformButtons = (RectTransform)choice.transform;
                    choice.transform.Find("Button").Find("ChoiceText").GetComponent<UnityEngine.UI.Text>().text = item.Key;
                    choice.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { changeIndexAndSendMessage(conv,item.Key,item.Value[1]) ; });
                    panelButtons.sizeDelta = new Vector2(panelButtons.rect.width, panelButtons.rect.height + transformButtons.rect.height * 2);
                    choice.transform.SetParent(conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons"), false);
                }
            }
        }

    }
    public void changeIndexAndSendMessage(Transform conv,string message, string newIndex)
    {
     
        this.index = newIndex;
        addMessage(conv, message);
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
                if (index == "Start") {
                    addBotMessage(activeConv, "simply dummy text of the printing and typesetting industry.\r\n" +
                                             "Lorem Ipsum has been the industry's standard dummy");
                }
                /*
                foreach (var item in this.arbre2Decision)
                {
                    if (item.Value[0] == index)
                    {

                    }
                    addBotMessage(activeConv, "simply dummy text of the printing and typesetting industry.\r\n" +
                                             "Lorem Ipsum has been the industry's standard dummy");
                }*
                */
                /* addMessage(activeConv,"There are many variations of passages of Lorem Ipsum available, \r\n" +
                                       "but the majority have suffered alteration in some form, by injected humour"); */


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
    public void historyController(Transform conv)
    {
        if (isLastMessageBot)
        {
            creationAnswerIntoConv(conv);
            isLastMessageBot = false;
        }
        else
        {
            isLastMessageBot = true;
            foreach (var item in phraseBot)
            {
                if (item.Key == index)
                {
                    for (int i = 0; i < item.Value.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (item.Value[i] != "")
                                addBotMessage(conv, item.Value[i]);
                        }
                        else
                        {
                            if (item.Value[i] != "")
                                addMessage(conv, item.Value[i]);
                        }
                    }
                    
                }

            }
        }
    }

    public void addBotMessage(Transform conv,string message)
    {
        if (conv != null)
        {
            RectTransform scroll = (RectTransform)conv.Find("Scroll").Find("panel");
            GameObject botMessage = (GameObject)Instantiate(BotMessage);
            
            botMessage.transform.GetComponent<RectTransform>();
            botMessage.transform.Find("Button").Find("BG").Find("Message").GetComponent<UnityEngine.UI.Text>().text = message;
            botMessage.transform.SetParent(conv.Find("Scroll").Find("panel"), false);
            RectTransform transformBotMessage = (RectTransform)botMessage.transform;
            actualScrollSize += transformBotMessage.rect.height * 2;
            if (actualScrollSize >= 500)
            {
                actualScrollSize = 0;
                scroll.sizeDelta = new Vector2(scroll.rect.width, scroll.rect.height + transformBotMessage.rect.height * 2);
            }
            historyController(conv);
        }
    }
   

    public void addMessage(Transform conv,string message)
    {
        if (conv != null)
        {
            PlaySound(messageSound,0.5f);
            GameObject Message = (GameObject)Instantiate(MeMessage);
            RectTransform scroll = (RectTransform)conv.Find("Scroll").Find("panel");
            RectTransform transformMessage = (RectTransform)Message.transform;
            Message.transform.Find("BG").Find("Message").GetComponent<UnityEngine.UI.Text>().text = message;
            Message.transform.SetParent(conv.Find("Scroll").Find("panel"),false);
            actualScrollSize += transformMessage.rect.height * 2;
            if (actualScrollSize >= 500) {
                actualScrollSize = 0;
                scroll.sizeDelta = new Vector2(scroll.rect.width, scroll.rect.height + transformMessage.rect.height * 2);
            }
            historyController(conv);
        }
    }
    
    public void respondToMessage(Transform message)
    {
        
        GameObject reply = message.Find("reply").gameObject;
        if (reply.activeSelf)
        {
            reply.SetActive(false); 
        }
        else
        {
            if (GameObject.Find("reply") != null)
            {
                GameObject.Find("reply").SetActive(false);
            }
            reply.SetActive(true);
        }
    }
    
    public void PlaySound(AudioClip soundClip, float volume = 1.0f)
    {
        GameObject.Find("screen").GetComponent<AudioSource>().PlayOneShot(soundClip, volume);
    }

}
