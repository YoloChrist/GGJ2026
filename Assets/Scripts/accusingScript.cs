using UnityEngine;

public class accusingScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        PlayerCollision.accuseNPC += checkAccused;
    }

    private void OnDisable()
    {
        PlayerCollision.accuseNPC -= checkAccused;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkAccused(npcDialogueData npc)
    {
        if (npc.gameObject.tag == "villain")
        {
            Debug.Log("GOT HIM!");
        }
        else
        {
            Debug.Log("nah get out");
        }
    }
}
