using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false;
    }

    private void Update()
    {
        // for preview of changes in edit mode
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit) ? true : false; 
        }

        Vector2 position = Input.mousePosition;


        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;


        //makes sure that tooltip won't be rendered offscreen
        if (pivotX > 0.75)
            pivotX = 1;
        else
            pivotX = 0;

        if (pivotY < 0.25)
            pivotY = 0;
        else     
            pivotY = 1;

        rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
}
