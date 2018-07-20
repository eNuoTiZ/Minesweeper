using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonLongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    private float holdTime = 1f;

    private bool held = false;
    private bool hasPointerExited = false;
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onLongPress = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        hasPointerExited = false;
        held = false;
        Invoke("OnLongPress", holdTime);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CancelInvoke("OnLongPress");

        if (!held && !hasPointerExited)
            onClick.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasPointerExited = true;
        CancelInvoke("OnLongPress");
        CancelInvoke("OnClick");
    }

    void OnLongPress()
    {
        held = true;
        onLongPress.Invoke();
    }

    public void OnClick(bool isLongPress)
    {
        if (isLongPress && Options.Instance.Vibrations)
        {
            Vibration.Vibrate(200);
            //Handheld.Vibrate();
        }

        gameObject.GetComponent<CellComponent>().OnClick(isLongPress);
    }
    
}