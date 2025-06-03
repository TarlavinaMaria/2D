using UnityEngine;

// Автоматически добавляет необходимые компоненты к объекту, если они отсутствуют
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerMover : MonoBehaviour
{
    // Параметры, настраиваемые в инспекторе Unity
    [SerializeField] private float _speed;        // Скорость перемещения персонажа
    [SerializeField] private float _jumpForce;   // Сила прыжка
    [SerializeField] private float _crouchSpeed = 2f; // Скорость в приседе

    // Константы для осей ввода
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    // Ссылки на компоненты
    private Rigidbody2D _rigidbody;     // Физическое тело персонажа
    private Animator _animator;         // Контроллер анимаций
    private SpriteRenderer _spriteRenderer; // Визуальное отображение персонажа
    private Vector2 _moveVector;        // Вектор движения

    // Имена параметров аниматора
    private string _floatMoveAnimation = "Speed";       // Параметр для движения
    private string _boolJumpAnimation = "IsJumping";    // Параметр для прыжка
    private string _triggerHurtAnimation = "Hurt";      // Параметр для получения урона
    private string _boolCrouchAnimation = "IsCrouching"; // Параметр для приседа

    private void Start()
    {
        // Инициализация компонентов при старте игры
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Вызов основных методов каждый кадр
        Move();         // Обработка движения
        HandleJump();   // Обработка прыжка
        HandleHurt();   // Обработка получения урона
        HandleCrouch(); // Обработка приседа
    }


    // Метод обработки движения персонажа
    private void Move()
    {
        // Получаем ввод с клавиатуры
        _moveVector.x = Input.GetAxis(Horizontal);
        _moveVector.y = Input.GetAxis(Vertical);

        // Применяем скорость к Rigidbody
        _rigidbody.velocity = new Vector2(
            _moveVector.x * _speed * Time.deltaTime,
            _moveVector.y * _speed * Time.deltaTime
        );

        // Управление анимацией движения
        if (_rigidbody.velocity.x != 0 || _rigidbody.velocity.y != 0)
        {
            // Если персонаж движется - включаем анимацию ходьбы
            _animator.SetFloat(_floatMoveAnimation, 1);
            Flipx(); // Поворот спрайта по направлению движения
        }
        else
        {
            // Если персонаж стоит - включаем анимацию покоя
            _animator.SetFloat(_floatMoveAnimation, 0);
        }
    }

    // Метод для поворота спрайта персонажа по направлению движения
    private void Flipx()
    {
        if (_moveVector.x > 0)
        {
            // Движение вправо - обычное отображение
            _spriteRenderer.flipX = false;
        }
        else if (_moveVector.x < 0)
        {
            // Движение влево - зеркальное отображение
            _spriteRenderer.flipX = true;
        }
        // Если движение отсутствует - сохраняем текущее состояние
    }

    // Метод обработки прыжка
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // При нажатии пробела - прыжок
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
            _animator.SetBool(_boolJumpAnimation, true); // Анимация прыжка
        }
        else if (_rigidbody.velocity.y == 0) // Проверка на приземление
        {
            // Когда персонаж на земле - выключаем анимацию прыжка
            _animator.SetBool(_boolJumpAnimation, false);
        }
    }

    // Метод обработки получения урона
    private void HandleHurt()
    {
        // При нажатии H включаем анимацию урона
        if (Input.GetKeyDown(KeyCode.H))
        {
            _animator.SetTrigger(_triggerHurtAnimation);
        }
    }
    // Метод обработки приседа
    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
            _animator.SetBool(_boolCrouchAnimation, true);
            _speed = _crouchSpeed; // Уменьшаем скорость
        }
        else
        {
            _animator.SetBool(_boolCrouchAnimation, false);
            _speed = 5f; // Возвращаем обычную скорость
        }
    }
}
