using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Urho;
using Urho.SharpReality;
using Urho.Physics;
using Urho.Shapes;

namespace HolographicsDrone.World
{
    ///-------------------------------------------------------------------
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Окружающий мир
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AWorld
        :
         Component
    {
        ///-------------------------------------------------------------------
        private const int   cTimeScanning       = 1000 * 20; //общее время сканирования


        private Node        mEnvironmentNode    = null;
        private bool        mScanning           = false; //процесс сканирования
        private Material    mMaterialScene      = null; //материал для сцены
        private TimeSpan    mTimeScan           = TimeSpan.Zero; //время сканирования
        ///-------------------------------------------------------------------




        ///-------------------------------------------------------------------
        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AWorld()
        {
            ReceiveSceneUpdates = true;

          //  mMaterialScene = Application.ResourceCache.GetMaterial("Materials/scanning_grid.xml");
            //mMaterialScene.SetTechnique(0, CoreAssets.Techniques.Diff, 0, 0);

            /*
            var text = Application.ResourceCache.GetTexture2D("Textures/scanning_grid.dds");

            mMaterialScene = new Material();
            mMaterialScene.SetTexture(TextureUnit.Diffuse, text);
            mMaterialScene.SetTechnique(0, CoreAssets.Techniques.Diff, 0, 0);

            */
            mMaterialScene = Material.FromColor(Color.Red * 0.6f);

        }
        ///--------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возвратим размерность мира для сканера
        /// в метрах
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public Vector3 extents
        {
            get
            {
                return new Vector3(20, 20, 20);
            }
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// возвратим размерность мира
        /// в метрах
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public Vector3 sizeBox
        {
            get
            {
                return new Vector3(22, 12, 22);
            }
        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// сигнал остановки и запуска сканера
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public event Action                 signal_startScanning;
        public event Action                 signal_stopScanning;

        public event Action                 signal_stopSpatialMapping;
        public event Action_startScanning   signal_startSpatialMapping;
        public delegate Task<bool> Action_startScanning(Vector3 extents, int trianglesPerCubicMeter, Color color, bool onlyAdd, bool convertToLeftHanded);
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// обработка и создание поверхности мира
        /// если возвратили true, то все простарнство отсканированно
        /// нужно остановить процесс
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void surfaceAddedOrUpdated(SpatialMeshInfo surface, Model generatedModel)
        {
            if (mEnvironmentNode == null)
            {
                createEnvironment();
            }

            bool isNew = false;
            StaticModel staticModel = null;
            Node node = mEnvironmentNode.GetChild(surface.SurfaceId, false);
            if (node != null)
            {
                isNew = false;
                staticModel = node.GetComponent<StaticModel>();
            }
            else
            {
                isNew = true;
                node = mEnvironmentNode.CreateChild(surface.SurfaceId);
                staticModel = node.CreateComponent<StaticModel>();
            }

            node.Position = surface.BoundsCenter;
            //node.Rotation = surface.BoundsRotation;

            var oldModel = staticModel.Model;
            staticModel.Model = generatedModel;

            if (isNew)
            {
                staticModel.SetMaterial(mMaterialScene);
                var rigidBody = node.CreateComponent<RigidBody>();
                rigidBody.RollingFriction = 0.5f;
                rigidBody.Friction = 0.5f;
                var collisionShape = node.CreateComponent<CollisionShape>();
                collisionShape.SetTriangleMesh(generatedModel, 0, Vector3.One, Vector3.Zero, Quaternion.Identity);
            }
            else
            {
                //Update Collision shape
                var physics = Scene.GetComponent<PhysicsWorld>();
                if (oldModel != null)
                {
                    physics.RemoveCachedGeometry(oldModel);
                }
                var collisionShape = node.GetOrCreateComponent<CollisionShape>();
                collisionShape.SetTriangleMesh(generatedModel, 0, Vector3.One, Vector3.Zero, Quaternion.Identity);
            }

            staticModel.Enabled = true;

            //првоерка, есть ли свободное пространство
            var check = checkScannedAll();
            if (check)
            {
                //сканированние завершено
                stopScanning();
            }
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// запуск сканирования
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public async void startScanning()
        {
            if (mScanning)
            {
                return;
            }


            mTimeScan = TimeSpan.Zero;
            mScanning = true;

            if (mEnvironmentNode == null)
            {
                createEnvironment();
            }

            signal_startScanning?.Invoke();
            await signal_startSpatialMapping?.Invoke(extents, 500, default(Color), true, false);
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// остановка сканирования
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void stopScanning()
        {
            signal_stopSpatialMapping?.Invoke();
            signal_stopScanning?.Invoke();

            if (!mScanning)
            {
                return;
            }
            mScanning = false;

            //скрываем модели которые отрендерились
            hideModels();
        }
        ///--------------------------------------------------------------------





        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обработка сканирвоания
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            if (!mScanning)
            {
                return;
            }
            mTimeScan += TimeSpan.FromSeconds(timeStep);
            if (mTimeScan.TotalMilliseconds > cTimeScanning)
            {
                //время сканирвоания вышло
                stopScanning();
            }



        }
        ///--------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// проверим, все отсканировали или нет
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public bool checkScannedAll()
        {

            return false;
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// создание ограничегоющего пространства
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void createEnvironment()
        {
            mEnvironmentNode = Node.CreateChild("EnvironmentNode");


            //создадим стенки
            var size = sizeBox;
            const float depth = 1.0f; //размерность толщина стенки
            const float top = 2.0f;
            createHate("hateDown",  new Vector3(0, -top, 0),  new Vector3(size.X, depth, size.Z), Color.Gray);
            createHate("hateUp",    new Vector3(0, sizeBox.Y -top, 0),   new Vector3(size.X, depth, size.Z), Color.Blue);

            createHate("hateLeft",  new Vector3(-sizeBox.X / 2, sizeBox.Y / 2 -top, 0), new Vector3(depth, size.Y, size.Z), Color.Green);
            createHate("hateRight", new Vector3( sizeBox.X / 2, sizeBox.Y / 2 - top, 0), new Vector3(depth, size.Y, size.Z), Color.Yellow);

            createHate("hateFront", new Vector3(0, sizeBox.Y / 2 - top,  sizeBox.Z / 2), new Vector3(size.X, size.Y, depth), Color.Yellow);
            createHate("hateRear",  new Vector3(0, sizeBox.Y / 2 - top, -sizeBox.Z / 2), new Vector3(size.X, size.Y, depth), Color.Magenta);
        }
        ///--------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// создание ограничегоющего пространства
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void createHate(string hateName, Vector3 pos, Vector3 size, Color color)
        {
            var hate = mEnvironmentNode.CreateChild(hateName);
            hate.Position = pos;
            hate.Scale = size;

            /*
            var box = hate.CreateComponent<Box>();
            box.Color = color * 0.5f;
            */

            var rigidBody = hate.CreateComponent<RigidBody>();
            CollisionShape shape = hate.CreateComponent<CollisionShape>();
            shape.SetBox(Vector3.One, Vector3.Zero, Quaternion.Identity);

        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// удаляем всю геометрию
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void clear()
        {
            if (mEnvironmentNode == null)
            {
                return;
            }


            var physics = Scene.GetComponent<PhysicsWorld>();

            while (true)
            {
                var model = mEnvironmentNode.GetComponent<StaticModel>(true);
                if (model == null)
                {
                    break;
                }
                physics.RemoveCachedGeometry(model.Model);
                model.Remove();
            }

            Node.RemoveChild(mEnvironmentNode);
            mEnvironmentNode.Dispose();
            mEnvironmentNode = null;
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// удаляем все модели
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void hideModels()
        {
            if (mEnvironmentNode == null)
            {
                return;
            }

            foreach (var node in mEnvironmentNode.GetChildrenWithComponent<StaticModel>())
            {
                var model = node.GetComponent<StaticModel>();
                model.Enabled = false;
            }
        }
        ///--------------------------------------------------------------------



    }
}
