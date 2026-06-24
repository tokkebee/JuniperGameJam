using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator buttonAnim;

    private void Start()
    {
        buttonAnim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData) //Needs to be public for class inheritance
    {
        buttonAnim.SetBool("isHovering", true);
    } 
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonAnim.SetBool("isHovering", false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        buttonAnim.SetTrigger("Click");
    }
}
