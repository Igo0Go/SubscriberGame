using UnityEngine;
using UnityEngine.Events;

public abstract class AliveController : MonoBehaviour
{
    public int Health
    { 
        get
        {
            return _health;
        }
        set
        {
            if(_health > 0)
            {
                int newHealth = value;
                OnHealthChanged.Invoke(newHealth, _health);
                _health = newHealth;
                if (_health <= 0)
                {
                    _health = 0;
                    Dead();
                }
            }
        }
    }
    [SerializeField]
    [Min(0)]
    protected int _health = 100;

    [SerializeField]
    [Min(0)]
    private float deadDelay = 0;

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent<int, int> OnDead;

    public virtual void GetDamage(int damage)
    {
        Health -= damage;
    }

    protected virtual void Dead()
    {
        Destroy(gameObject, deadDelay);
    }
}
