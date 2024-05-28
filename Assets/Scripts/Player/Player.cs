using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickUpSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    private bool isWalking;
    private bool canMove;


    private float playerRadius = 0.7f;
    private float playerHight = 2f;
    private float moveDistance;
    private float interactDictance = 2f;

    private Vector3 lastInteractDir;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private InputSystem inputSystem;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;



    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance= this; 
        moveDistance = moveSpeed * Time.deltaTime;
    }



    private void Start()
    {
        inputSystem.OnInteractAction += InputSystem_OnInteractAction;
        inputSystem.OnInteractAlternateAction += InputSystem_OnInteractAlternateAction;
    }

    private void InputSystem_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void InputSystem_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }




    private void Update()
    {
        HandleMoveMent();
        HandleInteractions();
    }




    private void HandleInteractions()
    {
        Vector2 inputVector2 = inputSystem.GetMoveMentVector2Normalize();

        Vector3 moveDir = new Vector3(inputVector2.x, 0f, inputVector2.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        else
        {
            lastInteractDir = transform.forward;
        }

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDictance, counterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                if (baseCounter != selectedCounter)//转换了所选counter
                {
                  SetSelectedCounter(baseCounter);
                }
            }
            else//没有clearcounter
            {
                SetSelectedCounter(null);
            }
        }
        else//如果没有找到counterlayer为null
        {
           SetSelectedCounter(null);
        }
    }


    private void HandleMoveMent()
    {
        Vector2 inputVector2 = inputSystem.GetMoveMentVector2Normalize();

        Vector3 moveDir = new Vector3(inputVector2.x, 0f, inputVector2.y);
        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDir, moveDistance);

        if (!canMove)//如果不能运动
        {
            //判断X轴方向是否可以
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x!= 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirX, moveDistance);

            if (canMove)//如果可以
            {
                moveDir = moveDirX;
            }
            else//不可以则判断Z
            {
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = moveDir.z!= 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //不能从任何方向移动
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * Time.deltaTime * moveSpeed;

        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
    }


    public bool IsWalking()
    {
        if (inputSystem.GetMoveMentVector2Normalize() != Vector2.zero)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTranform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject= kitchenObject;

        if(kitchenObject != null)
        {
            OnPickUpSomething?.Invoke(this,EventArgs.Empty); 
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return this.kitchenObject != null;
    }
}
