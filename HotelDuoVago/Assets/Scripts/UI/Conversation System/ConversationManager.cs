using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversationManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Conversation conversation;

    public float sentenceWaitTime;
    public float textSpeed;

    public void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartConversation(Conversation conv)
    {
        conversation = conv;
        sentences.Clear();
        if(conversation.canvas != null)
        {
            conversation.canvas.gameObject.SetActive(true);
        }

        Debug.Log("Starting Conversation With: " + conversation.robotName);

        if(conversation.nameText != null)
        {
            conversation.nameText.text = conversation.robotName;
        }

        foreach (string sentence in conversation.robotDialogueText)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            FindObjectOfType<KeyGrabber>().Init();
            EndConversation();
            return;
        }

        string sentence = sentences.Dequeue();
        if (conversation.sentenceText != null)
        {
            conversation.sentenceText.text = sentence;
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, textSpeed));
    }

    IEnumerator TypeSentence(string sentence, float time)
    {
        conversation.sentenceText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            conversation.sentenceText.text += letter;
            yield return new WaitForSeconds(time);
        }

        StartCoroutine(WaitForNextSentence(sentenceWaitTime));
    }


    IEnumerator WaitForNextSentence(float time)
    {
        yield return new WaitForSeconds(time);

        DisplayNextSentence();
    }

    public void EndConversation()
    {
        if(conversation.canvas != null)
        {
            conversation.canvas.gameObject.SetActive(false);
        }
        Debug.Log("End of Conversation With: " + conversation.robotName);
    }
}
