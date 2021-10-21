using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSpeed : StateMachineBehaviour
{
    [SerializeField] AnimationCurve _speedCurve;
    float _initialSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // save the initial speed for reset later
       _initialSpeed = stateInfo.speedMultiplier;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // set de speed by parameter multiplaier baced on animation time 
        animator.SetFloat("SpeedMultiplaier",  _speedCurve.Evaluate(stateInfo.normalizedTime));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //reset speed multiplayer
        animator.SetFloat("SpeedMultiplaier",_initialSpeed);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
