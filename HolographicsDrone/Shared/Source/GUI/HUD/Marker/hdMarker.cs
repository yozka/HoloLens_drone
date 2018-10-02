using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;


namespace HolographicsDrone.GUI
{
    ///-------------------------------------------------------------------
    using Utils;
    ///-------------------------------------------------------------------






     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Интерфейс часть, показывает маркер искомого объекта
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class AMarker
            :
                Sprite
    {
        ///-------------------------------------------------------------------
        private Camera          mCamera = null;
        private AMarkerAnhor    mAnhor = null;
        ///-------------------------------------------------------------------






        ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public AMarker()
        {
            var cache = Application.Current.ResourceCache;
            Texture =  cache.GetTexture2D("Textures/marker_drone.png");


            Size = new IntVector2(48, 96);
            HotSpot = new IntVector2(48 / 2, 2);

            // Set random rotation in degrees and random scale
            Rotation = 0.0f;
            SetScale(1.0f);

            // Set random color and additive blending mode
            SetColor(Color.Red);
            BlendMode = BlendMode.Addalpha;
        }
        ///--------------------------------------------------------------------







         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// инциализация сцены
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public void update(float timeStep, Scene scene)
        {
            if (!findAnhor(scene))
            {
                return;
            }

            var pos = mAnhor.Node.WorldPosition;




            

            var posCenter = mCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            var posDest = pos - posCenter;
            posDest.Normalize();

            var posB = posCenter + posDest;

            var ptB = mCamera.WorldToScreenPoint(posB);
            Vector2 ptA = new Vector2(0.5f, 0.5f);


            Vector2 pt = ptB;


            //Vector2 pt = MathHelperExt.intersect(ptB);

            const float margin = 0.1f;
            pt.X = MathHelper.Clamp(pt.X, margin, 1.0f - margin);
            pt.Y = MathHelper.Clamp(pt.Y, margin, 1.0f - margin);



            var angle = Math.Atan2(pt.Y , pt.X) - Math.PI / 4;

            var sizeScreen = Root.Size;
            Position =  new IntVector2((int)(pt.X * sizeScreen.X), (int)(pt.Y * sizeScreen.Y));

            //Position = new IntVector2(sizeScreen.X / 2, sizeScreen.Y / 2);
            Rotation = (float)((angle * 180.0f) / Math.PI);

            //Console.WriteLine(ptB);
    
        }
        ///--------------------------------------------------------------------







         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// поиск всего
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private bool findAnhor(Scene scene)
        {
            if (mCamera == null || mCamera.IsDeleted)
            {
                mCamera = scene.GetComponent<Camera>(true);
                if (mCamera == null)
                {
                    return false;
                }
            }

            if (mAnhor == null || mAnhor.IsDeleted)
            {
                mAnhor = scene.GetComponent<AMarkerAnhor>(true);
                if (mAnhor == null)
                {
                    return false;
                }
            }


            return true;
        }
        ///--------------------------------------------------------------------










    }
}
