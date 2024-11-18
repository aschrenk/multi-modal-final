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

    int sceneNumber;
    int currentLine;
    int storedLine;
    int prevLine;
    string skipCode;

    bool lineConnected;
    int portsConnected;
    bool playThisScene;

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
        skipCode = null;

        lineConnected = false;
        portsConnected = 0;
        playThisScene = false;

        currentAudio = speaker;
        speaker.volume = 0;

        inturrupt = false;
        pizza = true;
        discord = 0;

        Invoke("WaitingForScene", 5);
        
        //delete after adding switch logic
        //lineConnected = true;
        //PlayLine();
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
        
        CheckSpeaker();

        lineScreen.text = scriptLine[currentLine];

        Invoke("NextLine", lineLength[currentLine]);
    }

    void CheckSpeaker()
    {
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
    }
    
    void NextLine()
    {        
        recordOverlay.SetActive(false);
        playOverlay.SetActive(false);
        
        if (inturrupt == true && prevLine != -1)
        {
            //inturrupting first scene
            if (sceneNumber == 1)
            {
                if (speakerID[prevLine] == 1)
                {
                    skipCode = "1a";
                    currentLine = 18;
                    PlayLine();
                }
                if (skipCode == "1a")
                {
                    currentLine = prevLine;
                    inturrupt = false;
                    PlayLine();
                }

                if (speakerID[prevLine] == 4)
                {
                    skipCode = "1b";
                    currentLine = 19;
                    PlayLine();
                }
                if (skipCode == "1b")
                {
                    currentLine = 17;
                    inturrupt = false;
                    PlayLine();
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

            changeMaterialSockets[0].SetOriginalMaterial();
            changeMaterialSockets[2].SetOriginalMaterial();

            speaker.volume = 1;
            currentAudio = behind;
            
            Invoke("PlayLine", 5);
            return;
        }
        else
        {
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

    void WaitingForScene()
    {
        if (sceneNumber == 1)
        {
            //blink the lights
            for (int i = 0; i < 10; i++)
            {
                Invoke("MaterialBlink", 2);
            }

            changeMaterialSockets[0].SetOtherMaterial();
            changeMaterialSockets[3].SetOtherMaterial();
            PlayLine();
        }
        else if (sceneNumber == 2)
        {
            //blink the lights
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);

            changeMaterialSockets[1].SetOtherMaterial();
            changeMaterialSockets[2].SetOtherMaterial();
            PlayLine();
        }
        else if (sceneNumber == 3)
        {
            //blink the lights
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);
            Invoke("MaterialBlink", 2);

            changeMaterialSockets[0].SetOtherMaterial();
            changeMaterialSockets[2].SetOtherMaterial();
            PlayLine();
        }

    }

    void MaterialBlink()
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
