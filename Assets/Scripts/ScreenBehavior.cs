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
    public Texture Marina; 
    public Texture Maeva; 
    public GameObject MeMessage; 
    public GameObject annonceCanvas;
    public GameObject Choices;
    private GameObject contactCanvas;
    private GameObject convCanvas;
    private Transform activeConv = null;
    private string index = "Start";
    private bool init = false;
    Dictionary<string, string[]> arbre2Decision = new Dictionary<string, string[]>();
    Dictionary<string, string[]> phraseBot = new Dictionary<string, string[]>();
    public AudioClip notifSound;
    public AudioClip messageSound;
    private GameObject[] messages;
    private string[] users = new string[] {"Marina" , "Maeva"};
    private float actualScrollSizeMarina = 0;
    private float actualScrollSizeMaeva = 0;
    private float actualChoiceScrollSizeMarina = 0;
    private float actualChoiceScrollSizeMaeva = 0;
    private string currentConv = "Marina";



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
        arbre2Decision.Add("Tu avais pas dit que tu avais arrêté ?", new string[] { "Marina-02-03", "Marina-03-01" });
        arbre2Decision.Add("Ah ouais à ce point là, ça t'as beaucoup marqué.\n J'espère que tu trouveras quelque-chose qui te plaîra plus...", new string[] { "Marina-02-04", "Marina-03-02" });
        arbre2Decision.Add("Mais attends... du coup tu était dans l'université de ta mère ?", new string[] { "Marina-02-04", "Marina-03-03" });
        arbre2Decision.Add("N/A-Marina-03-01", new string[] { "Marina-03-01", "Maeva-01-01" });
        arbre2Decision.Add("N/A-Marina-03-02", new string[] { "Marina-03-02", "Maeva-01-01" });
        arbre2Decision.Add("N/A-Marina-03-03", new string[] { "Marina-03-03", "Maeva-01-01" });

        arbre2Decision.Add("Ca m'étonne pas..", new string[] { "Maeva-01-01", "Maeva-02-02" });
        arbre2Decision.Add("Mais non !?", new string[] { "Maeva-01-01", "Maeva-02-01" });

        arbre2Decision.Add("Oui", new string[] { "Maeva-02-01", "Maeva-03-01" });
        arbre2Decision.Add("Oui, je sors d'une relation compliquée", new string[] { "Maeva-02-01", "Maeva-03-01" });

        arbre2Decision.Add("Oui ", new string[] { "Maeva-02-02", "Maeva-03-01" });
        arbre2Decision.Add("Oui, je sors d'une relation compliquée ", new string[] { "Maeva-02-02", "Maeva-03-01" });

        arbre2Decision.Add("Skip-01", new string[] { "Maeva-03-01", "Ellipse-01" });

        arbre2Decision.Add("Un ciné ça te va ?", new string[] { "Maeva-04-01", "Maeva-05-01" });
        arbre2Decision.Add("Un bar ça te va ?", new string[] { "Maeva-04-01", "Maeva-05-01" });
        arbre2Decision.Add("Un Jardiland ça te va ?", new string[] { "Maeva-04-01", "Maeva-05-02" });

        arbre2Decision.Add("N/A-Marina-05-01", new string[] { "Maeva-05-01", "Marina-04-00" });
        arbre2Decision.Add("N/A-Marina-05-02", new string[] { "Maeva-05-02", "Marina-04-00" });

        arbre2Decision.Add("Oui  ", new string[] { "Marina-04-00", "Marina-04-01" });
        arbre2Decision.Add("Non", new string[] { "Marina-04-00", "Marina-04-02" });

        arbre2Decision.Add("Je vais supprimer les autres, il n'y a qu'avec toi que c'est sérieux ?", new string[] { "Marina-04-01", "Ellipse-02-02" });
        arbre2Decision.Add("C'est à toi que je préfère parler ne t'inquiète pas", new string[] { "Marina-04-01", "Ellipse-02-01" });
        arbre2Decision.Add("Bin non :(", new string[] { "Marina-04-01", "Ellipse-02-02" });

        arbre2Decision.Add("Skip-03", new string[] { "Marina-04-02", "Ellipse-02-01" });

        arbre2Decision.Add("Ok mais j'ai un rdv ce soir il faudra que je parte tôt", new string[] { "Marina-05-01", "Marina-06-01" });
        arbre2Decision.Add("C'est compliqué j'ai déjà un rdv ce soir", new string[] { "Marina-05-01", "Marina-06-02" });
        arbre2Decision.Add("Oui    ", new string[] { "Marina-06-01", "Finale-Marina" });
        arbre2Decision.Add("Oui     ", new string[] { "Marina-06-02", "Finale-Marina" });

        arbre2Decision.Add("Oui      ", new string[] { "Maeva-06-01", "Finale-Maeva" });


        phraseBot.Add("Start", new string[] { "Salut ça va ?\nTu fais quoi dans la vie ?" });
        phraseBot.Add("Marina-01-01", new string[] { "Ouais ouais ^^\nCa peut aller, et toi ça va ?" });
        phraseBot.Add("Marina-01-03", new string[] { "Ah oui je vois très bien où est ton école.\nMoi j'ai arrêté les cours l'année dernière." });
        phraseBot.Add("Marina-01-02", new string[] { "Haha oui mais je compte pas vivre ici toute ma vie.\nJe compte bientôt aller à Dreux" });
        phraseBot.Add("Marina-02-01", new string[] { "Et arrête si ça se trouve c'est très bien Dreux.\nAprès c'est vrai que j'ai arrêté les cours depuis 1 an." });
        phraseBot.Add("Marina-02-02", new string[] { "Donc tu es forcement quelqu'un de bien !\n", "Et tu fais quoi dans la vie ?\n", "Je faisais de la psycho jusqu'y il a un an avec ma mère" });
        phraseBot.Add("Marina-02-03", new string[] { "Des cours de psychologies à domicile, pour mieux m'intégrer avec les gens et mieux les comprendre haha :)\nJe faisais ça avec ma mère, elle est prof à l'université.\nJ'en ai un peu marre.\nMais je n'ai pas encore trouvé ce que je voulais faire de ma vie.\nEnfin bref on est pas là pour parler pour moi." });
        phraseBot.Add("Marina-02-04", new string[] { "Actuellement oui, après avec ma mère on a beaucoup déménagés.\nOn habite du côté de la place de la Rougemare.\n", "Ouais elle fait un taff qui l'oblige à se déplacer ?", "Elle fait de la recherche sur le cerveau des gens !", "Quoi ?", "Non je rigole, elle étudie les interactions sociales des personnes et les personnes atteintes\n de troubles/désordres mentaux.", "Ca a l'air super interessant pourquoi tu as décidé d'arrêter ?", "Finalement ça c'était éloigné de mes intérêts et ça continue à me déplaire. " });
        phraseBot.Add("Marina-03-01", new string[] { "C'est compliqué mais oui on a arrêtés les cours..." });
        phraseBot.Add("Marina-03-02", new string[] { "" });
        phraseBot.Add("Marina-03-03", new string[] { "Non non depuis le lycée elle est ma prof à domicile" });
        phraseBot.Add("Maeva-01-01", new string[] { "Salut, tu es mon premier match de toute ma vie :p" });
        phraseBot.Add("Maeva-02-01", new string[] { "Mais si !\nAlors comme ça tu fais du design ?", "Bah comment tu sais ?", "J'ai réconnu ton école sur une de tes photos.", "Ah oui toi aussi tu étudis là-bas ?", "J'y ai été quelques temps mais je suis à l'université maintenant.\n Sinon je suppose que tu es célibataire si tu es sur cette appli."});
        phraseBot.Add("Maeva-02-02", new string[] { "Haha connard :)\nBin oui c'est la première fois que j'installe cette appli, j'en avais pas vraiment besoin avant xD", "C'est vrai que tu ne laisses pas indifférent.", "Merci <3\nAlors comme ça tu fais du design ?", "Bah comment tu sais ?", "J'ai réconnu ton école sur une de tes photos.", "Ah oui toi aussi tu étudies là-bas ?", "J'y ai été quelques temps mais je suis à l'université maintenant.\nSinon je suppose que tu es célibataire si tu es sur cette appli." });
        phraseBot.Add("Maeva-03-01", new string[] { "Haha moi aussi ;)" });
        phraseBot.Add("Maeva-04-01", new string[] { "Salut beau gosse, j'pensais pas mal à toi dernièrement ^^\nT'es occupé en ce moment ?", "Ouais en ce moment je suis pas mal pris par mes partiels, en plus j'ai des rattrapages de maths...", "Ah pas de soucis je te proposerais bien de sortir un peu mais je ne voudrais pas te déranger", "C'est vrai que là ça ne m'arrange pas trop, on fait ça dans une semaine ?" });
        phraseBot.Add("Maeva-05-01", new string[] { "Haha super idée\n Va pour dans une semaine" });
        phraseBot.Add("Maeva-05-02", new string[] { "Haha super idée ^^\n Va pour dans une semaine" });
        phraseBot.Add("Marina-04-00", new string[] { "Dit, tu parles à d'autres filles sur l'appli ?" });
        phraseBot.Add("Marina-04-01", new string[] { "Ah... je pensais que j'étais la seule à avoir cette chance :(" });
        phraseBot.Add("Marina-04-02", new string[] { "Ah c'est bien ;)\nPas que je sois jalouse mais on dit qu'il y plein de gens bizarre et de fake sur Whats'Up" });
        phraseBot.Add("Marina-05-01", new string[] { "Il faut vraiment que je te vois ce soir, c'est important." });
        phraseBot.Add("Marina-06-01", new string[] { "D'accord. Le square Charles Verdrel, à 18h30. Tu seras-là ?" });
        phraseBot.Add("Marina-06-02", new string[] { "Stp c'est vraiment important. Viens me voir avant je n'en ai pas pour longtemps.\nJe serais au square Charles Verdrel, à 18h30. Tu seras-là ?" });
        phraseBot.Add("Maeva-06-01", new string[] { "Hello, alors ces partiels ? ;)", "J'ai dead ça", "Bon,\n je sais que tu m'avais proposé une petite sortie,\n il se trouve que j'ai mon appartement de libre ce soir et pour le reste du week-end ;)\nCa te dirait de passer ?\n;)"});
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(0); 
        GameObject.Find("Cover").SetActive(false);
        GameObject.Find("VideoCanvas").SetActive(false);
        foreach (var user in users)
        {
            addConversation(user);
        }
    }

    IEnumerator annonce(string annonce,Transform conv, int time = 5)
    {
        annonceCanvas.SetActive(true);
        var image = GameObject.Find("annonce").GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);    
        GameObject.Find("annoncetext").GetComponent<UnityEngine.UI.Text>().text = annonce;
        float timePassed = 0;
        while (timePassed < time)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        clearAllConvMsg();
        actualScrollSizeMarina = 0;
        actualScrollSizeMaeva = 0;
        actualChoiceScrollSizeMarina = 0;
        actualChoiceScrollSizeMaeva = 0;
        StartCoroutine(FadeTo(0.0f, 1.0f));
        historyController(conv);
        LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").GetComponent<RectTransform>());
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
        if (user == "Marina")
        {
            newUser.transform.Find("Button").Find("userPicture").GetComponent<RawImage>().texture  = Marina;
        }
        else
        {
            newUser.transform.Find("Button").Find("userPicture").GetComponent<RawImage>().texture  = Maeva;
        }
        newUser.name = "user_" + user;
        newUser.transform.SetParent(contactCanvas.transform,false);
        if (newUser.name == "user_"+this.currentConv)
            triggerNotifs(newUser);
        conv.name = "conv_" + user;
        conv.transform.Find("Scroll").Find("ConvName").GetComponent<UnityEngine.UI.Text>().text = user;
        conv.SetActive(false);
        conv.transform.SetParent(convCanvas.transform,false);
        
        var button = newUser.transform.Find("Button").GetComponent<Button>();
        button.onClick.AddListener(delegate { openConv(user,newUser); });
    }

    public void clearAllConvMsg()
    {
        foreach (Transform child in GameObject.Find("conv").transform)
        {
            RectTransform rect = (RectTransform) child.Find("Scroll").Find("panel");
            rect.sizeDelta = new Vector2(rect.rect.width, 815);
            foreach(Transform msg in child.Find("Scroll").Find("panel").transform)
            {
                Destroy(msg.gameObject);
            } 
        }
    }

    public void clearAllResponses(RectTransform panelButtons)
    {
        foreach (Transform child in panelButtons)
        {
            Destroy(child.gameObject);
        }
    }

    public void creationAnswerIntoConv(Transform conv)
    {
        GameObject choice;
        if (conv != null)
        {
            RectTransform panelButtons = (RectTransform)conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons");
            actualChoiceScrollSizeMarina = 0;
            actualChoiceScrollSizeMaeva = 0;
            panelButtons.sizeDelta = new Vector2(panelButtons.rect.width, 300);
            clearAllResponses(panelButtons);
            foreach (var item in arbre2Decision)
            {
                //Switch Conversation 
                if (item.Value[0] == index && item.Key.Contains("N/A"))
                {
                    this.index = item.Value[1];
                    this.currentConv = item.Value[1].Split('-')[0];
                    foreach (Transform child in convCanvas.transform)
                    {
                        if (child.name == "conv_" + this.currentConv)
                        {
                            historyController(child);
                        }
                    }
                    foreach (Transform child in GameObject.Find("contacts").transform)
                    {
                        if (child.name == "user_" + this.currentConv)
                        {
                            triggerNotifs(child.gameObject);
                        }
                    }
                }
                else if (item.Key.Contains("Skip") && item.Value[0] == index)
                {
                    this.index = item.Value[1];
                    historyController(conv);
                }
                else if (index.Contains("Ellipse"))
                {
                    if (index == "Ellipse-01")
                    {
                        this.index = "Maeva-04-01";
                        StartCoroutine(annonce("J'ai discuté pendant une semaine à Maeva et Marina.\n " +
                                               "C'est amusant de voir qu'elles n'ont pas du tout le même caractère.\n " +
                                               "Maeva est vraiment directe. Quant à Marina je la trouve vraiment drôle,\n" +
                                               " dommage, on dirait qu'elle n'a pas confiance en elle.\n" +
                                               "Elle semble beaucoup s'attacher à moi...", conv, 8));
                    }
                    else if (index == "Ellipse-02-01")
                    {
                        this.index = "Maeva-06-01";
                        this.currentConv = "Maeva";
                        foreach (Transform child in convCanvas.transform)
                        {
                            if (child.name == "conv_" + this.currentConv)
                            {
                                StartCoroutine(annonce("Une semaine encore s'écoule, Maeva commence à évoquer le fait de se rencontrer.\n Marina agit de manière de plus en plus possessive sans que je ne puisse vraiment l'expliquer.", child, 8));
                            }
                        }
                    }
                    else if (index == "Maeva-06-01")
                    {
                        foreach (Transform child in convCanvas.transform)
                        {
                            if (child.name == "conv_Marina")
                            {
                                addBotMessage(child, "Il faut vraiment que je te vois ce soir, c'est important.");
                                addBotMessage(child, "Stp c'est vraiment important. Viens me voir je n'en ai pas pour longtemps.\nJe serais au square Charles Verdrel, à 18h30.Tu seras-là ?");
                                choice = (GameObject)Instantiate(Choices);
                                Image img = choice.transform.Find("Button").GetComponent<Image>();
                                img.color = new Color(255, 0, 0, 1);
                                choice.transform.Find("Button").Find("ChoiceText").GetComponent<UnityEngine.UI.Text>().text = "Oui";
                                choice.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { loadEndingScene("love"); });
                                choice.transform.SetParent(child.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons"), false);
                                LayoutRebuilder.ForceRebuildLayoutImmediate(child.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
                            }
                        }
                    }
                    else if (index == "Marina-05-01")
                    {
                        foreach (Transform child in convCanvas.transform)
                        {
                            if (child.name == "conv_Maeva")
                            {
                                addBotMessage(child, "Hello, alors ces partiels ? ;)");
                                addMessage(child, "J'ai dead ça !");
                                addBotMessage(child, "Bon, je sais que tu m'avais proposé une petite sortie, il se trouve que j'ai mon appartement de libre ce soir et pour le reste du week-end ;)");
                                addBotMessage(child, "Ca te dirait de passer ?");
                                choice = (GameObject)Instantiate(Choices);
                                Image img = choice.transform.Find("Button").GetComponent<Image>();
                                img.color = new Color(255, 0, 0, 1);
                                choice.transform.Find("Button").Find("ChoiceText").GetComponent<UnityEngine.UI.Text>().text = "Oui";
                                choice.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { loadEndingScene("death"); });
                                choice.transform.SetParent(child.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons"), false);
                                LayoutRebuilder.ForceRebuildLayoutImmediate(child.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
                            }
                        }
                    }
                    else if(index == "Ellipse-02-02")
                    {
                        this.index = "Marina-05-01";
                        StartCoroutine(annonce("Une semaine encore s'écoule, malgré ma promesse, je n'ai pas arrêté de parler à Maeva qui commence à évoquer le fait de se rencontrer.\n Marina agit de manière de plus en plus possessive sans que je ne puisse vraiment l'expliquer.", conv, 8));
                    }
                }
                if (this.index == "Finale-Maeva")
                {
                    loadEndingScene("death");
                }
                if (this.index == "Finale-Marina")
                {
                    loadEndingScene("love");
                }
                else if (item.Value[0] == index)
                {
                    if (conv.name == "conv_" + this.currentConv)
                    {
                        choice = (GameObject)Instantiate(Choices);
                        if (item.Value[1] == "Finale-Marina" || item.Value[1] == "Finale-Maeva")
                        {
                            Image img = choice.transform.Find("Button").GetComponent<Image>();
                            img.color = new Color(255,0, 0, 1);  
                        }
                        choice.transform.Find("Button").Find("ChoiceText").GetComponent<UnityEngine.UI.Text>().text = item.Key;
                        choice.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { changeIndexAndSendMessage(conv,item.Key,item.Value[1]) ; });
                        choice.transform.SetParent(conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons"), false);
                        LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
                        RectTransform transformButtons = (RectTransform)choice.transform;
                        float actualChoiceScrollSize;
                        if (conv.name == "conv_Marina")
                        {
                            actualChoiceScrollSizeMarina += transformButtons.rect.height;
                            actualChoiceScrollSize = actualChoiceScrollSizeMarina;
                        }else
                        {
                            actualChoiceScrollSizeMaeva += transformButtons.rect.height;
                            actualChoiceScrollSize = actualChoiceScrollSizeMaeva;
                        }
                        if (actualChoiceScrollSize >= 280 / 2)
                        {
                            panelButtons.sizeDelta = new Vector2(panelButtons.rect.width, panelButtons.rect.height + transformButtons.rect.height);
                        }
                        LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
                    }
                }
            }
            conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        }

    }
    public void changeIndexAndSendMessage(Transform conv,string message, string newIndex)
    {
        this.index = newIndex;
        addMessage(conv, message);
        historyController(conv);
        RectTransform panelButtons = (RectTransform)conv.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons");
        clearAllResponses(panelButtons);
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
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.Find("Scroll").Find("Response").Find("ListChoices").Find("ScrollButton").Find("panelButtons").GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(child.Find("Scroll").GetComponent<RectTransform>());
                activeConv = child;
                if (index == "Start" && init == false && id==currentConv) {
                    historyController(child);
                    init = true;
                }
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
    
    IEnumerator waitBotMessage(Transform conv, string message) 
    {
        yield return new WaitForSeconds(1);
        addBotMessage(conv, message);
    }
    
    IEnumerator waitMessage(Transform conv, string message) 
    {
        yield return new WaitForSeconds(1);
        addMessage(conv, message);
    }
    
    IEnumerator waitResponse(Transform conv) 
    {
        yield return new WaitForSeconds(1);
        creationAnswerIntoConv(conv);
    }
    public void historyController(Transform conv)
    {
            
        foreach (var item in phraseBot)
        {
            if (item.Key == index)
            {
                for (int i = 0; i < item.Value.Length; i++)
                {
                if (i % 2 == 0)
                {
                    if (item.Value[i] != "")
                    {
                        StartCoroutine(waitBotMessage(conv, item.Value[i]));
                    }
                }
                else
                {
                    if (item.Value[i] != "")
                    {
                        StartCoroutine(waitMessage(conv, item.Value[i]));
                    }
                }
               }
                
            }

        }
        StartCoroutine(waitResponse(conv));
    }

    public void addBotMessage(Transform conv,string message)
    {
        if (conv != null)
        {
            RectTransform scroll = (RectTransform)conv.Find("Scroll").Find("panel");
            GameObject botMessage = (GameObject)Instantiate(BotMessage);
            botMessage.transform.Find("BG").Find("Message").GetComponent<UnityEngine.UI.Text>().text = message;
            botMessage.transform.SetParent(conv.Find("Scroll").Find("panel"),false); 
            RectTransform transformBotMessage = (RectTransform)botMessage.transform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").GetComponent<RectTransform>());
            float actualScrollSize;
            if (conv.name == "conv_Marina")
            {
                actualScrollSizeMarina += transformBotMessage.rect.height;
                actualScrollSize = actualScrollSizeMarina;
            }else
            {
                actualScrollSizeMaeva += transformBotMessage.rect.height;
                actualScrollSize = actualScrollSizeMaeva;
            }
            if (actualScrollSize >= 480)
            {
                scroll.sizeDelta = new Vector2(scroll.rect.width, scroll.rect.height + transformBotMessage.rect.height);
            }
            if (conv.gameObject.activeSelf == false)
            {
                triggerNotifs(GameObject.Find(conv.name.Replace("conv", "user")));
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").GetComponent<RectTransform>());
            conv.Find("Scroll").GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
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
            LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").GetComponent<RectTransform>());
            float actualScrollSize;
            if (conv.name == "conv_Marina")
            {
                actualScrollSizeMarina += transformMessage.rect.height;
                actualScrollSize = actualScrollSizeMarina;
            }else
            {
                actualScrollSizeMaeva += transformMessage.rect.height;
                actualScrollSize = actualScrollSizeMaeva;

            }
            if (actualScrollSize >= 480) {
                scroll.sizeDelta = new Vector2(scroll.rect.width, scroll.rect.height + transformMessage.rect.height);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(conv.Find("Scroll").GetComponent<RectTransform>());
            conv.Find("Scroll").GetComponent<ScrollRect>().normalizedPosition = new Vector2(1, 0);
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
    
    // need to add all scenes to the build in order to call this function
    public void loadEndingScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

}
