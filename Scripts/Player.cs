using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
//namespace Yarn.Unity.Example
//{
    public class Player : MonoBehaviour
    {

    public static Player playerInstance; 
        public float speed = 1.0f;

        [SerializeField]
        private Animator animator;

        public float interactionRadius = 2.0f;

        public Inventory inventory;

    //private void Awake()
    //{
    //    if (playerInstance == null)
    //        playerInstance = this;
    //    else if (playerInstance != this)
    //        Destroy(gameObject);
    //    DontDestroyOnLoad(gameObject);
    //}

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    /// Draw the range at which we'll start talking to people.
    void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;

            // Flatten the sphere into a disk, which looks nicer in 2D games
            Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, new Vector3(1, 1, 0));

            // Need to draw at position zero because we set position in the line above
            Gizmos.DrawWireSphere(Vector3.zero, interactionRadius);
        }

        void FixedUpdate()
        {
            // Remove all player control when we're in dialogue
            if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true)
            {
                animator.SetInteger("DirectionX", 0);
                animator.SetInteger("DirectionY", 0);
                return;
            }

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 currentVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;

            float newVelocityX = 0f;
            if (moveHorizontal < 0 && currentVelocity.x <= 0)
            {
                newVelocityX = -speed;
                animator.SetInteger("DirectionX", -1);
            }
            else if (moveHorizontal > 0 && currentVelocity.x >= 0)
            {
                newVelocityX = speed;
                animator.SetInteger("DirectionX", 1);
            }
            else
            {
                animator.SetInteger("DirectionX", 0);
            }

            float newVelocityY = 0f;
            if (moveVertical < 0 && currentVelocity.y <= 0)
            {
                newVelocityY = -speed;
                animator.SetInteger("DirectionY", -1);
            }
            else if (moveVertical > 0 && currentVelocity.y >= 0)
            {
                newVelocityY = speed;
                animator.SetInteger("DirectionY", 1);
            }
            else
            {
                animator.SetInteger("DirectionY", 0);
            }

            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(newVelocityX, newVelocityY);

            // Detect if we want to start a conversation
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckForNearbyNPC();
                if (FindObjectOfType<DialogueRunner>().isDialogueRunning == false)
                    CheckForNearbyItem();
            }
        }

        /// Find all DialogueParticipants
        /** Filter them to those that have a Yarn start node and are in range; 
         * then start a conversation with the first one
         */
        public void CheckForNearbyNPC()
        {
            var allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
            var target = allParticipants.Find(delegate (NPC p) {
                return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
                (p.transform.position - this.transform.position)// is in range?
                .magnitude <= interactionRadius;
            });
            if (target != null)
            {
                // Kick off the dialogue at this node.
                FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
            }
        }

        public void CheckForNearbyItem()
        {
            var allItems = new List<IInventoryItem>(FindObjectsOfType<IInventoryItem>());
            var target = allItems.Find(delegate (IInventoryItem i)
            {
                return (i.transform.position - this.transform.position)// is in range?
                .magnitude <= interactionRadius;
            });
            if (target != null)
            {
                inventory.AddItem(target);
            }
        }
    }
//}
