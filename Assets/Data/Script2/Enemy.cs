using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    private Animator _animator;

    private void Awake()
    {
        // Получаем компонент анимации врага
        _animator = GetComponent<Animator>();
    }

    // Метод получения урона
    public void TakeDamage()
    {
        _health--; // Уменьшаем здоровье врага
        Debug.Log($"Враг получил урон! Осталось жизней: {_health}");

        _animator.SetTrigger("Hit"); // Запускаем анимацию удара

        // Если враг больше не имеет здоровья, вызываем метод смерти
        if (_health <= 0)
        {
            Die();
        }
    }
    // Метод смерти врага
    private void Die()
    {
        Debug.Log("Враг уничтожен!");
        _animator.SetTrigger("Die"); // Анимация смерти
        Destroy(gameObject, 1f); // Удаление после короткой задержки
    }
}
