using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gem : MonoBehaviour, IPointerClickHandler
{

    //Click Event Handler
    public void OnPointerClick(PointerEventData eventData)
    {
        //only works if player hasn't lost yet
        if(!RoundsManager.instance.playerLost)
            GemWasClicked();
    }



    public string gemName;

    //EXPANSION: some gems may give more points than others, the base chain multiplier can be used to control that
    public float baseChainMultiplier = 1.0f;


    //keep positions in gem matrix
    public int i_index;
    public int j_index;

    public void GemWasClicked()
    {
        //plays "selected" sound
        SoundManager.instance.PlaySelectGemOneShot();

        //marks the current gem as highlighted
        GemMatrixManager.instance.setNewHighlightedGem(i_index, j_index);
    }



    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(gemName + "'s  width: " + gameObject.GetComponent<RectTransform>().rect.width);
        //Debug.Log(gemName + "'s  height: " + gameObject.GetComponent<RectTransform>().rect.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
