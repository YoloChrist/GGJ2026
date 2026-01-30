using UnityEngine;

public class npcspriteManager : MonoBehaviour
{
    [SerializeField] private Sprite[] npcGeneric;
    [SerializeField] private Sprite[] npcSpecial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getSprite(int index, string which)
    {
        switch (which)
        {
            case "generic":
                return npcGeneric[index];
            case "special":
                return npcSpecial[index];
        }
        return null;
    }
}
