using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<Text>().color = Color.white;
    }

    private void Start()
    {
        this.GetComponent<Text>().color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<Text>().color = Color.grey;
    }
}
