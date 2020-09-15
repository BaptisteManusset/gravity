using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using UnityEngine.SceneManagement;

public class Karen : MonoBehaviour
{
  public static Karen instance;

  private void Awake()
  {
    if(instance == null)
    {
      instance = this;
    } else
    {
      Destroy(gameObject);
    }



    DontDestroyOnLoad(gameObject);
  }
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.R))
    {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
#if UNITY_EDITOR

      Selection.activeGameObject = GameObject.FindGameObjectWithTag("Player");
#endif
    }
  }
}
