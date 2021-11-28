// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""2Dmovement"",
            ""id"": ""98e18305-f42d-4e75-af1c-b80926ea1b61"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6537adf7-71b8-4a9b-8ec5-d7a3a723b34b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""546d6aff-f746-4e56-a159-7a5b0644ad94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7ecb5c47-9ac9-4fb9-897d-68e40619aca9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""ad9d9a45-8f54-4747-8527-5c1f3cf638a9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""cec6b5be-517b-4010-bc2e-9fe2b607346b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""50de15e3-eef9-480e-8101-73abdd201ec9"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f9b275fb-40b6-4e83-a561-4e7729bcc41d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f8f61cd-cba8-425c-bb89-0edad67bf6fa"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""5f7f2e0f-68b3-40f8-b182-bfe22b8b3c1a"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""25a66202-1789-4a1a-86fa-f54646620451"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""e1aa4141-d4a2-41bc-89ef-35d6f38642b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Flashlight_Toggle"",
                    ""type"": ""Button"",
                    ""id"": ""ed027a7c-d347-468d-9f89-32ad8831c590"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""654efd59-f6b4-4e0d-82d3-60295d130ea9"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eeb74882-6695-4490-8220-419d4e53f9a8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bb82b0cc-c472-4721-be9d-96f52f4b3432"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight_Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // 2Dmovement
        m__2Dmovement = asset.FindActionMap("2Dmovement", throwIfNotFound: true);
        m__2Dmovement_Move = m__2Dmovement.FindAction("Move", throwIfNotFound: true);
        m__2Dmovement_Jump = m__2Dmovement.FindAction("Jump", throwIfNotFound: true);
        m__2Dmovement_Aim = m__2Dmovement.FindAction("Aim", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
        m_UI_Interact = m_UI.FindAction("Interact", throwIfNotFound: true);
        m_UI_Flashlight_Toggle = m_UI.FindAction("Flashlight_Toggle", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // 2Dmovement
    private readonly InputActionMap m__2Dmovement;
    private I_2DmovementActions m__2DmovementActionsCallbackInterface;
    private readonly InputAction m__2Dmovement_Move;
    private readonly InputAction m__2Dmovement_Jump;
    private readonly InputAction m__2Dmovement_Aim;
    public struct _2DmovementActions
    {
        private @PlayerControls m_Wrapper;
        public _2DmovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m__2Dmovement_Move;
        public InputAction @Jump => m_Wrapper.m__2Dmovement_Jump;
        public InputAction @Aim => m_Wrapper.m__2Dmovement_Aim;
        public InputActionMap Get() { return m_Wrapper.m__2Dmovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(_2DmovementActions set) { return set.Get(); }
        public void SetCallbacks(I_2DmovementActions instance)
        {
            if (m_Wrapper.m__2DmovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnJump;
                @Aim.started -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m__2DmovementActionsCallbackInterface.OnAim;
            }
            m_Wrapper.m__2DmovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
            }
        }
    }
    public _2DmovementActions @_2Dmovement => new _2DmovementActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Pause;
    private readonly InputAction m_UI_Interact;
    private readonly InputAction m_UI_Flashlight_Toggle;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputAction @Interact => m_Wrapper.m_UI_Interact;
        public InputAction @Flashlight_Toggle => m_Wrapper.m_UI_Flashlight_Toggle;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Interact.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInteract;
                @Flashlight_Toggle.started -= m_Wrapper.m_UIActionsCallbackInterface.OnFlashlight_Toggle;
                @Flashlight_Toggle.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnFlashlight_Toggle;
                @Flashlight_Toggle.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnFlashlight_Toggle;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Flashlight_Toggle.started += instance.OnFlashlight_Toggle;
                @Flashlight_Toggle.performed += instance.OnFlashlight_Toggle;
                @Flashlight_Toggle.canceled += instance.OnFlashlight_Toggle;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    public interface I_2DmovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnPause(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnFlashlight_Toggle(InputAction.CallbackContext context);
    }
}
