using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour, IEntityAnimations
{
    [SerializeField] Animator playerAnimator;

    private void Start()
    {

        playerAnimator = GetComponent<Animator>();

    }

    public void IdleAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.IDLE_ANIMATION_TAG);
    }


    public void RunningAnim()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.RUN_ANIMATION_TAG);

    }
    public void MovingAnimation(bool isMoving)
    {
        playerAnimator.SetBool(EntityAnimationTags.MOVING_ANIMATION_TAG, isMoving);
    }
    public void Moving_X_YAnimation(float Moving_X, float Moving_Y)
    {
        playerAnimator.SetFloat(EntityAnimationTags.MOVING_X_ANIMATION_TAG, Moving_X);
        playerAnimator.SetFloat(EntityAnimationTags.MOVING_Y_ANIMATION_TAG, Moving_Y);
    }

    public void JumpAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.JUMP_ANIMATION_TAG);
    }
    #region atacks
    #region  GearSword
    public void AttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK_ANIMATION_TAG);
    }
    public void AttackAnimation2()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK2_ANIMATION_TAG);
    }
    public void AttackAnimation3()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK3_ANIMATION_TAG);
    }
    
    public void SpecialAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.SPECIALATTACK_ANIMATION_TAG);
    }
    #endregion
    #region claymore
    public void ClaymoreAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK_CLAYMOREANIMATION_TAG);
    }
    public void ClaymoreAttackAnimation2()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK2_CLAYMOREANIMATION_TAG);
    }
    public void ClaymoreAttackAnimation3()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK3_CLAYMOREANIMATION_TAG);
    }    
    public void SpecialClaymoreAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.SPECIALCLAYMOREATTACK_ANIMATION_TAG);
    }
    #endregion
    #region gun
    public void GunAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK_GUNANIMATION_TAG);
    }
    public void GunAttackAnimation2()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK2_GUNANIMATION_TAG);
    }
    public void GunAttackAnimation3()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK3_GUNANIMATION_TAG);
    }    
    public void GunSpecialAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.SPECIALGUNATTACK_ANIMATION_TAG);
    }
    #endregion
    #region fist
    public void FistAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK_FISTANIMATION_TAG);
    }
    public void FistAttackAnimation2()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK2_FISTANIMATION_TAG);
    }
    public void FistAttackAnimation3()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK3_FISTANIMATION_TAG);
    }
    public void SpecialFistAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.SPECIALFISTATTACK_ANIMATION_TAG);
    }
    #endregion
    #region spear
    public void SpearAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK_SPEARANIMATION_TAG);
    }
    public void SpearAttackAnimation2()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK2_SPEARANIMATION_TAG);
    }
    public void SpearAttackAnimation3()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.ATTACK3_SPEARANIMATION_TAG);
    }
    public void SpecialSpearAttackAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.SPECIALSPEARATTACK_ANIMATION_TAG);
    }
    #endregion
    #endregion
    public void PunchAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.PUNCH_ANIMATION_TAG);
    }

    public void StunnedAnimation(bool isStunned)
    {
        playerAnimator.SetBool(EntityAnimationTags.STUN_ANIMATION_TAG, isStunned);
    }

    public void DamagedAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.DAMAGE_ANIMATION_TAG);
    }

    public void DeathAnimation()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.DEATH_ANIMATION_TAG);
    }
    public void Revive()
    {
        playerAnimator.SetTrigger(EntityAnimationTags.Revive_TAG);
    }
}
