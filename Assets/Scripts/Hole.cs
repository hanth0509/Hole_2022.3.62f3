using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField] public float speed ;
    private Vector2 moveInput;
    private Move controls;
    void Start()
    {
        
    }

    private void Awake()
    {
        controls = new Move();
        controls.Hole.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Hole.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    private void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * speed * Time.fixedDeltaTime;
        transform.Translate(movement, Space.World);
    }
    private void OnEnable()
    {
        controls.Hole.Enable();
    }
    private void OnDisable()
    {
        controls.Hole.Disable();
    }
        // --- UP SIZE ---
    public void ScaleUp()
    {
        // Tăng 20%
        transform.localScale *= 1.2f;
        Debug.Log("Hole đã to lên!");
    }
}
