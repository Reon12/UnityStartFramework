using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Enums;

public sealed class PlayerCharacterMovement : MonoBehaviour
{

	[Header("달리기 이동 속력")]
	[SerializeField] public float _MoveSpeed = 6.0f;

	[Header("가속률")] [Range(10.0f, 10000.0f)]
	[Tooltip("1 초에 걸쳐 최대 속력의 몇 퍼센트만큼 가속되도록 할 것인지를 결정합니다.")]
	[SerializeField] private float _OneSecAcceleration = 600.0f;

	[Header("Orient Rotation To Movement")]
	[Tooltip("이동하는 방향으로 회전시킵니다.")]
	[SerializeField] private bool _UseOrientRotationToMovement = true;

	[Header("Yaw 회전 속력")]
	[Tooltip("이동하는 방향으로 회전시킵니다. (UseOrientRotationToMovement 속성을 사용할 때 적용됩니다.)")]
	[SerializeField] private float _RotationYawSpeed = 720.0f;

	[Header("점프 힘")]
	[SerializeField] private float _JumpVelocityY = 10.0f;

	[Header("적용되는 중력 스케일")] [Space(30.0f)]
	[SerializeField] private float _GravityScale = 1.0f;

	[Header("최대 Y 속력")]
	[SerializeField] private float _MaxYVelocity = 10.0f;

	[Header("지형 검사 시 무시할 레이어")]
	[SerializeField] private LayerMask _IgnoreGroundLayers;

	// 캐릭터 컴포넌트를 나타냅니다.
	private PlayerCharacterBase _PlayerableCharacter; // 기존 컴포넌트 = PlayerableCharacter

	[Header("Use SpringArm Component")]
	[SerializeField] private SpringArm _SprintArm; // 새로 추가한 컴포넌트

	[Header("Use Weapon")]
	[SerializeField] private GameObject _Weapon;


	#region Move
	// 이동 입력 값을 나타냅니다.
	private Vector3 _InputVector;

	// 목표 속도를 나타냅니다.
	private Vector3 _TargetVelocity;

	// 속도를 나타냅니다.
	private Vector3 _Velocity;

	// 충격 속도
	private Vector3 _ImpulseVelocity;
	#endregion

	#region Jump
	// 점프키 입력 끝 상태를 나타냅니다.
	private bool _IsJumpInputFinished = true;

	#endregion

	// 속도를 나타냅니다.
	public Vector3 velocity => characterController.velocity;

	// Y 축을 제외한 속도를 나타냅니다.
	public Vector3 moveXZVelocity
	{
		get
		{
			Vector3 velocity = characterController.velocity;
			velocity.y = 0.0f;
			return velocity;
		}
	}

	// 땅에 닿음 상태를 나타냅니다.
	public bool isGrounded { get; private set; }

	public bool isDashable { get; set; }

	// 이동 가능 상태를 나타냅니다.
	public bool isMovable => !_PlayerableCharacter.skillController.blockMovement;

	// 무기 사용 여부를 나타냅니다.
	public bool useWeapon { get; private set; } = true;

	// 점프 가능 상태를 나타냅니다.
	public bool isJumpable =>
		// 이동 가능 상태이며
		isMovable

		// 땅에 닿아있거나
		&& (isGrounded)

		// 점프 입력이 끝났을 경우 점프할 수 있도록 합니다.
		&& _IsJumpInputFinished;


	// CharacterController 컴포넌트를 나타냅니다.
	public CharacterController characterController { get; private set; }

	// 땅으로 착지 시 호출되는 대리자입니다.
	/// - Param : 남은 점프 카운트
	public System.Action<int> onLanded;

	// 점프 시작 시 호출되는 대리자입니다.
	/// - Param : 남은 점프 카운트
	public System.Action<int> onJumpStarted;

	private void Awake()
	{
		_PlayerableCharacter = GetComponent<PlayerCharacterBase>();
		characterController = GetComponent<CharacterController>();
		isDashable = false;

	}

	private void Update()
	{
        // 캐릭터를 회전시킵니다.
        OrientRotationToMovement();

		// 대쉬키 입력을 확인합니다.
		DashInput();

	}

