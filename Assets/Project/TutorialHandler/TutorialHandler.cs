using UnityEngine;
using System.Collections.Generic;
using System;

/**
 * @brief Manages a sequential tutorial by toggling between a list of CanvasGroups.
 */
public class TutorialHandler : Singleton<TutorialHandler>
{
    [SerializeField]
    private List<CanvasGroup> frames = new List<CanvasGroup>();

    public Action OnTutorialComplete;

    private int m_currentFrameIndex = 0;

    private void Start()
    {
        bool shouldShowTutorial = LevelSelector.HasInstance && 
                                 LevelSelector.Instance.selectedLevel != null && 
                                 LevelSelector.Instance.selectedLevel.hasTutorial;

        if (shouldShowTutorial)
        {
            InitializeFrames();
        }
        else
        {
            CompleteTutorial();
        }
    }

    /**
     * @brief Ensures only the first frame is active at the start.
     */
    private void InitializeFrames()
    {
        if (frames.Count == 0) 
        {
            CompleteTutorial();
            return;
        }

        m_currentFrameIndex = 0;
        UpdateFrameVisibility();
    }

    /**
     * @brief Advances the tutorial to the next frame.
     * * If it's the last frame, the tutorial completes and invokes the completion event.
     */
    public void NextFrame()
    {
        m_currentFrameIndex++;

        if (m_currentFrameIndex < frames.Count)
        {
            UpdateFrameVisibility();
        }
        else
        {
            CompleteTutorial();
        }
    }

    /**
     * @brief Sets the visibility and interactivity of all frames based on the current index.
     */
    private void UpdateFrameVisibility()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            bool isActive = (i == m_currentFrameIndex);
            
            // Set CanvasGroup properties
            frames[i].alpha = isActive ? 1f : 0f;
            frames[i].interactable = isActive;
            frames[i].blocksRaycasts = isActive;

            // Also toggle the GameObject active state for hierarchy cleanliness and performance
            frames[i].gameObject.SetActive(isActive);
        }
    }

    /**
     * @brief Logic to handle the end of the tutorial (e.g., hiding the UI or triggering events).
     */
    private void CompleteTutorial()
    {
        Debug.Log("TutorialHandler: Tutorial completed.");
        
        // Hide all frames
        foreach (var frame in frames)
        {
            if (frame != null)
            {
                frame.alpha = 0f;
                frame.interactable = false;
                frame.blocksRaycasts = false;
                frame.gameObject.SetActive(false);
            }
        }

        OnTutorialComplete?.Invoke();
    }
}
