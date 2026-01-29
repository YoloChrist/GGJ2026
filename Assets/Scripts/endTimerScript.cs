using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class endTimerScript : MonoBehaviour
{
    [SerializeField] private int goodNPCsSpoeknTo = 0;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int totalTime = 60;
    [SerializeField] private int currentTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = totalTime;
        timerText.SetText("Conversations Left: " + (5-goodNPCsSpoeknTo).ToString());
    }

    void OnEnable()
    {
        npcDialogueData.addToTimerCounter += addToCounter;
    }

    void OnDisable()
    {
        npcDialogueData.addToTimerCounter -= addToCounter;
    }

    // Update is called once per frame
    void addToCounter()
    {
        goodNPCsSpoeknTo++;
        if (goodNPCsSpoeknTo == 5)
        {
            goodNPCsSpoeknTo = 6;
            StartCoroutine(timer());
        }
        else
        {
            timerText.SetText("Conversations Left: " + (5-goodNPCsSpoeknTo).ToString());
        }
    }

    IEnumerator timer()
    {
        while (currentTime >= 0)
        {
            timerText.SetText(currentTime.ToString());
            currentTime--;
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene("loseScene");    
    }
}
