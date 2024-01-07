using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    [Tooltip("Number of times the cursor can move per second")]
    [SerializeField] float moveRate;

    private PlayerInput input;

    private Vector2 moveVector;
    private Coroutine moveRoutine;

    private void Start()
    {
        input = GetComponent<PlayerInput>();

        SubscribeInputs();
    }

    private IEnumerator Move()
    {
        WaitForSeconds waitDelay = new WaitForSeconds(1f/moveRate);
        while (enabled)
        {
            if (Region.instance.inBound(transform.position + Vector3.right * moveVector.x)){
                transform.position = transform.position + Vector3.right * moveVector.x;
            }
            if (Region.instance.inBound(transform.position + Vector3.up * moveVector.y))
            {
                transform.position = transform.position + Vector3.up * moveVector.y;
            }
            yield return waitDelay;
        }
        yield return null;
    }

    private void SubscribeInputs()
    {
        if (input == null) { return; }
        input.actions["Move Cursor"].started += MoveCursorStarted;
        input.actions["Move Cursor"].performed += MoveCursorPerformed;
        input.actions["Move Cursor"].canceled += MoveCursorCanceled;
        input.actions["Spawn Unit"].performed += SpawnUnit;
    }

    private void UnsubscribeInputs()
    {
        if (input == null) { return; }
        input.actions["Move Cursor"].started -= MoveCursorStarted;
        input.actions["Move Cursor"].performed -= MoveCursorPerformed;
        input.actions["Move Cursor"].canceled -= MoveCursorPerformed;
        input.actions["Spawn Unit"].performed += SpawnUnit;
    }

    private void OnEnable()
    {
        SubscribeInputs();
    }

    private void OnDisable()
    {
        UnsubscribeInputs();
    }

    public void SpawnUnit(InputAction.CallbackContext context)
    {
        Region.instance.SpawnUnit(Vector3Int.RoundToInt(transform.position), "new dood", ElementType.AIR, 1, 0);
    }

    private void ReadMoveVector(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        moveVector.x = Mathf.Round(moveVector.x);
        moveVector.y = Mathf.Round(moveVector.y);
    }

    public void MoveCursorStarted(InputAction.CallbackContext context)
    {
        ReadMoveVector(context);
        moveRoutine = StartCoroutine(Move());
        Debug.Log("Started");
    }

    public void MoveCursorPerformed(InputAction.CallbackContext context)
    {
        ReadMoveVector(context);
        Debug.Log(moveVector);
    }

    public void MoveCursorCanceled(InputAction.CallbackContext context)
    {
        moveVector = Vector3.zero;
        StopCoroutine(moveRoutine);
        moveRoutine = null;
        Debug.Log("Stopped");
    }
}
