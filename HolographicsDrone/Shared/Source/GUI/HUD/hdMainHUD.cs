using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;


namespace HolographicsDrone.GUI
{
    ///-------------------------------------------------------------------

    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Интерфейс 
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AMainHUD
            :
                Component
    {
        ///-------------------------------------------------------------------
        private readonly Text       mScanning   = null; //надпись о процессе сканирования
        private readonly AMarker    mMarker     = null;




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMainHUD()
        {


            ReceiveSceneUpdates = true;
            var cache = Application.ResourceCache;


            //надпись сканирования
            mScanning = new Text();
            mScanning.Visible = false;
            mScanning.Value = "[error]";
            mScanning.SetColor(Color.Red);
            mScanning.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 100);
            mScanning.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Center);
            Application.UI.Root.AddChild(mScanning);


      
            mMarker = new AMarker();
            Application.UI.Root.AddChild(mMarker);
       

            /*
            Sprite sprite = new Sprite();
            sprite.Texture = cache.GetTexture2D("Textures/marker_drone.png"); 

            // The UI root element is as big as the rendering window, set random position within it
            sprite.Position = new IntVector2(200, 200);

            // Set sprite size & hotspot in its center
            sprite.Size = new IntVector2(48, 96);
            sprite.HotSpot = new IntVector2(48 / 2, 2);

            // Set random rotation in degrees and random scale
            sprite.Rotation = 0.0f;
            sprite.SetScale(1.0f);

            // Set random color and additive blending mode
            sprite.SetColor(Color.Red);
            sprite.BlendMode = BlendMode.Addalpha;

            // Add as a child of the root UI element
            Application.UI.Root.AddChild(sprite);
            */
        }
        ///--------------------------------------------------------------------







         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// инциализация сцены
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public override void OnSceneSet(Scene scene)
        {


    
        }
        ///--------------------------------------------------------------------







         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Обновление
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        protected override void OnUpdate(float timeStep)
        {
            mMarker.update(timeStep, Scene);

        }
        ///--------------------------------------------------------------------






         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Начало сканирвоания
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void startScanning()
        {
            mScanning.Visible = true;
            mScanning.Value = ".:Scanning:.";
        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Конец сканирования
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void stopScanning()
        {
            mScanning.Visible = false;
            mScanning.Value = "";
        }
        ///--------------------------------------------------------------------







    }
}
