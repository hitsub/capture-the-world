using UnityEngine;

public enum InputKind{
	Get, Down, Up
}

//入力種類
public enum InputType{
	Keyboard, 
	DualShock4OnWindows,
	DualShock3OnWindows,
	DualShock4OnMac,
	DualShock3OnMac,
	DualShock2viaELECOM,
	XBOX360
}

//入力管理クラス
static public class GlobalInput{
	//tatic public Dictionary<string, GlobalInputBase> table = new Dictionary<string, GlobalInputBase> (){
	//	{"fire", new GlobalInputBase().wi
	//};
	//通常キー
	static public bool Convert(string name, InputType type, InputKind kind = InputKind.Get){
		if (name == "fire") {
			if (type == InputType.Keyboard) return Input.GetKey (KeyCode.Mouse0);
			if (type == InputType.DualShock4OnWindows) return (Input.GetKey (KeyCode.JoystickButton4) || Input.GetKey (KeyCode.JoystickButton5));
			if (type == InputType.DualShock4OnMac) return (Input.GetKey (KeyCode.JoystickButton4) || Input.GetKey (KeyCode.JoystickButton5));
			if (type == InputType.DualShock3OnWindows) return (Input.GetKey (KeyCode.JoystickButton10) || Input.GetKey (KeyCode.JoystickButton11));
			if (type == InputType.DualShock3OnMac) return (Input.GetKey (KeyCode.JoystickButton10) || Input.GetKey (KeyCode.JoystickButton11));
			if (type == InputType.DualShock2viaELECOM) return (Input.GetKey (KeyCode.JoystickButton6) || Input.GetKey (KeyCode.JoystickButton7));
			if (type == InputType.XBOX360) return (Input.GetKey (KeyCode.JoystickButton4) || Input.GetKey (KeyCode.JoystickButton5));
		}
		if (name == "boost") {
			if (type == InputType.Keyboard) return Input.GetKey (KeyCode.LeftShift);
			if (type == InputType.DualShock4OnWindows) return Input.GetKey (KeyCode.JoystickButton10);
			if (type == InputType.DualShock4OnMac) return Input.GetKey (KeyCode.JoystickButton10);
			if (type == InputType.DualShock3OnWindows) return Input.GetKey (KeyCode.JoystickButton1);
			if (type == InputType.DualShock3OnMac) return Input.GetKey (KeyCode.JoystickButton1);
			if (type == InputType.DualShock2viaELECOM) return Input.GetKey (KeyCode.JoystickButton10);
			if (type == InputType.XBOX360) return Input.GetKey (KeyCode.JoystickButton8);
		}
		if (name == "cancel") {
			if (type == InputType.Keyboard) return Input.GetKey (KeyCode.Escape);
			if (type == InputType.DualShock4OnWindows) return Input.GetKey (KeyCode.JoystickButton2);
			if (type == InputType.DualShock4OnMac) return Input.GetKey (KeyCode.JoystickButton2);
			if (type == InputType.DualShock3OnWindows) return Input.GetKey (KeyCode.JoystickButton13);
			if (type == InputType.DualShock3OnMac) return Input.GetKey (KeyCode.JoystickButton13);
			if (type == InputType.DualShock2viaELECOM) return Input.GetKey (KeyCode.JoystickButton1);
			if (type == InputType.XBOX360) return Input.GetKey (KeyCode.JoystickButton1);
		}
		if (name == "jump") {
			if (type == InputType.Keyboard) {
				if (kind == InputKind.Get)
					return Input.GetKey (KeyCode.Space);
				if (kind == InputKind.Down)
					return Input.GetKeyDown (KeyCode.Space);
			}
			if (type == InputType.DualShock4OnWindows) return Input.GetKey (KeyCode.JoystickButton1);
			if (type == InputType.DualShock4OnMac) return Input.GetKey (KeyCode.JoystickButton1);
			if (type == InputType.DualShock3OnWindows) return Input.GetKey (KeyCode.JoystickButton14);
			if (type == InputType.DualShock3OnMac) return Input.GetKey (KeyCode.JoystickButton14);
			if (type == InputType.DualShock2viaELECOM) return Input.GetKey (KeyCode.JoystickButton2);
			if (type == InputType.XBOX360) return Input.GetKey (KeyCode.JoystickButton0);
		}
		if (name == "decide") {
			if (type == InputType.Keyboard) return Input.GetKeyDown (KeyCode.Return);
			if (type == InputType.DualShock4OnWindows) return Input.GetKeyDown (KeyCode.JoystickButton1);
			if (type == InputType.DualShock4OnMac) return Input.GetKeyDown (KeyCode.JoystickButton1);
			if (type == InputType.DualShock3OnWindows) return Input.GetKeyDown (KeyCode.JoystickButton14);
			if (type == InputType.DualShock3OnMac) return Input.GetKeyDown (KeyCode.JoystickButton14);
			if (type == InputType.DualShock2viaELECOM) return Input.GetKeyDown (KeyCode.JoystickButton2);
			if (type == InputType.XBOX360) return Input.GetKeyDown (KeyCode.JoystickButton0);
		}
		return false;
	}
	static public float Arrow(string name, InputType type){
		if (name == "up") {
			if (type == InputType.Keyboard) {
				if (Input.GetKey(KeyCode.UpArrow)) return -1f;
			}
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis8")*(-1f);
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis8");
			//if (type == InputType.DualShock3OnWindows) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock3OnMac) return (Input.GetKey (KeyCode.JoystickButton4)) ? -1f : 0;
			if (type == InputType.DualShock2viaELECOM) return Input.GetAxis ("Axis6")*(-1f);
			if (type == InputType.XBOX360) return Input.GetAxis ("Axis7")*(-1f);
		}
		if (name == "down") {
			if (type == InputType.Keyboard) {
				if (Input.GetKey(KeyCode.DownArrow)) return 1f;
			}
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis8")*(-1f);
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis8");
			//if (type == InputType.DualShock3OnWindows) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock3OnMac) return (Input.GetKey (KeyCode.JoystickButton6)) ? -1f : 0;
			if (type == InputType.DualShock2viaELECOM) return Input.GetAxis ("Axis6")*(-1f);
			if (type == InputType.XBOX360) return Input.GetAxis ("Axis7")*(-1f);
		}
		return 0;
	}
	//軸取得
	static public float Axis(string name, InputType type){
		if (name =="MoveHorizontal"){
			if (type == InputType.Keyboard) {
				if (Input.GetKey(KeyCode.D)) return 1f;
				if (Input.GetKey(KeyCode.A)) return -1f;
			}
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock3OnWindows) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock3OnMac) return Input.GetAxis ("Axis1");
			if (type == InputType.DualShock2viaELECOM) return Input.GetAxis ("Axis1");
			if (type == InputType.XBOX360) return Input.GetAxis ("Axis1");
		}
		if (name =="MoveVertical"){
			if (type == InputType.Keyboard) {
				if (Input.GetKey(KeyCode.W)) return 1f;
				if (Input.GetKey(KeyCode.S)) return -1f;
			}
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis2");
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis2");
			if (type == InputType.DualShock3OnWindows) return Input.GetAxis ("Axis2");
			if (type == InputType.DualShock3OnMac) return Input.GetAxis ("Axis2");
			if (type == InputType.DualShock2viaELECOM) return Input.GetAxis ("Axis2");
			if (type == InputType.XBOX360) return Input.GetAxis ("Axis2");
		}
		if (name =="EyeHorizontal"){
			if (type == InputType.Keyboard) return Input.GetAxis ("Mouse X");
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis3");
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis3");
		}
		if (name =="EyeVertical"){
			if (type == InputType.Keyboard) return Input.GetAxis ("Mouse Y");
			if (type == InputType.DualShock4OnWindows) return Input.GetAxis ("Axis6");
			if (type == InputType.DualShock4OnMac) return Input.GetAxis ("Axis4");
			if (type == InputType.DualShock3OnWindows) return Input.GetAxis ("Axis4");
			if (type == InputType.DualShock3OnMac) return Input.GetAxis ("Axis4");
			if (type == InputType.DualShock2viaELECOM) return Input.GetAxis ("Axis4");
			if (type == InputType.XBOX360) return Input.GetAxis ("Axis5");
		}
		return 0;
	}
}

//視点操作
[System.Serializable]
public class Look
{
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

	public void Init(Transform character, Transform camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
	}


	public void LookRotation(Transform character, Transform camera, InputType type)
	{
		float yRot = GlobalInput.Axis("EyeHorizontal", type) * XSensitivity;
		float xRot = GlobalInput.Axis("EyeVertical", type) * YSensitivity;

		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		if(clampVerticalRotation)
			m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
			camera.localRotation = m_CameraTargetRot;
		}

		UpdateCursorLock();
	}

	public void SetCursorLock(bool value)
	{
		lockCursor = value;
		if(!lockCursor)
		{//we force unlock the cursor if the user disable the cursor locking helper
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void UpdateCursorLock()
	{
		//if the user set "lockCursor" we check & properly lock the cursos
		if (lockCursor)
			InternalLockUpdate();
	}

	private void InternalLockUpdate()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			m_cursorIsLocked = false;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			m_cursorIsLocked = true;
		}

		if (m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

}