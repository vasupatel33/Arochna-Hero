using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ElementHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Attributes")]
    [SerializeField] private float scale;

    [Header("References")]
    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        text.fontSize *= scale;
    }

    public void OnPointerExit(PointerEventData data)
    {
        text.fontSize /= scale;
    }
}
