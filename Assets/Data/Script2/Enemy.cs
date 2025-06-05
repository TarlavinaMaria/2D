using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 3;

    public void TakeDamage()
    {
        _health--;

        Debug.Log($"Враг получил урон! Осталось жизней: {_health}");

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Враг уничтожен!");
        Destroy(gameObject);
    }
}
