//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/PlayerInput.inputactions
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

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""e132b3aa-7114-4dfb-b669-02f722a4961a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5796b5e2-84b3-442c-88cc-54200a512e4a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""f4204d1a-8f05-4825-be3b-2cc6a10163be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraRotationX"",
                    ""type"": ""Value"",
                    ""id"": ""fef3c1ca-8ce2-418b-b517-3b3a5deded20"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CameraRotationY"",
                    ""type"": ""Value"",
                    ""id"": ""0877bdbe-b16d-47f2-a936-d63618975b43"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Value"",
                    ""id"": ""b9fdc42c-18df-4d42-bfcd-d38af6e841e2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Squat"",
                    ""type"": ""Button"",
                    ""id"": ""0ac75913-194d-478a-8bef-21de348bd2d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""de0a033e-6105-4db7-baba-7de7c187e984"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectWeapon"",
                    ""type"": ""Value"",
                    ""id"": ""14247622-225a-497b-8782-db6fdbece0e8"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""SelectWeaponByScroll"",
                    ""type"": ""Value"",
                    ""id"": ""83dc711e-1672-43f7-9dba-b6c15ce2a559"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DropWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""4fea43a8-f92d-4732-a483-14137f5764cb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""551f9b6c-2264-4da2-8a2a-eea9be9ef7eb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""a6badfc2-f891-45ee-9b7b-26ae90d27bec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""62d14c5f-2889-4fbc-b470-6a0e87d8a03e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WSAD"",
                    ""id"": ""55e0bfd9-0ebb-4358-9967-2c29601af604"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""246f0529-151e-4a1c-893d-7e1fa8dc617c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""77a343b1-65f8-405b-85eb-d621a470b7e3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2059b2b1-abe9-4f9c-860b-a397f39a4563"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cfb226c6-fac0-403f-8f5d-97b1f7b4be43"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow"",
                    ""id"": ""6c3bba70-3371-409a-a40c-6c12ed938b65"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""60cefc27-12ad-4712-b2b0-3d157122e327"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c5e364c2-8805-494d-9c03-ebe6f1c2a44c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7acf32ff-dcd3-4a8f-b4eb-0a5d988268d3"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""091d9a8f-8d04-4223-8845-b4131e1bbcf4"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""35ebbe15-2a72-41b6-a244-a8d384d7231b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93aa9cce-fca2-4b95-896b-fce33e648384"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotationX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49f790c3-0c28-44ef-8223-0de3d4720f96"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotationY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""908febb5-1029-4d8c-b8e8-e8bfcc4001ba"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de467b0a-3596-485b-af40-0e290f07e29b"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Squat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1ecd0b0d-04c8-40c4-9b93-1e681ec6c5f4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3ba484f-053b-4193-bc03-9e0b407da191"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""SelectWeaponByScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71edfac5-f2f0-490e-9655-9c285aeecda8"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""SelectWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d4b473f-7187-4c1e-a934-c8444cbaae67"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""SelectWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""560f42a8-fe7d-4c39-8001-b50ece5811b0"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""SelectWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d3c644f-f295-4e72-9260-94eba4f3cec6"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""SelectWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0df8cfcc-2fa9-48b0-b0d6-9d6e0a934d85"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""DropWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c8b36b0-5661-44c5-a402-a14d6f80ca59"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ebf819a-0e55-4ab5-b416-b740558f3bfc"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""efc467ad-e454-4bc1-97b6-66173a9f3f8e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""GameScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""GameScheme"",
            ""bindingGroup"": ""GameScheme"",
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
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_CameraRotationX = m_Player.FindAction("CameraRotationX", throwIfNotFound: true);
        m_Player_CameraRotationY = m_Player.FindAction("CameraRotationY", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_Squat = m_Player.FindAction("Squat", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_SelectWeapon = m_Player.FindAction("SelectWeapon", throwIfNotFound: true);
        m_Player_SelectWeaponByScroll = m_Player.FindAction("SelectWeaponByScroll", throwIfNotFound: true);
        m_Player_DropWeapon = m_Player.FindAction("DropWeapon", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_Reload = m_Player.FindAction("Reload", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_CameraRotationX;
    private readonly InputAction m_Player_CameraRotationY;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_Squat;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_SelectWeapon;
    private readonly InputAction m_Player_SelectWeaponByScroll;
    private readonly InputAction m_Player_DropWeapon;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_Reload;
    private readonly InputAction m_Player_Aim;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @CameraRotationX => m_Wrapper.m_Player_CameraRotationX;
        public InputAction @CameraRotationY => m_Wrapper.m_Player_CameraRotationY;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @Squat => m_Wrapper.m_Player_Squat;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @SelectWeapon => m_Wrapper.m_Player_SelectWeapon;
        public InputAction @SelectWeaponByScroll => m_Wrapper.m_Player_SelectWeaponByScroll;
        public InputAction @DropWeapon => m_Wrapper.m_Player_DropWeapon;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @Reload => m_Wrapper.m_Player_Reload;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @CameraRotationX.started += instance.OnCameraRotationX;
            @CameraRotationX.performed += instance.OnCameraRotationX;
            @CameraRotationX.canceled += instance.OnCameraRotationX;
            @CameraRotationY.started += instance.OnCameraRotationY;
            @CameraRotationY.performed += instance.OnCameraRotationY;
            @CameraRotationY.canceled += instance.OnCameraRotationY;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Squat.started += instance.OnSquat;
            @Squat.performed += instance.OnSquat;
            @Squat.canceled += instance.OnSquat;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @SelectWeapon.started += instance.OnSelectWeapon;
            @SelectWeapon.performed += instance.OnSelectWeapon;
            @SelectWeapon.canceled += instance.OnSelectWeapon;
            @SelectWeaponByScroll.started += instance.OnSelectWeaponByScroll;
            @SelectWeaponByScroll.performed += instance.OnSelectWeaponByScroll;
            @SelectWeaponByScroll.canceled += instance.OnSelectWeaponByScroll;
            @DropWeapon.started += instance.OnDropWeapon;
            @DropWeapon.performed += instance.OnDropWeapon;
            @DropWeapon.canceled += instance.OnDropWeapon;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @CameraRotationX.started -= instance.OnCameraRotationX;
            @CameraRotationX.performed -= instance.OnCameraRotationX;
            @CameraRotationX.canceled -= instance.OnCameraRotationX;
            @CameraRotationY.started -= instance.OnCameraRotationY;
            @CameraRotationY.performed -= instance.OnCameraRotationY;
            @CameraRotationY.canceled -= instance.OnCameraRotationY;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Squat.started -= instance.OnSquat;
            @Squat.performed -= instance.OnSquat;
            @Squat.canceled -= instance.OnSquat;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @SelectWeapon.started -= instance.OnSelectWeapon;
            @SelectWeapon.performed -= instance.OnSelectWeapon;
            @SelectWeapon.canceled -= instance.OnSelectWeapon;
            @SelectWeaponByScroll.started -= instance.OnSelectWeaponByScroll;
            @SelectWeaponByScroll.performed -= instance.OnSelectWeaponByScroll;
            @SelectWeaponByScroll.canceled -= instance.OnSelectWeaponByScroll;
            @DropWeapon.started -= instance.OnDropWeapon;
            @DropWeapon.performed -= instance.OnDropWeapon;
            @DropWeapon.canceled -= instance.OnDropWeapon;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_GameSchemeSchemeIndex = -1;
    public InputControlScheme GameSchemeScheme
    {
        get
        {
            if (m_GameSchemeSchemeIndex == -1) m_GameSchemeSchemeIndex = asset.FindControlSchemeIndex("GameScheme");
            return asset.controlSchemes[m_GameSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCameraRotationX(InputAction.CallbackContext context);
        void OnCameraRotationY(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnSquat(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSelectWeapon(InputAction.CallbackContext context);
        void OnSelectWeaponByScroll(InputAction.CallbackContext context);
        void OnDropWeapon(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
    }
}
