﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBehavior : MonoBehaviour
{
    //cached component
    TextMeshProUGUI tmpro;

    //cached objects
    InteractionCanvas canvas;
    DialogueText d;

    //Dialoguebox fields
    //holds prefab of the object that will tell the dialogue what to say
    [SerializeField] Interactable owner;
    //[SerializeField] string filename = "example1";
    [SerializeField] float txtSpeed = 0.1f;
    //TextAsset txt; unsused for now
    //arraylist that holds all dialogue buttons that are active
    ArrayList buttons = new ArrayList();

    //array to store sentences
    [SerializeField] string[] sentences;
    private int currIndex;


    // Start is called before the first frame update
    void Start()
    {
        //get canvas with the script interactionCanvas
        canvas = FindObjectOfType<InteractionCanvas>();
        //get this object's text mesh pro component
        tmpro = GetComponent<TextMeshProUGUI>();

        //load text file from the resources folder. Has to be from the resources folder.
        //txt = Resources.Load("TextFiles/" + owner.GetFileName()) as TextAsset; unsused for now

        //Loading the dialogue manager associated to our owner. Because we only have a reference to the owner's prefab, it will search for the file with the name that is stated in the prefab, not the instance
        d = Resources.Load("TextFiles/" + owner.GetFileName()) as DialogueText;

        //get all sentences in our dialogueText object
        sentences = d.sentences;//txt.text.Split('\n'); <---Previous way to do it

        //start index at second position because we will show the first one on instantiation
        currIndex = 1;
        //on creation show first sentence
        StartCoroutine(ShowText(sentences[0]));
    }

    
    //Coroutine to show next char sequentially
    IEnumerator ShowText(string text)
    {
        //first it is empty
        tmpro.text = "";
        /*// At the moment we dont need this chunk of code but we will when we decide to incorporate buttons at the end of dialogue
        if(text.Length == 0)
        {
            ShowAnswerOptions();
        }
        
        else
        {
            foreach (char c in text.ToCharArray())
            {
                //show next char
                tmpro.text += c;
                //wait txtSpeed amount of seconds before showing next char
                yield return new WaitForSeconds(txtSpeed);
            }
        }
        */
        
        foreach (char c in text.ToCharArray())
        {
            //show next char
            tmpro.text += c;
            //wait txtSpeed amount of seconds before showing next char
            yield return new WaitForSeconds(txtSpeed);
        }
    }
    

    
    //method the continue button will call whenever it is pressed
    public void showNextSentence()
    {

        //stops any coroutines currently occurring to not obstruct the one we will start in our else statement
        StopAllCoroutines();
        int size = sentences.Length;

        //Destroy box if the last sentence was shown
        if (currIndex >= size) //last sentence will be empty which will be where we show the options
        {
            canvas.DestroyBox();
        }
        //otherwise show next sentence in the sentences array
        else
        {
            StartCoroutine(ShowText(sentences[currIndex]));
            currIndex++;
        }
        
    }
    

    //for the buttons
    public void ShowAnswerOptions()
    {

        foreach (GameObject go in owner.getButtons())
        {
            //instantiates the button and adds it to the arrayList;
            buttons.Add(Instantiate(go, transform));
        }
    }

    //for the buttons
    public void DestroyAllDialogueButtons()
    {
        //destroy every single dialogue button in the arrayList
        foreach (GameObject go in buttons)
        {
            Destroy(go);
        }
        //reset arraylist of buttons
        buttons = new ArrayList();
    }

}
