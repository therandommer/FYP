using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField]
    List<Sprite> backgrounds; //holds potential backgrounds for player to choose
    int activeBackground = 0;
    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    Slider backgroundSlider;
    [SerializeField]
    Text backgroundText;
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Awake()
    {
        backgroundText.text = "Background: " + ((int)backgroundSlider.value + 1);
    }
    private void Update()
    {
        if(activeBackground != backgroundSlider.value)
        {
            activeBackground = (int)backgroundSlider.value;
            sprite.sprite = backgrounds[(int)backgroundSlider.value];
            backgroundText.text = "Background: " + ((int)backgroundSlider.value + 1);
        }
    }
    public int GetBackgroundID()
    {
        return activeBackground;
    }
}
