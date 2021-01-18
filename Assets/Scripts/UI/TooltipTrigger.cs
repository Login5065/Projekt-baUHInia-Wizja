
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;

    [Multiline()]
    public string content;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {

        TooltipSystem.Show(content, header);


    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {

        TooltipSystem.Hide();

    }
}
