using MagicLeap.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;
using MagicLeap.Core.StarterKit;

namespace MagicLeap
{
    /// <summary>
    /// This example demonstrates using the magic leap raycast functionality to calculate intersection with the physical space.
    /// It demonstrates casting rays from the users headpose, controller, and eyes position and orientation.
    ///
    /// This example uses a raycast visualizer which represents these intersections with the physical space.
    /// </summary>
    public class RaycastExample : MonoBehaviour
    {
        public enum RaycastMode
        {
            Controller,
            Head,
            Eyes
        }

        [SerializeField, Tooltip("The overview status text for the UI interface.")]
        private Text _overviewStatusText = null;

        [SerializeField, Tooltip("Raycast Visualizer.")]
        private MLRaycastVisualizer _raycastVisualizer = null;

        [SerializeField, Tooltip("Raycast from controller.")]
        private MLRaycastBehavior _raycastController = null;

        [Space, SerializeField, Tooltip("MLControllerConnectionHandlerBehavior reference.")]
        private MLControllerConnectionHandlerBehavior _controllerConnectionHandler = null;

        private RaycastMode _raycastMode = RaycastMode.Controller;
        private int _modeCount = System.Enum.GetNames(typeof(RaycastMode)).Length;

        private float _confidence = 0.0f;

        public TV[] screens;
        public TV currentScreenMesh;
        public Wall currentWall;
        public TV currentTV;
        public Transform controller;


        /// <summary>
        /// Validate all required components and sets event handlers.
        /// </summary>
        void Awake()
        {
            currentScreenMesh = screens[0];

            if (_overviewStatusText == null)
            {
                Debug.LogError("Error: RaycastExample._overviewStatusText is not set, disabling script.");
                enabled = false;
                return;
            }

            if (_raycastController == null)
            {
                Debug.LogError("Error: RaycastExample._raycastController is not set, disabling script.");
                enabled = false;
                return;
            }

            if (_controllerConnectionHandler == null)
            {
                Debug.LogError("Error: RaycastExample._controllerConnectionHandler not set, disabling script.");
                enabled = false;
                return;
            }

            // _raycastController.gameObject.SetActive(false);

            _raycastMode = RaycastMode.Controller;

            UpdateRaycastMode();

#if PLATFORM_LUMIN
            MLInput.OnControllerButtonDown += OnButtonDown;
#endif
        }

        void Update()
        {
            MoveScreen();
        }
        void MoveScreen()
        {
            if (currentWall == null)
                return;

            float rotationAngle = currentWall.televisionRotationAngle;

            if (currentWall.xorZWallFixedCoor == XorZWallFixedCoor.X)
            {
                hitPos.x = currentWall.transform.position.x;
            }
            else
            {
                hitPos.z = currentWall.transform.position.z;
            }

            currentTV.transform.position = hitPos;
            currentTV.transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }


        /// <summary>
        /// Cleans up the component.
        /// </summary>
        void OnDestroy()
        {
#if PLATFORM_LUMIN
            MLInput.OnControllerButtonDown -= OnButtonDown;
#endif
        }

        /// <summary>
        /// Updates type of raycast and enables correct cursor.
        /// </summary>
        private void UpdateRaycastMode()
        {

            EnableRaycast(_raycastController);

        }

        /// <summary>
        /// Enables raycast behavior and raycast visualizer
        /// </summary>
        private void EnableRaycast(MLRaycastBehavior raycast)
        {
            raycast.gameObject.SetActive(true);
            _raycastVisualizer.raycast = raycast;

#if PLATFORM_LUMIN
            _raycastVisualizer.raycast.OnRaycastResult += _raycastVisualizer.OnRaycastHit;
            _raycastVisualizer.raycast.OnRaycastResult += OnRaycastHit;
#endif
        }
        private void OnButtonDown(byte controllerId, MLInput.Controller.Button button)
        {
            if (button == MLInput.Controller.Button.Bumper)
            {
                if(locatedTV!=null) 
                {
                    Destroy(locatedTV.gameObject);
                    locatedTV = null;
                }
                else
                 LocateScreen();

            }

            if (button == MLInput.Controller.Button.HomeTap)
            {
                ChangeScreen();
            }

        }
        private void LocateScreen()
        {

            currentTV.Located = true;
            var mesh = Instantiate(currentScreenMesh);
            currentTV = mesh;
        }

        private int scrIndx = 0;
        private void ChangeScreen()
        {
            if (currentTV != null)
                Destroy(currentTV.gameObject);

            if (scrIndx == 0)
            {
                scrIndx = 1;
                currentScreenMesh = screens[1];
            }
            else if (scrIndx == 1)
            {
                scrIndx = 2;
                currentScreenMesh = screens[2];
            }
            else if (scrIndx == 2)
            {
                scrIndx = 3;
                currentTV = null;
                return; 
            }
            else
            {


                scrIndx = 0;
                currentScreenMesh = screens[0];
            }


            var mesh = Instantiate(currentScreenMesh);
            currentTV = mesh;
        }

        private Vector3 hitPos;
        private TV locatedTV;

        public void OnRaycastHit(MLRaycast.ResultState state, MLRaycastBehavior.Mode mode, Ray ray, RaycastHit result, float confidence)
        {
            locatedTV = null;
            hitPos = result.point;
            
            if (scrIndx != 3)
            {
              
                if (result.collider.TryGetComponent<Wall>(out var wall))
                {

                    

                    if (currentWall != null)
                        if (currentWall.ID == wall.ID)
                            return;

                    if (currentTV == null)
                     currentTV = Instantiate(currentScreenMesh);

                    if (currentWall != null)
                        currentWall.ForgetWall();

                    currentWall = wall;
                }

            }
            else
            {
                if (result.collider.TryGetComponent<TV>(out var tv))
                {
                    if (tv.Located)
                        locatedTV = tv;


                }



            }
        }
    }
}
