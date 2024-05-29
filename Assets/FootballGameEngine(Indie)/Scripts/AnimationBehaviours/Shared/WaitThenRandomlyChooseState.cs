using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.AnimationBehaviours.Shared
{
    public class WaitThenRandomlyChooseState : StateMachineBehaviour
    {
        [SerializeField]
        float _waitTime = 3f;

        [SerializeField]
        int _numberOfStates = 0;

        float actualWaitTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            // randomly choose wait time
            actualWaitTime = _waitTime * Random.value;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            actualWaitTime -= Time.deltaTime;
            if(actualWaitTime <= 0f)
            {
                // choose a random state
                int chosenState = Random.Range(1, _numberOfStates + 1);

                // trigget state change
                animator.SetInteger("RandomState", chosenState);

            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            // reset stuff
            animator.SetInteger("RandomState", 0);
        }
    }
}
