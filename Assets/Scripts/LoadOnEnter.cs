using UnityEngine;
using UnityEngine.UI;

public class LoadOnEnter : StateMachineBehaviour
{
    public GameObject Ground;
    public GameObject Walls;
    public GameObject Player;
    public GameObject PickUps;

    private GameObject _ground;
    private GameObject _walls;
    private GameObject _player;
    private GameObject _pickUps;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If the particle system already exists then exit the function.
        //if (_ground != null)
        //    return;

        // Otherwise instantiate the particles and set up references to their components..
        //_ground = Instantiate(Ground);

        //_player = Instantiate(Player);

        //_walls = Instantiate(Walls);
        //_pickUps = Instantiate(PickUps);

        //SceneManager.LoadScene(0);

        Board.Instance().ResizeBoard(Options.Instance.CellRatio, true);
        //Board.Mono.StartCoroutine(Board.Instance().ResizeBoard(Options.Instance.CellRatio, true));

        //Board.Instance().ResizeBoard(Options.Instance.CellRatio, true);

        //Board.Instance().ResetBoard();

        GameObject.FindGameObjectWithTag("SmileyButton").GetComponent<Image>().sprite = PrefabHelper.Instance.HappySmiley;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
