//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.1.1
//     from Assets/Scripts/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
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
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""546d6aff-f746-4e56-a159-7a5b0644ad94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
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
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // 2Dmovement
        m__2Dmovement = asset.FindActionMap("2Dmovement", throwIfNotFound: true);
        m__2Dmovement_Move = m__2Dmovement.FindAction("Move", throwIfNotFound: true);
        m__2Dmovement_Jump = m__2Dmovement.FindAction("Jump", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // 2Dmovement
    private readonly InputActionMap m__2Dmovement;
    private I_2DmovementActions m__2DmovementActionsCallbackInterface;
    private readonly InputAction m__2Dmovement_Move;
    private readonly InputAction m__2Dmovement_Jump;
    public struct _2DmovementActions
    {
        private @PlayerControls m_Wrapper;
        public _2DmovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m__2Dmovement_Move;
        public InputAction @Jump => m_Wrapper.m__2Dmovement_Jump;
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
            }
        }
    }
    public _2DmovementActions @_2Dmovement => new _2DmovementActions(this);
    public interface I_2DmovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
