using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        private float m_WalkSpeed;
        private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private Look m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
		private bool m_Boost;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
		private bool m_Jumping;
		[SerializeField]private AudioSource m_AudioSource;
		[SerializeField]private AudioSource SE;
		[SerializeField]private AudioSource Jet;

		[SerializeField] private Text textSpeed;
		private Rigidbody rigid;
		private Manager manager;

		PlayerData pData;
		[SerializeField] int playerNum;
		public FlagType flagColor;
		[SerializeField] InputType inputType;

		ShotAreaTrigger shotArea;

		GameObject tmpCrystal; //現在の操作対象

		//ステータス
		public float Energy;
		int MaxEnergy, Mileage, RecoveryCooltime, Recovery, WalkRecoveryBonus, Damage, FireRate;
		float nonActiveTime = 0f; //走らないかつブーストしてない時間
		float shootCoolTime = 0f;
		FlagType side;

        // Use this for initialization
        private void Start()
        {
			pData = GameObject.Find ("PlayerData").GetComponent<PlayerData> ();

            m_CharacterController = GetComponent<CharacterController>();
			m_Camera = transform.FindChild("FirstPersonCharacter").gameObject.GetComponent<Camera>();
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
			m_MouseLook.Init(transform , m_Camera.transform);

			rigid = this.gameObject.GetComponent<Rigidbody> ();
			manager = GameObject.Find("Manager").GetComponent<Manager>();
			shotArea = transform.FindChild ("ShotArea").gameObject.GetComponent<ShotAreaTrigger> ();

			//ステータス初期化
			Energy = pData.energy [playerNum];
			MaxEnergy = (int)Energy;
			m_WalkSpeed = pData.WalkSpeed [playerNum];
			m_RunSpeed = pData.RunSpeed [playerNum];
			Mileage = pData.Mileage [playerNum];
			RecoveryCooltime = pData.RecoveryCooltime [playerNum];
			Recovery = pData.Recovery [playerNum];
			WalkRecoveryBonus = pData.WalkRecoveryBonus [playerNum];
			Damage = pData.Damage [playerNum];
			FireRate = pData.FireRate [playerNum];
			side = pData.Team [playerNum];

			shootCoolTime = 60f / pData.FireRate [playerNum];

        }


        // Update is called once per frame
        private void Update()
        {
			if (!manager.isStart)
				return;
			if (manager.isEndGame) {
				if (GlobalInput.Convert ("fire", inputType)) {
					Destroy (GameObject.Find ("PlayerData"));
					SceneManager.LoadScene ("Start");
				}
				return;
			}

			//視線操作
			m_MouseLook.LookRotation (transform, m_Camera.transform, inputType);

            // the jump state needs to read here to make sure it is not missed
			//ジャンプフラグ
			if (!m_Jump)
				m_Jump = GlobalInput.Convert ("jump", inputType, InputKind.Down);
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
			//ブーストフラグ
			m_Boost = (GlobalInput.Convert ("jump", inputType) &&  (Energy > 0));
			//m_Boost = (CrossPlatformInputManager.GetButton ("Jump") && (Energy > 0));
			if (m_Boost && !(Jet.isPlaying))
				//Jet.Play ();
			if (!m_Boost)
				//Jet.Stop ();

			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded){
                StartCoroutine(m_JumpBob.DoBobCycle());
                //PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

			//エネルギー注入
			if (GlobalInput.Convert("fire", inputType) && shotArea.isShot && Energy >= pData.Damage [playerNum]) {
				shootCoolTime += 1f * Time.deltaTime;
				if (shootCoolTime >= 60f / pData.FireRate [playerNum]) {
					shootCoolTime = 0;
					InjectResult res = shotArea.col.gameObject.transform.parent.gameObject.GetComponent<CrystalScript> ().InjectEnergy (pData.Damage [playerNum], side);
					//注入成功したら(敵と競合しなかったら)判定
					if (res.Result)
						Energy -= pData.Damage [playerNum];
					if (res.Capture || res.Destroy) {
						Energy += 200;
						manager.GetScore (10, side);
					}
				}
			}

			//エネルギー消費
			if (m_Boost)
				Energy -= (pData.Mileage [playerNum] * Time.deltaTime);
			if (!m_IsWalking)
				Energy -= 0.7f * (pData.Mileage [playerNum] * Time.deltaTime);
			
			//エネルギー自然回復
			if (m_IsWalking && !m_Boost)
				nonActiveTime += 1f * Time.deltaTime;
			else
				nonActiveTime = 0;
			if (nonActiveTime > RecoveryCooltime)
					Energy += Recovery * Time.deltaTime;

			//エネルギーの下限上限
			if (Energy < 0)
				Energy = 0;
			if (Energy > MaxEnergy)
				Energy = MaxEnergy;
        }


        private void PlayLandingSound()
        {
            //m_AudioSource.clip = m_LandSound;
            //m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
			if ((!manager.isStart) || (manager.isEndGame))
				return;

            float speed;
			GetInput(out speed);

            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    //PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
				m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
				if (m_Boost)
					m_MoveDir += new Vector3 (0, 0.75f, 0);
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }

		void OnTriggerEnter (Collider col){
			if (col.gameObject.tag == "Crystal") {
				manager.CatchCrystal ();
				col.gameObject.transform.parent.gameObject.GetComponent<CrystalScript> ().Kill ();
				//SE.Play ();

			}
		}

        private void PlayJumpSound()
        {
            //m_AudioSource.clip = m_JumpSound;
            //m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            //PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
			float horizontal = GlobalInput.Axis("MoveHorizontal", inputType);
			float vertical = GlobalInput.Axis("MoveVertical", inputType);

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
			m_IsWalking = !GlobalInput.Convert("boost", inputType);

#endif
            // set the desired speed to be walking or running
			speed = m_IsWalking ? m_WalkSpeed :(Energy >50) ? m_RunSpeed : m_WalkSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
//            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
