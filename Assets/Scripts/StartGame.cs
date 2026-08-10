using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
  [SerializeField] private SpriteRenderer spriteRenderer;
  [SerializeField] private Animator animator;
    public async void PlayGame()
{
  AsyncOperation op = SceneManager.LoadSceneAsync("LevelHub");

  await op;

}

}