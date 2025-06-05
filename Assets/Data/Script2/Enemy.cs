using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        _health--;
        Debug.Log($"Враг получил урон! Осталось жизней: {_health}");

        _animator.SetTrigger("Hit"); // Анимация удара

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Враг уничтожен!");
        _animator.SetTrigger("Die"); // Анимация смерти
        Destroy(gameObject, 1f); // Удаление после короткой задержки
    }
}
