using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonUserControlCustom : MonoBehaviour {

        [SerializeField] private ThirdPersonCharacterCustom m_Character; // A reference to the ThirdPersonCharacter on the object
    [SerializeField] OTcontrols ot_Character;
    [SerializeField] QTControls qt_Character;
        [SerializeField] private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        [SerializeField] private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

    public Transform Cam
    {
        get
        {
            return m_Cam;
        }

        set
        {
            m_Cam = value;
        }
    }

    private void OnEnable()
        {
            Camera_Selector.CamHasChanged += CameraSwitch;
        }

        private void OnDisable()
        {
            Camera_Selector.CamHasChanged -= CameraSwitch;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        void CameraSwitch(Camera newCam)
        {
            print("camera has switched to " + newCam);
            m_Cam = newCam.transform;
        }

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacterCustom>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Action");
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);
        if (ot_Character != null)
        {
            h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            v = CrossPlatformInputManager.GetAxisRaw("Vertical");
        }

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
          //  print("calculating based on camera position");
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
           // print("calculating based on world position");
        }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            if (this.enabled)
        {
            if (ot_Character != null)
                ot_Character.Move(m_Move);
            else if (qt_Character != null)
                qt_Character.Move(m_Move);
            else
                m_Character.Move(m_Move, m_Jump);
        }
            
            m_Jump = false;
        }
    
}
