using UnityEngine;

// ������������� ��������� ����������� ���������� � �������, ���� ��� �����������
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMover : MonoBehaviour
{
    // ���������, ������������� � ���������� Unity
    [SerializeField] private float _speed;        // �������� ����������� ���������
    [SerializeField] private float _jumpForce;   // ���� ������
    [SerializeField] private float _crouchSpeed = 2f; // �������� � �������

    // ��������� ��� ���� �����
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    // ������ �� ����������
    private Rigidbody2D _rigidbody;     // ���������� ���� ���������
    private Animator _animator;         // ���������� ��������
    private SpriteRenderer _spriteRenderer; // ���������� ����������� ���������
    private Vector2 _moveVector;        // ������ ��������

    // ����� ���������� ���������
    private string _floatMoveAnimation = "Speed";       // �������� ��� ��������
    private string _boolJumpAnimation = "IsJumping";    // �������� ��� ������
    private string _triggerHurtAnimation = "Hurt";      // �������� ��� ��������� �����
    private string _boolCrouchAnimation = "IsCrouching"; // �������� ��� �������

    private void Start()
    {
        // ������������� ����������� ��� ������ ����
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ����� �������� ������� ������ ����
        Move();         // ��������� ��������
        HandleJump();   // ��������� ������
        HandleHurt();   // ��������� ��������� �����
        HandleCrouch(); // ��������� �������
    }


    // ����� ��������� �������� ���������
    private void Move()
    {
        // �������� ���� � ����������
        _moveVector.x = Input.GetAxis(Horizontal);
        _moveVector.y = Input.GetAxis(Vertical);

        // ��������� �������� � Rigidbody
        _rigidbody.velocity = new Vector2(
            _moveVector.x * _speed * Time.deltaTime,
            _moveVector.y * _speed * Time.deltaTime
        );

        // ���������� ��������� ��������
        if (_rigidbody.velocity.x != 0 || _rigidbody.velocity.y != 0)
        {
            // ���� �������� �������� - �������� �������� ������
            _animator.SetFloat(_floatMoveAnimation, 1);
            Flipx(); // ������� ������� �� ����������� ��������
        }
        else
        {
            // ���� �������� ����� - �������� �������� �����
            _animator.SetFloat(_floatMoveAnimation, 0);
        }
    }

    // ����� ��� �������� ������� ��������� �� ����������� ��������
    private void Flipx()
    {
        if (_moveVector.x > 0)
        {
            // �������� ������ - ������� �����������
            _spriteRenderer.flipX = false;
        }
        else if (_moveVector.x < 0)
        {
            // �������� ����� - ���������� �����������
            _spriteRenderer.flipX = true;
        }
        // ���� �������� ����������� - ��������� ������� ���������
    }

    // ����� ��������� ������
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ��� ������� ������� - ������
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _animator.SetBool(_boolJumpAnimation, true); // �������� ������
        }
        else if (_rigidbody.velocity.y == 0) // �������� �� �����������
        {
            // ����� �������� �� ����� - ��������� �������� ������
            _animator.SetBool(_boolJumpAnimation, false);
        }
    }

    // ����� ��������� ��������� �����
    private void HandleHurt()
    {
        // ��� ������� H �������� �������� �����
        if (Input.GetKeyDown(KeyCode.H))
        {
            _animator.SetTrigger(_triggerHurtAnimation);
        }
    }
    // ����� ��������� �������
    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
            _animator.SetBool(_boolCrouchAnimation, true);
            _speed = _crouchSpeed; // ��������� ��������
        }
        else
        {
            _animator.SetBool(_boolCrouchAnimation, false);
            _speed = 5f; // ���������� ������� ��������
        }
    }
}
