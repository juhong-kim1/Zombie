using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int hashMove = Animator.StringToHash("Move");

    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private PlayerInput input;
    private Animator animator;

    private Rigidbody rb;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //회전

        var rotation = Quaternion.Euler(0, input.Roate * rotateSpeed * Time.deltaTime, 0);

        rb.MoveRotation(rb.rotation * rotation);

        //이동

        var distance = input.Move * moveSpeed * Time.deltaTime; 

        rb.MovePosition(transform.position + distance * transform.forward);

        animator.SetFloat(hashMove, input.Move);
    }

}
