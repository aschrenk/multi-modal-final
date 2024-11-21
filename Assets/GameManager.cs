using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int[] speakerID;
    public float[] lineLength;
    public string[] scriptLine;
    public AudioClip[] voiceLines;

    Coroutine lineCoroutine;

    int sceneNumber;
    int currentLine;
    int storedLine;
    int prevLine;
    int skipCode;

    bool lineConnected;
    int portsConnected;
    bool playThisScene;

    public GameObject infoPopup;
    public GameObject wholeScreen;
    public TMP_Text speakerScreen;
    public TMP_Text lineScreen;

    AudioSource currentAudio;
    public AudioSource speaker;
    public AudioSource behind;

    public GameObject recordOverlay;
    public GameObject playOverlay;
    public GameObject recordButton;
    public GameObject playButton;

    bool inturrupt;
    bool pizza;
    int discord;

    public ChangeMaterial[] changeMaterialSockets;
    public ChangeMaterial[] changeMaterialJacks;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneNumber = 1;
        currentLine = 0;
        storedLine = -1;
        prevLine = -1;
        skipCode = -1;

        lineConnected = false;
        portsConnected = 0;
        playThisScene = false;

        currentAudio = speaker;
        speaker.volume = 0;

        inturrupt = false;
        pizza = true;
        discord = 0;

    }

    public void BeginGame()
    {
        infoPopup.SetActive(false);
        Invoke("WaitingForScene", 2);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if (sceneNumber != 4)
        {
            ConnectionCheck();
        }
    }

    void PlayLine()
    {
        currentAudio.clip = voiceLines[currentLine];
        currentAudio.Play();

        switch (speakerID[currentLine])
        {
            case 1:
                speakerScreen.text = "555-0189";
                break;
            case 2:
                speakerScreen.text = "535-4273";
                break;
            case 3:
                speakerScreen.text = "435-1638";
                break;
            case 4:
                speakerScreen.text = "800-5882";
                break;
            default:
                break;
        }

        lineScreen.text = scriptLine[currentLine];

        if (lineCoroutine != null)
        {
            StopCoroutine(lineCoroutine);
        }
        lineCoroutine = StartCoroutine(LineManage());
    }

    private IEnumerator LineManage()
    {
        Debug.Log(inturrupt + " " + currentLine);
        yield return new WaitForSecondsRealtime(lineLength[currentLine]);
        
        StopCoroutine(LineManage());
        NextLine();
    }
    
    void NextLine()
    {        
        recordOverlay.SetActive(false);
        playOverlay.SetActive(false);
        
        switch (skipCode)
        {
            case -1:
                break;
            case 1:
                Debug.LogWarning("Skip 1");

                skipCode = -1;
                storedLine = -1;
                currentLine = prevLine;
                PlayLine();
                return;
            case 2:
                Debug.LogWarning("Skip 2");

                skipCode = -1;
                storedLine = -1;
                currentLine = 17;
                PlayLine();
                return;
            default: 
                break;
        }
        
        if (inturrupt == true && prevLine != -1)
        {
            //inturrupting first scene
            if (sceneNumber == 1)
            {
                if (speakerID[storedLine] == 1 && skipCode == -1)
                {
                    inturrupt = false;
                    skipCode = 1;
                    currentLine = 18;
                    PlayLine();
                    return;
                }
                else if (speakerID[storedLine] == 4 && skipCode == -1)
                {
                    inturrupt = false;
                    pizza = false;
                    skipCode = 2;
                    currentLine = 19;
                    PlayLine();
                    return;
                }
            }
            else if (sceneNumber == 2)
            {

            }
            else if (sceneNumber == 3)
            {

            }
            else if (sceneNumber == 4)
            {
                //turn audio back to behind after speaker is done
                currentAudio = behind;
            }

            return;
        }
        //food diverge - FIX FOR FINAL
        else if (currentLine == 24 && pizza == false)
        {
            currentLine = 27;
            PlayLine(); 
            return;
        }
        else if (currentLine == 26)
        {
            currentLine = 29;
            PlayLine();
            return;
        }
        //playtest end message
        else if (currentLine == 29)
        {
            speakerScreen.text = "206-5381";
            lineScreen.text = "This is the end of the playtest content!";
            return;
        }
        //end scene 1, setup scene 2 (3 for demo)
        else if (currentLine == 17)
        {
            sceneNumber = 3;
            currentLine = 20;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            //fix for final
            portsConnected--;
            lineConnected = false;
            changeMaterialJacks[0].SetOriginalMaterial();
            changeMaterialJacks[1].SetOriginalMaterial();

            changeMaterialSockets[0].SetOriginalMaterial();
            changeMaterialSockets[3].SetOriginalMaterial();

            Invoke("WaitingForScene", 5);
            return;
        }
        //end scene 2
        else if (currentLine == 40)
        {
            sceneNumber = 3;
            currentLine = 1000;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            changeMaterialSockets[1].SetOriginalMaterial();
            changeMaterialSockets[2].SetOriginalMaterial();

            Invoke("WaitingForScene", 5);
            return;
        }
        //end scene 3
        else if (currentLine == 50)
        {
            sceneNumber = 4;
            currentLine = 10000;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            changeMaterialSockets[0].SetOriginalMaterial();
            changeMaterialSockets[2].SetOriginalMaterial();

            speaker.volume = 1;
            currentAudio = behind;
            
            Invoke("PlayLine", 5);
            return;
        }
        else
        {
            Debug.Log("Normal advance");
            currentLine++;
            PlayLine();
        }
    }

    public void RecordPress()
    {
        //recordButton.GetComponent<Animation>().Play();
        
        if (lineConnected == true)
        {
            recordOverlay.SetActive(true);
            storedLine = currentLine;
        }       
    }

    public void PlayPress()
    {
        //playButton.GetComponent<Animation>().Play();
        StopCoroutine(LineManage());

        if (playThisScene == false && storedLine != -1 && (lineConnected == true || sceneNumber == 4))
        {
            playOverlay.SetActive(true);
            inturrupt = true;
            playThisScene = true;

            prevLine = currentLine;
            currentLine = storedLine;

            if (sceneNumber == 4)
            {
                currentAudio = speaker;
            }
            PlayLine();
        }
    }

    private IEnumerator BlinkLights()
    {
        for (int i = 0; i < 11; i++)
        {
            switch (sceneNumber)
            {
                case 1:
                    changeMaterialSockets[0].ToggleMaterial();
                    changeMaterialSockets[3].ToggleMaterial();
                    break;
                case 2:
                    changeMaterialSockets[1].ToggleMaterial();
                    changeMaterialSockets[2].ToggleMaterial();
                    break;
                case 3:
                    changeMaterialSockets[0].ToggleMaterial();
                    changeMaterialSockets[2].ToggleMaterial();
                    break;
                default:
                    break;
            }
            yield return new WaitForSecondsRealtime(1);
        }

        CallStopLights();
    }

    void CallStopLights()
    {
        StopCoroutine(BlinkLights());

        PlayLine();
    }

    void WaitingForScene()
    {
        StartCoroutine(BlinkLights());
    }

    void ConnectionCheck()
    {        
        if (lineConnected == true)
        {
            wholeScreen.SetActive(true);
            speaker.volume = 1;
        }
        else
        {
            wholeScreen.SetActive(false);
            speaker.volume = 0;
        }
    }

    public void EnterPort(int port)
    {
        if (sceneNumber == 1)
        {
            if (port == 0 || port == 3)
            {
                portsConnected++;
            }
        }
        else if (sceneNumber == 2)
        {
            if (port == 1 || port == 2)
            {
                portsConnected++;
            }
        }
        else if (sceneNumber == 3)
        {
            if (port == 0 || port == 2)
            {
                portsConnected++;
            }
        }

        if (portsConnected >= 2)
        {
            lineConnected = true;
            changeMaterialJacks[0].SetOtherMaterial();
            changeMaterialJacks[1].SetOtherMaterial();
        }
    }

    public void ExitPort(int port)
    {
        if (sceneNumber == 1)
        {
            if (port == 0 || port == 3)
            {
                portsConnected--;
            }
        }
        else if (sceneNumber == 2)
        {
            if (port == 1 || port == 2)
            {
                portsConnected--;
            }
        }
        else if (sceneNumber == 3)
        {
            if (port == 0 || port == 2)
            {
                portsConnected--;
            }
        }

        if (portsConnected < 2)
        {
            lineConnected = false;
            changeMaterialJacks[0].SetOriginalMaterial();
            changeMaterialJacks[1].SetOriginalMaterial();
        }
    }
}
