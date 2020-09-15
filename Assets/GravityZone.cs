using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      Player p = other.gameObject.GetComponent<Player>();
      p.gravity = true;
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      Player p = other.gameObject.GetComponent<Player>();
      p.gravity = false;
    }
  }
}