	private void FixedUpdate()
	{
		// 이동 입력 값을 초기화합니다.
		_InputVector = Vector3.zero;

		_InputVector.x = InputManager.GetAxis("Horizontal");
		_InputVector.z = InputManager.GetAxis("Vertical");
		_InputVector.Normalize();

		// 이동 입력 값을 카메라 방향으로 변환합니다.
		_InputVector = _PlayerableCharacter.springArm.InputToCameraDirection(_InputVector); // 기존에 _PlayerableCharacter.springArm 였음

		// 점프 입력 처리를 합니다.
		if (InputManager.GetAction("Jump", ActionEvent.Stay))
			JumpInput();
		else FinishJumpInput();


		// 속도를 계산합니다.
		if (InputManager.GetAction("Dash", ActionEvent.Stay))
        {
			CalculateVelocity();
        }
		else CalculateVelocity();



		// 중력을 계산합니다.
		CalculateGravity();

		// 캐릭터를 이동시킵니다.
		MoveCharacter();

		// 땅 착지 상태를 갱신합니다.
		UpdateGroundedState();

	}

	// 속도를 계산합니다.
	private void CalculateVelocity()
	{

		// 입력 값을 연산합니다.
		void CalculateInputVector()
		{
			_TargetVelocity.x = _InputVector.x * _MoveSpeed * Time.deltaTime;
			_TargetVelocity.z = _InputVector.z * _MoveSpeed * Time.deltaTime;
		}

		// 가속률을 연산합니다
		void CalculateOneSecAcceleration()
		{
			// 현재 Y 속력을 저장합니다.
			float currentVelocityY = _TargetVelocity.y;

			// 목표 속도를 저장합니다.
			Vector3 currentVelocity = isMovable ? _TargetVelocity : Vector3.zero;

			// 현재 속도를 저장합니다.
			_Velocity = characterController.velocity * Time.deltaTime;

			// 목표 속도와, 현재 속도에서 Y 축 값을 제외합니다.
			currentVelocity.y = _Velocity.y = 0.0f;

			// 가속률을 연산시킵니다.
			_Velocity = isDashable ? Vector3.MoveTowards(_Velocity, currentVelocity, _MoveSpeed * (_OneSecAcceleration * 0.01f * Time.deltaTime) * Time.deltaTime) + (_ImpulseVelocity * Time.deltaTime) :
				Vector3.MoveTowards(
				_Velocity, currentVelocity, _MoveSpeed * 
				(_OneSecAcceleration * 0.01f * Time.deltaTime) * Time.deltaTime) +
				(_ImpulseVelocity * Time.deltaTime);

			_ImpulseVelocity = Vector3.Lerp(_ImpulseVelocity, Vector3.zero, _OneSecAcceleration * 0.01f * Time.deltaTime);
			if (_ImpulseVelocity.magnitude < 0.01f) _ImpulseVelocity = Vector3.zero;

			// 연산된 속도에 Y 속력을 적용시킵니다.
			_Velocity.y = currentVelocityY;
		}


		// 입력 값을 연산합니다.
		CalculateInputVector();

		// 가속률을 연산합니다.
		CalculateOneSecAcceleration();
	}

	// 중력을 계산합니다.
	private void CalculateGravity()
	{
		// Y 축 속력에 중력을 적용시킵니다.
		_TargetVelocity.y += Physics.gravity.y * _GravityScale * 0.05f * Time.deltaTime;

		// Y 축 속력이 최대 Y 축 이동 속력을 초과하지 않도록 합니다.
		_TargetVelocity.y = Mathf.Clamp(_TargetVelocity.y, -_MaxYVelocity, _MaxYVelocity);

	}

	// 캐릭터를 실제로 이동시킵니다.
	private void MoveCharacter()
	{
		// 캐릭터를 이동시킵니다.
		characterController.Move(_Velocity);
	}

