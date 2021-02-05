using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public static FloatingText Instance;
    public TextMeshProUGUI FloatingTextPrefab;

    void Awake()
    {
        Instance = this;
    }
    public void TriggerFloatingText(string text, Vector3 worldPos, Color color, float duration, bool moveUp)
    {
        var obj = Instantiate(FloatingTextPrefab);
        obj.text = text;
        obj.rectTransform.position = Camera.main.WorldToScreenPoint(worldPos + Vector3.up);
        obj.rectTransform.parent = transform;
        obj.color = color;
        //Destroy(obj, duration + 0.1f);
        obj.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InElastic);
        if (moveUp)
            obj.rectTransform.DOMoveY(Camera.main.pixelHeight * 0.1f, duration).SetRelative().SetEase(Ease.OutBounce);
    }
}
