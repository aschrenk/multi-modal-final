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
    public ChangeMaterial[] buttonMaterial;
    
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
                Debug.LogWarning("one line cut away, return to prev");

                skipCode = -1;
                storedLine = -1;
                currentLine = prevLine;
                PlayLine();
                return;
            case 2:
                Debug.LogWarning("employee hangs up");

                skipCode = -1;
                storedLine = -1;
                currentLine = 17;
                PlayLine();
                return;
            case 3:
                Debug.LogWarning("s2 employee line");
                
                storedLine = -1;
                if (currentLine == 32)
                {
                    currentLine = 33;
                }
                else
                {
                    skipCode = -1;
                    currentLine = 28;
                }
                PlayLine();
                return;
            case 4:
                Debug.LogWarning("s2 alex line");

                storedLine = -1;
                if (currentLine == 34 || currentLine == 35)
                {
                    currentLine++;
                }
                else
                {
                    skipCode = -1;
                    discord++;
                    currentLine = 28;
                }
                PlayLine();
                return;
            case 5:
                Debug.LogWarning("s2 alex line");

                storedLine = -1;
                if (currentLine == 51)
                {
                    currentLine++;
                }
                else
                {
                    skipCode = -1;
                    discord++;
                    currentLine = 50;
                }
                PlayLine();
                return;
            case 6:
                Debug.LogWarning("final scene cut in");

                storedLine = -1;
                if (currentLine == 65 || currentLine == 66)
                {
                    currentLine++;
                }
                else
                {
                    skipCode = -1;
                    discord++;
                    if (prevLine >= 61)
                    {
                        currentLine = 68;
                    }
                    else
                    {
                        RouteSplit();
                    }
                }
                PlayLine();
                return;
            default: 
                break;
        }
        
        if (inturrupt == true && prevLine != -1 && skipCode == -1)
        {
            inturrupt = false;

            //inturrupting first scene
            if (sceneNumber == 1)
            {
                if (speakerID[storedLine] == 1)
                {
                    skipCode = 1;
                    currentLine = 18;
                    PlayLine();
                    return;
                }
                else if (speakerID[storedLine] == 4)
                {
                    pizza = false;
                    skipCode = 2;
                    currentLine = 19;
                    PlayLine();
                    return;
                }
            }
            else if (sceneNumber == 2)
            {
                if (speakerID[storedLine] == 2 || speakerID[storedLine] == 3)
                {
                    skipCode = 1;
                    currentLine = 31;
                    PlayLine();
                    return;
                }
                else if (speakerID[storedLine] == 4)
                {
                    skipCode = 3;
                    currentLine = 32;
                    PlayLine();
                    return;
                }
                else if (speakerID[storedLine] == 1)
                {
                    skipCode = 4;
                    currentLine = 34;
                    PlayLine();
                    return;
                }
            }
            else if (sceneNumber == 3)
            {
                if (storedLine == 26 || storedLine == 27)
                {
                    skipCode = 5;
                    currentLine = 51;
                    PlayLine();
                    return;
                }
                else
                {
                    skipCode = 1;
                    currentLine = 53;
                    PlayLine();
                    return;
                }
            }
            else if (sceneNumber == 4)
            {
                //turn audio back to behind after speaker is done
                currentAudio = behind;

                skipCode = 6;
                currentLine = 65;
                PlayLine();
                return;
            }

            return;
        }
        //food diverge - fixed
        else if (currentLine == 41 && pizza == false)
        {
            currentLine = 44;
            PlayLine(); 
            return;
        }
        else if (currentLine == 43)
        {
            currentLine = 46;
            PlayLine();
            return;
        }
        else if (currentLine == 60)
        {
            RouteSplit();
            PlayLine();
            return;
        }
        //end scene 1, setup scene 2 (3 for demo)
        else if (currentLine == 17)
        {
            sceneNumber = 2;
            currentLine = 20;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            portsConnected -= 2;
            lineConnected = false;
            changeMaterialJacks[0].SetOriginalMaterial();
            changeMaterialJacks[1].SetOriginalMaterial();

            changeMaterialSockets[0].SetOriginalMaterial();
            changeMaterialSockets[3].SetOriginalMaterial();

            Invoke("WaitingForScene", 5);
            return;
        }
        //end scene 2
        else if (currentLine == 30)
        {
            sceneNumber = 3;
            currentLine = 37;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            portsConnected--;
            lineConnected = false;
            changeMaterialJacks[0].SetOriginalMaterial();
            changeMaterialJacks[1].SetOriginalMaterial();

            changeMaterialSockets[1].SetOriginalMaterial();
            changeMaterialSockets[2].SetOriginalMaterial();

            Invoke("WaitingForScene", 5);
            return;
        }
        //end scene 3
        else if (currentLine == 50)
        {
            sceneNumber = 4;
            currentLine = 54;
            playThisScene = false;

            lineScreen.text = "Connecting...";
            speakerScreen.text = "000-0000";

            portsConnected -= 2;
            lineConnected = false;
            changeMaterialJacks[0].SetOriginalMaterial();
            changeMaterialJacks[1].SetOriginalMaterial();

            changeMaterialSockets[0].SetOriginalMaterial();
            changeMaterialSockets[2].SetOriginalMaterial();

            speaker.volume = 1;
            currentAudio = behind;
            
            Invoke("PlayLine", 5);
            return;
        }
        else
        {
            //Debug.Log("Normal advance");
            currentLine++;
            PlayLine();
        }
    }

    void RouteSplit()
    {
        if (discord >= 2)
        {
            currentLine = 68;
        }
        else
        {
            currentLine = 61;
        }
    }

    public void RecordPress()
    {
        //recordButton.GetComponent<Animation>().Play();
        
        if (lineConnected == true)
        {
            recordOverlay.SetActive(true);
            storedLine = currentLine;
            buttonMaterial[1].SetOtherMaterial();
        }       
    }

    public void PlayPress()
    {
        //playButton.GetComponent<Animation>().Play();
        StopCoroutine(LineManage());

        if (playThisScene == false && storedLine != -1 && (lineConnected == true || sceneNumber == 4))
        {
            buttonMaterial[1].SetOriginalMaterial();
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
            buttonMaterial[0].SetOtherMaterial();
        }
        else
        {
            wholeScreen.SetActive(false);
            speaker.volume = 0;
            buttonMaterial[0].SetOriginalMaterial();
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
