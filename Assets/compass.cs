using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{
  [SerializeField] Transform target;
  void Update()
  {
    transform.rotation = target.rotation;
  }
}
