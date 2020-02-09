using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Yarn.Unity.Example
{
    public class NPCWander : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        private Transform thisTransform;
        private Vector3 startPos;
        private Rigidbody2D rb;

        // Movement speed of the object
        public float moveSpeed = 0.2f;

        // Min and max time for taking a decision 
        public Vector2 decisionTime = new Vector2(1, 4);
        private float decisionTimeCount = 0;
        private bool colliding = false;

        // Possible directions to move in
        public Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.zero, Vector3.zero };
        private int currentMoveDirection;

        // Start is called before the first frame update
        void Start()
        {
            // Cache the transform for quicker access
            thisTransform = this.transform;
            startPos = thisTransform.position;
            rb = gameObject.GetComponent<Rigidbody2D>();
            // Set random time delay for taking a decision 
            decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

            ChooseMoveDirection();
        }
        

        //Prevent the NPC continuing to try and wander if for example the player is standing in the way. 
        private void OnCollisionEnter2D(Collision2D collision)
        {
            colliding = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            colliding = false;
        }

        // Update is called once per frame
        void Update()
        {

            // Wait a random number of seconds 
            // Walk left 
            // Wait
            // Walk right
            


            // Remove all player control when we're in dialogue
            if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true || colliding)
            {
                return;
            }

            switch(currentMoveDirection)
            {
                case 0: // right
                    if (thisTransform.position.x <= startPos.x + 2.0f)
                    {
                        animator.SetInteger("DirectionX", 1);
                        animator.SetInteger("DirectionY", 0);
                        thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;
                    }
                    else
                        decisionTimeCount = 0;
                    break;
                case 1: // left
                    if (thisTransform.position.x >= startPos.x - 2.0f)
                    {
                        animator.SetInteger("DirectionX", -1);
                        animator.SetInteger("DirectionY", 0);
                        thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;
                    }
                    else
                        decisionTimeCount = 0;
                    break;
                default:
                    break;

            }

            // Move in chosen direction at the set speed
            //if (thisTransform.position.x >= startPos.x - 2.0f && thisTransform.position.x <= startPos.x + 2.0f)
            //    thisTransform.position += moveDirections[currentMoveDirection] * Time.deltaTime * moveSpeed;

            if (decisionTimeCount > 0)
                decisionTimeCount -= Time.deltaTime;
            else
            {
                animator.SetInteger("DirectionX", 0);
                animator.SetInteger("DirectionY", 1);
                // Choose a random time delay for taking a decision 
                decisionTimeCount = Random.Range(decisionTime.x, decisionTime.y);

                // Choose a movement direction or stay in place 
                ChooseMoveDirection();
            }
        }

        

        void ChooseMoveDirection()
        {
            currentMoveDirection = Mathf.FloorToInt(Random.Range(0, moveDirections.Length));
        }
    }
}
