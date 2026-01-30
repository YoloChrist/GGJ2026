using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class startGameBtn : MonoBehaviour
{
    [SerializeField] private dialogueHandler dh;
    [SerializeField] private GameObject dialogueUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        dialogueUI.SetActive(true);
        dh.startMenuText();
    }
}
