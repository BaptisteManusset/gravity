using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

#pragma warning disable 618, 649

public class Player : MonoBehaviour
{
  public float speedFront = 10;
  public float speedCoter = 10;
  public float rotationSpeed = 100;
  public float boost = 10;

  CharacterController rb;
  [SerializeField] Camera m_Camera;

  //[SerializeField] private MouseLook m_Mouselook;
  [SerializeField] LayerMask layerMask;


  [Header("Camera")]
  public float XSensitivity = 2f;
  public float YSensitivity = 2f;
  public bool clampVerticalRotation = true;
  public float MinimumX = -90F;
  public float MaximumX = 90F;
  public bool smooth;
  public float smoothTime = 5f;
  public bool lockCursor = true;


  private Quaternion m_CharacterTargetRot;
  private Quaternion m_CameraTargetRot;
  private bool m_cursorIsLocked = true;


  [Header("Physic")]
  public bool gravity;
  private void Awake()
  {
    rb = GetComponent<CharacterController>();

  }

  private void RotateView()
  {
    float yRot = Input.GetAxis("Mouse X") * XSensitivity;
    float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

    m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
    m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

    transform.rotation *= Quaternion.Euler(-xRot, yRot, 0f);



    UpdateCursorLock();
  }


  void Update()
  {
    RotateView();

    Movement();


    if (Input.GetKeyDown(KeyCode.LeftControl))
    {
      AlignWithGround(1);
    }

    if (Input.GetKeyDown(KeyCode.Tab))
    {
      gravity = !gravity;
    }
  }

  private void AlignWithGround(float speed = 0.1f)
  {
    RaycastHit hit;
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 10, layerMask))
    {
      Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
      Quaternion wantedRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
      transform.DORotateQuaternion(wantedRotation, speed);
    }

  }

  private void Movement()
  {
    //boost
    float boostPower = Input.GetAxis("Fire3") * boost * Time.deltaTime + 1;

    //vertical boost
    float translationVertical = Input.GetAxis("Jump") * speedFront * Time.deltaTime;
    var movement = m_Camera.transform.TransformDirection(Vector3.up) * translationVertical * boostPower;
    rb.Move(movement);


    //gauche droite
    float translationHori = Input.GetAxis("Horizontal") * speedCoter * Time.deltaTime;
    movement = m_Camera.transform.TransformDirection(Vector3.right) * translationHori * boostPower;
    rb.Move(movement);

    //avant arriere
    float translation = Input.GetAxis("Vertical") * speedFront * Time.deltaTime;
    movement = m_Camera.transform.TransformDirection(Vector3.forward) * translation * boostPower;
    rb.Move(movement);

    //rotation
    float rotationVerti = Input.GetAxis("Rotation") * rotationSpeed * Time.deltaTime;
    Vector3 Rot = new Vector3(0, 0, -rotationVerti) * boostPower;
    transform.Rotate(Rot);

  }

  private void OnGUI()
  {
    string debug = "";
    debug += $"Gravity: {gravity} ";
    //debug += $"B  Rot: {transform.rotation} Pos: {transform.position}";
    //debug += $"B  Rot: {m_Camera.transform.rotation} Pos: {m_Camera.transform.position}";

    GUI.Label(new Rect(0, 0, 300, 100), debug);
  }

  public void UpdateCursorLock()
  {
    //if the user set "lockCursor" we check & properly lock the cursos
    if (lockCursor)
      InternalLockUpdate();
  }
  private void InternalLockUpdate()
  {
    if (Input.GetKeyUp(KeyCode.Escape))
    {
      m_cursorIsLocked = false;
    } else if (Input.GetMouseButtonUp(0))
    {
      m_cursorIsLocked = true;
    }

    if (m_cursorIsLocked)
    {
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
    } else if (!m_cursorIsLocked)
    {
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
    }
  }


  private void LateUpdate()
  {
    if (gravity) ApplyGravity();

  }

  private void ApplyGravity()
  {
    AlignWithGround(.5f);

  }
}

