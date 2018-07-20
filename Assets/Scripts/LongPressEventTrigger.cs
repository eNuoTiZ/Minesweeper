using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float durationThreshold = 3.0f;

    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onLongPress = new UnityEvent();

    //private bool isPointerDown = false;
    private bool hasPointerExited = false;
    //private bool longPressTriggered = false;
    private float timePressStarted;

    //private void Update()
    //{
    //    if (isPointerDown && !longPressTriggered && !hasPointerExited)
    //    {
    //        if (Time.time - timePressStarted > durationThreshold)
    //        {
    //            longPressTriggered = true;
    //            onLongPress.Invoke();
    //        }
    //        else
    //        {
    //            longPressTriggered = false;
    //            onClick.Invoke();
    //        }
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        timePressStarted = Time.time;
        //isPointerDown = true;
        //longPressTriggered = false;
        hasPointerExited = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //isPointerDown = false;
        if (hasPointerExited)
        {
            return;
        }
        if (Time.time - timePressStarted > durationThreshold && !hasPointerExited)
        {
            //longPressTriggered = true;
            onLongPress.Invoke();
        }
        else
        {
            //longPressTriggered = false;
            onClick.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hasPointerExited = true;
    }

    public void OnClicked(bool isLongPress)
    {
        gameObject.GetComponent<CellComponent>().OnClick(isLongPress);
        //int row = gameObject.GetComponent<CellComponent>().x;
        //int col = gameObject.GetComponent<CellComponent>().y;

        //Board.Instance().Cells[row, col].Selected = true;

    }
}