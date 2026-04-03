using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TestingQuestionsGenerator : MonoBehaviour
{

    [SerializeField]
    private MultipleChoicesHandler handler;
    [SerializeField]
    private MultipleChoiceQuestion[] questions;
    [SerializeField]
    private bool onStartRun = false;

    [Header("Settings")]
    [SerializeField]
    private bool initialDelay = false;
    [SerializeField]
    private bool gapDelay = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(onStartRun)StartCoroutine(GapDelayTest());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GapDelayTest()
    {
        if(initialDelay) yield return new WaitForSeconds(1f);
        foreach(var question in questions)
        {
            handler.AddQuestion(question);
            if(gapDelay) yield return new WaitForSeconds(1f);
        }
    }

}
