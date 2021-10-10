using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAnimations
{
    void IdleAnimation();

    void Moving_X_YAnimation(float Moving_X, float Moving_Y);

    void AttackAnimation();

    void DamagedAnimation();

    void DeathAnimation();
}
