using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDT.KitchenGame
{
    public class Player : MonoBehaviour, IKitchenObjectParent 
    {
        public static Player Instance { get; private set; }

        public event EventHandler OnPickedUpSomething;

        public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
        public class OnSelectedCounterChangedEventArgs : EventArgs
        {
            public BaseCounter selectedCounter;
        }

        [SerializeField] private float moveSpeed = 7f;
        [SerializeField] private InputController inputController;
        [SerializeField] private LayerMask countersLayerMask;
        [SerializeField] private Transform kitchenObjectHoldPoint;

        private bool isWalking;
        private Vector3 lastInteractDirection;
        private BaseCounter selectedCounter;
        private KitchenObject kitchenObject;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There is more than 1 player instance.");
            }
            Instance = this;
        }

        private void Start()
        {
            inputController.OnInteractAction += InputController_OnInteractAction;
            inputController.OnInteractAlternateAction += InputController_OnInteractAlternateAction;
        }

        private void InputController_OnInteractAlternateAction(object sender, EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying()) return;

            if (selectedCounter != null)
                selectedCounter.InteractAlternate(this);
        }

        private void InputController_OnInteractAction(object sender, System.EventArgs e)
        {
            if (!GameManager.Instance.IsGamePlaying()) return;

            if (selectedCounter != null)
                selectedCounter.Interact(this);
        }

        private void Update()
        {
            HandleMovement();
            HandleInteractions();
        }

        private void HandleInteractions()
        {
            Vector2 inputVector = inputController.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

            if (moveDirection != Vector3.zero)
            {
                lastInteractDirection = moveDirection;
            }

            float interactDistance = 2f;
            if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, countersLayerMask))
            {
                if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
                {
                    // Has clear counter
                    if (baseCounter != selectedCounter)
                    {
                        SetSelectedCounter(baseCounter);
                    }
                } else
                {
                    SetSelectedCounter(null);
                }
            } else
            {
                SetSelectedCounter(null);
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

        private void HandleMovement()
        {
            Vector2 inputVector = inputController.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;
            float playerRadius = 0.7f;
            float playerHeight = 2f;
            bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

            if (!canMove)
            {
                // Attempt move on X
                Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
                canMove = (moveDirection.x < -0.5f || moveDirection.x > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, moveDistance);

                if (canMove)
                {
                    // Can move on X
                    moveDirection = moveDirectionX;
                }
                else
                {
                    // Attempt to move on Z
                    Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                    canMove = (moveDirection.z < -0.5f || moveDirection.z > +0.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, moveDistance);

                    if (canMove)
                    {
                        // Can move on Z
                        moveDirection = moveDirectionZ;
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDirection * moveDistance;
            }

            isWalking = moveDirection != Vector3.zero;
            float rotateSpeed = 20f;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
        }

        public bool IsWalking()
        {
            return isWalking;
        }

        public Transform GetKitchenObjectFollowTransform()
        {
            return kitchenObjectHoldPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null)
            {
                OnPickedUpSomething?.Invoke(this, EventArgs.Empty);
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
            return kitchenObject != null;
        }
    }

}