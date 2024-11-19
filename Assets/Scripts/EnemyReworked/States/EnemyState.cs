using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to https://www.youtube.com/watch?v=cnpJtheBLLY for Basic State Machine Logic
public abstract class EnemyState : MonoBehaviour
{
    // EnemyState defines the enemy's current behaviour and the conditions it will change under - return this; if state is unchanged
    public abstract EnemyState RunCurrentState();

}
