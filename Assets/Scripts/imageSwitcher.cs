using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageSwitcher : MonoBehaviour
{
    public bool isAvailable;
    public Sprite offSprite;
    private Sprite onSprite;

    // Start is called before the first frame update
    void Start()
    {
        onSprite = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAvailable)
        {
            GetComponent<Image>().sprite = onSprite;
        }

        else
        {
            GetComponent<Image>().sprite = offSprite;
        }
    }
}
