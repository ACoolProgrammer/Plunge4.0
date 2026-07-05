using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoverScript : MonoBehaviour
{
[SerializeField] private SpriteRenderer spriteRenderer;
[SerializeField] private Animator animator;
 private void OnMouseEnter()
    {
        Debug.Log("Mouse entered the object!");
        animator.SetBool("IsHovered", true);
    }

       private void OnMouseExit()
    {
        animator.SetBool("IsHovered", false);
    }
}
