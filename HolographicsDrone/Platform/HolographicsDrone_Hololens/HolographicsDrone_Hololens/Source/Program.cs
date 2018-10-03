using System;
using Windows.ApplicationModel.Core;

using Urho;
using Urho.Actions;
using Urho.SharpReality;
using Urho.Shapes;
using Urho.Resources;
using Urho.Physics;


using Windows.Media.Capture;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;



using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Urho;
using Urho.Gui;
using Urho.SharpReality;
using Urho.Resources;
using Urho.Shapes;
using Urho.Urho2D;

namespace HolographicsDrone
{
    ///--------------------------------------------------------------------
    using HolographicsDrone.World;
    using HolographicsDrone.GUI;
    using HolographicsDrone.Scenario;
    ///--------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Программа
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    internal class AProgram
    {
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// точка входа
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        [MTAThread]
        private static void Main()
        {
            var app = new UrhoAppViewSource<AApplication>(new ApplicationOptions("Data"));
            app.UrhoAppViewCreated += OnViewCreated;
            CoreApplication.Run(app);
        }


        static void OnViewCreated(UrhoAppView view)
        {
            view.WindowIsSet += View_WindowIsSet;
        }

        static void View_WindowIsSet(Windows.UI.Core.CoreWindow coreWindow)
        {
            // you can subscribe to CoreWindow events here
        }
    }
    ///--------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Программа
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AApplication : StereoApplication
    {
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///-------------------------------------------------------------------  
        public AApplication(ApplicationOptions opts) 
            : 
            base(opts)
        {

        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Запуск
        /// </summary>
        ///
        ///-------------------------------------------------------------------
        protected override void Start()
        {
            base.Start();
            EnableGestureTapped = true;


            //создаем hud интерфейс
            var hud = Scene.GetOrCreateComponent<AMainHUD>();
            //

            //создаем менеджер дрона
            var scenario = Scene.GetOrCreateComponent<AScenarioHololens>();


            //создаем мир
            var world = Scene.GetOrCreateComponent<AWorld>();
            world.signal_stopSpatialMapping     += StopSpatialMapping;
            world.signal_startSpatialMapping    += StartSpatialMapping;

            //прикрутим управляющие сигналы
            world.signal_startScanning  += hud.startScanning;
            world.signal_stopScanning   += hud.stopScanning;
            world.signal_stopScanning   += scenario.home;

            //

         
            //Scene.GetOrCreateComponent<DebugRenderer>();Engine.PostRenderUpdate += debugRender;



            //запуск, сначало стартуем сканирование

            //world.createEnvironment();
            //scenario.home();

            world.startScanning();
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// отирсовка 
        /// </summary>
        ///
        ///-------------------------------------------------------------------
        private void debugRender(PostRenderUpdateEventArgs obj)
        {
            Renderer.DrawDebugGeometry(false);

            var debugRendererComp = Scene.GetComponent<DebugRenderer>();
            var physicsComp = Scene.GetComponent<PhysicsWorld>();
            if (physicsComp != null)
            {
                physicsComp.DrawDebugGeometry(debugRendererComp, false);
            }
        }
        ///-------------------------------------------------------------------



        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка нажатия
        /// </summary>
        ///
        ///-------------------------------------------------------------------
        public override void OnGestureTapped()
        {
            /*
            if (lt)
            {
                mText.Value = "";
                mDrone.Position = LeftCamera.Node.WorldPosition;
                mDrone.GetComponent<RigidBody>().SetLinearVelocity(RightCamera.Node.Rotation * new Vector3(0f, 0.25f, 1f) * 9 );

                lt = false;
                return;
            }


            Ray cameraRay = RightCamera.GetScreenRay(0.5f, 0.5f);
            var result = Scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 100, DrawableFlags.Geometry, 0x70000000);

            if (result == null)
            {
                mText.Value = "";
                return;
            }

            if (result.Value.Node.Name == "Floor")
            {
                result.Value.Node.Remove();
                return;
            }


            if (result.Value.Node == mDrone || result.Value.Node.Parent == mDrone)
            {
                mText.Value = "OK";
                lt = true;
                pos = result.Value.Position;
                return;
            }
            mText.Value = "";

    */


            /*
            var world = Scene.GetOrCreateComponent<AWorld>();
            world.startScanning();
            */

            var scenario = Scene.GetComponent<AScenario>();
            if (scenario != null)
            {
                scenario.home();
            }
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка пространства
        /// </summary>
        ///
        ///-------------------------------------------------------------------
        public override void OnSurfaceAddedOrUpdated(SpatialMeshInfo surface, Model generatedModel)
        {
            var world = Scene.GetOrCreateComponent<AWorld>();
            world.surfaceAddedOrUpdated(surface, generatedModel);
        }
        ///-------------------------------------------------------------------









    }



}