	// 이동하는 방향으로 캐릭터를 회전시킵니다.
	private void OrientRotationToMovement()
	{
		// _UseOrientRotationToMovement 속성을 사용하지 않는다면 실행하지 않습니다.
		if (!_UseOrientRotationToMovement) return;

		// 이동 불가능 상태라면 실행하지 않습니다.
		if (!isMovable) return;

		// 이동이 있을 경우에만 회전이 이루어져야 하므로 이동 입력이 없을 경우 실행하지 않습니다.
		if (_InputVector.magnitude <= characterController.minMoveDistance) return;



		// 현재 회전값을 얻습니다.
		float currentYawRotation = transform.eulerAngles.y;

		// 목표 회전값을 얻습니다.
		float tawrgetYawRotation = _TargetVelocity.ToAngle(castDirVector : true);

		// 다음 회전값을 얻습니다.
		float nextYawRotation = Mathf.MoveTowardsAngle(
			currentYawRotation, tawrgetYawRotation, _RotationYawSpeed * Time.deltaTime);

		// 적용시킬 오일러각을 저장합니다.
		Vector3 newEulerAngle = Vector3.up * nextYawRotation;

		transform.eulerAngles = newEulerAngle;
	}
	
	// 점프 입력을 처리합니다.
	private void JumpInput()
	{
		// 점프 가능 상태일 경우
		if (isJumpable)
		{
			// Y 이동 속력을 변경합니다.
			_TargetVelocity.y = _JumpVelocityY * Time.deltaTime;
		}
		// 점프키가 입력되었으므로, 점프 입력 상태로 설정합니다.
		_IsJumpInputFinished = false;
	}
	
	// 점프 입력을 끝냅니다.
	private void FinishJumpInput()
    {
		_IsJumpInputFinished = true;
    }

	// 대쉬키 입력 확인 메서드
	private void DashInput()
    {
		// 대쉬키가 눌려있지 않고 땅에 있다면
		if (!InputManager.GetAction("Dash", ActionEvent.Stay) && isGrounded)
			_MoveSpeed = 3.0f;

		// 대쉬키가 눌려있고 땅에 있다면
		else if (InputManager.GetAction("Dash", ActionEvent.Stay) && isGrounded)
		{
			_MoveSpeed = 6.0f;
			isDashable = true;
		}

		// 땅에있지 않고 대쉬키가 눌렸다면
		else if (!isGrounded && InputManager.GetAction("Dash", ActionEvent.Down))
		{
			isDashable = false;
			_MoveSpeed = 3.0f;
		}
	}

	private void UseWeaponInput(bool isTrue)
	{
		
		// 무기를 사용한다면
		if (isTrue)
        {
			useWeapon = true;
			// 무기 오브젝트 프리팹을 로드합니다.
			ResourceManager.Instance.LoadResource<GameObject>("SK_BattleAxe", "Character/FurryS1/Prefabs/Characters/SK_BattleAxe").GetComponent<PlayerCharacterMovement>();
        }
	}
	// 땅에 닿음 상태를 갱신합니다.
	private void UpdateGroundedState()
	{
		RaycastHit hit;
		Ray ray = new Ray(
			transform.position + characterController.center,
			Vector3.down);

		Ray newRay = new Ray(
			transform.position + characterController.center,
			transform.forward);

		// 땅에 닿아 있는지 확인합니다.
		isGrounded =
		// 캐릭터의 레이어가 아닌, 다른 오브젝트가 하단에 존재한다면
		Physics.SphereCast(ray,

			// 발사시키는 구의 반지름을 설정합니다.
			characterController.radius,

			out hit,

			(characterController.center.y) + (characterController.skinWidth * 2.0f) - (characterController.radius),

			~_IgnoreGroundLayers) &&

			// 캐릭터가 상승중이 아닐 경우
			_Velocity.y <= 0.0f;

#if UNITY_EDITOR
		Debug.DrawRay(
			ray.origin,
			ray.direction * (characterController.center.y + (characterController.skinWidth)),
			Color.red);

		Debug.DrawRay(
			newRay.origin,
			newRay.direction * (characterController.center.y + (characterController.skinWidth)),
			Color.green);
#endif
		
		if (isGrounded)
		{
			// 땅에 닿아있다면 Y 축 이동 속력을 0 으로 설정합니다.
			_TargetVelocity.y = 0.0f;
			
		}

		else if(!isGrounded)
        {
			
			// 땅에 닿아있지 않다면 낙하 속도 증가
			_TargetVelocity.y -= 0.0005f;
			if (_TargetVelocity.y <= -1.0f)
			{
				_TargetVelocity.y = -1.0f;
            }
        }

	}

	// 충격을 줍니다.
	public void AddImpulse(Vector3 impulseVelocity) =>
		_ImpulseVelocity = impulseVelocity;

   

}
