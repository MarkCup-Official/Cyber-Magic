using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingButton : MonoBehaviour, IPointerClickHandler
{
    float cold = 0;
    int time=0;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (cold > 0)
        {
            time+=1;
            cold = 0.5f;
            if(time==5)
            {
                time=0;
                cold=0;
                Click();
            }
        }
        else
        {
            time=1;
            cold = 0.5f;
        }
    }

    public CanvasGroup go;

    bool show=false;

    public void Click(){
        show=!show;

        if(show){
            go.alpha=1;
            go.interactable=true;
            go.blocksRaycasts=true;
        }else{
            go.alpha=0;
            go.interactable=false;
            go.blocksRaycasts=false;
        }
    }

    public void Update()
    {
        if (cold > 0)
        {
            cold -= Time.deltaTime;
        }
    }
}
