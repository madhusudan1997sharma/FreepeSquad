using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Control_Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image BorderCircle;
    public Image CenterCircle;
    private Vector3 inputVector;

    private void Start()
    {

    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(BorderCircle.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / BorderCircle.rectTransform.sizeDelta.x);
            pos.y = (pos.y / BorderCircle.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            CenterCircle.rectTransform.anchoredPosition = new Vector3(inputVector.x * (BorderCircle.rectTransform.sizeDelta.x / 2.3f), inputVector.z * (BorderCircle.rectTransform.sizeDelta.y / 2.3f));
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        CenterCircle.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }
}