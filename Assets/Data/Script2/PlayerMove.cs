using UnityEngine;

// Требуемые компоненты: Спрайт, Анимация, Физика, Коллайдер
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMove : MonoBehaviour
{
    // Константы для анимационных параметров
    private const string Horizontal = nameof(Horizontal);
    private const string Speed = nameof(Speed);
    private const string Jump = nameof(Jump);
    private const string SpeedUpDown = nameof(SpeedUpDown);
    private const string AttackAnim = nameof(Attack);

    // Настройки движения и прыжка
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private KeyCode _jumpKeyCode;

    // Настройки атаки
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private KeyCode _attackKey = KeyCode.F;
    [SerializeField] private LayerMask _enemyLayer;

    //Система частиц
    [SerializeField] private ParticleSystem _hit;


    // Компоненты
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    // Переменные состояния
    private float _derection;
    private bool _isGround = false;


    private void Awake()
    {
        // Получаем компоненты при старте
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Управление движением персонажа
        _derection = Input.GetAxis(Horizontal) * _speed * Time.deltaTime;
        _rigidbody2D.velocity = new Vector2(_derection, _rigidbody2D.velocity.y);

        // Проверяем нажатие клавиши прыжка
        if (Input.GetKeyDown(_jumpKeyCode) && _isGround == true)
        {
            _isGround = false;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
            _animator.SetBool(Jump, true);
            _animator.SetFloat(SpeedUpDown, _rigidbody2D.velocity.y);

            Flipx();// Поворот спрайта по направлению движения
        }
        else if (_rigidbody2D.velocity.y < 0)
        {
            _animator.SetFloat(SpeedUpDown, _rigidbody2D.velocity.y);
        }

        // Управление поворотом персонажа и анимациями движения
        if (_rigidbody2D.velocity.x > 0)
        {
            transform.Translate(_speed * Time.deltaTime, 0, 0);
            _animator.SetFloat(Speed, 1); // Анимация движение, скорость

            Flipx(); // Поворот спрайта по направлению движения
        }
        else if (_rigidbody2D.velocity.x < 0)
        {
            transform.Translate(_speed * Time.deltaTime * -1, 0, 0);
            _animator.SetFloat(Speed, 1);// Анимация движение, скорость

            Flipx(); // Поворот спрайта по направлению движения
        }
        else
        {
            _animator.SetFloat(Speed, 0);// Анимация покоя, скорость
        }

        // Проверка атаки
        if (Input.GetKeyDown(_attackKey))
        {
            Attack();
        }
    }
    // Метод для поворота спрайта персонажа по направлению движения
    private void Flipx()
    {
        if (_derection > 0)
        {
            // Движение вправо - обычное отображение
            _spriteRenderer.flipX = false;
        }
        else if (_derection < 0)
        {
            // Движение влево - зеркальное отображение
            _spriteRenderer.flipX = true;
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        }

    }
    // Метод для прыжка (колизия), работает при столкновении колайдером, но только 1 раз
    private void OnCollisionEnter2D()
    {
        _isGround = true;
        _animator.SetBool(Jump, false);
        _animator.SetFloat(Speed, 1);
        _animator.SetFloat(SpeedUpDown, 0);
    }
    // Метод атаки персонажа
    private void Attack()
    {
        _animator.SetTrigger(AttackAnim); // Запускаем анимацию удара
        Debug.Log("Удар!");
        // Определяем направление удара
        Vector2 attackDirection = _spriteRenderer.flipX ? Vector2.left : Vector2.right;
        // Создаем луч для проверки попадания удара
        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, _attackRange, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            // Если объект — враг, наносим ему урон
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();

                // Активируем систему частиц при успешном попадании
                if (_hit != null)
                {
                    // Позиционируем частицы в точке удара
                    _hit.transform.position = hit.point;

                    // Настраиваем направление частиц в зависимости от направления удара
                    var main = _hit.main;
                    if (attackDirection == Vector2.left)
                    {
                        main.startRotation = Mathf.PI; // 180 градусов для удара влево
                    }
                    else
                    {
                        main.startRotation = 0f; // 0 градусов для удара вправо
                    }

                    _hit.Play(); // Запускаем эффект
                }
            }
        }
    }
}