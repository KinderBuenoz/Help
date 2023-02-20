using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleDialogueScript : MonoBehaviour
{
    public GameObject dialoguePanel;
    public GameObject Z;
    public GameObject gameObject_this;
    public GameObject puzzle;
    public GameObject player;

    public Image display;
    public Sprite newImage;
    public TMP_Text dialogueText;

    public string[] dialogue;
    private int index;
    [SerializeField] private bool hasCompletedLine = false;

    public float wordSpeed;
    public bool playerIsClose;
    public bool start = true;

    private AudioSource audioSource;
    [SerializeField] private AudioClip dialogueTypingSoundClip;
    [SerializeField] private bool stopAudioSource;

    private void Awake()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

            if (Input.GetKeyDown(KeyCode.Mouse0) && playerIsClose && start == true)
            {

                Debug.Log("Interact");
                if(dialogue.Length == 0)
            {
                StartCoroutine(SwitchPuzzleImmediate());
            }

                else if (dialoguePanel.activeInHierarchy)
                {
                    zeroText();
                }
            
                else
                {
                    start = false;
                    dialoguePanel.SetActive(true);
                    index = 0;
                    StartCoroutine(Typing());
                }
            }

            else if (Input.GetKeyDown(KeyCode.Mouse0) && start == false && hasCompletedLine)
            {
                NextLine();
            }


    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        start = true;
        dialoguePanel.SetActive(false);
        
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            hasCompletedLine = false;
            dialogueText.text += letter;
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(dialogueTypingSoundClip);
            yield return new WaitForSeconds(wordSpeed);
        }

        hasCompletedLine = true;
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
            StartCoroutine(SwitchPuzzleScene());
        }
    }

    IEnumerator SwitchPuzzleImmediate()
    {
        yield return new WaitForSeconds(0f);

        puzzle.SetActive(true);
        player.SetActive(false);
    }
    IEnumerator SwitchPuzzleScene()
    {
        yield return new WaitForSeconds(0.05f);
        puzzle.SetActive(true);
        player.SetActive(false);
        
    }

    private void OnTriggerEnter2D(Collider2D interact)
    {
        if (interact.CompareTag("Player"))
        {
            playerIsClose = true;
            Z.SetActive(true) ;
            display.sprite = newImage;
            Debug.Log("Player is in range");
        }
    }

    private void OnTriggerExit2D(Collider2D interact)
    {
        if (interact.CompareTag("Player"))
        {
            playerIsClose = false;
            Debug.Log("Player is out of range");
            Z.SetActive(false);
            zeroText();
        }
    }

}
