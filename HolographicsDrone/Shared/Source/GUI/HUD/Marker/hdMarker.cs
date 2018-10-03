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

            var pos = mAnhor.Node.WorldPosition; //позиция объекта
            var posCenter = mCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)); //позиция точки наблюдения

            var posDest = pos - posCenter; //вектор направления
            var length = posDest.Length;
            posDest.Normalize();
            var posAnhor = posCenter + posDest;
            var ptAnhor = mCamera.WorldToScreenPoint(posAnhor); //положение метки
            

            var ptA     = mCamera.WorldToScreenPoint(pos);
            var ptAS    = mCamera.WorldToScreenPoint(pos + mAnhor.Node.Scale);
            float sizeObj = (ptAS - ptA).Length;

            float scale = 1.0f;
            float marginDirect = 0.1f;
            if (    sizeObj > 0.1f &&
                    ptA.X > marginDirect && ptA.Y > marginDirect && ptA.X < 1 - marginDirect && ptA.Y < 1 - marginDirect
                )
            {
                //скрыть
                scale = 0.0f;
            }


      




            const float margin = 0.01f;
            ptAnhor.X = MathHelper.Clamp(ptAnhor.X, margin, 1.0f - margin);
            ptAnhor.Y = MathHelper.Clamp(ptAnhor.Y, margin, 1.0f - margin);

            var ptDest = ptAnhor - new Vector2(0.5f, 0.5f);

            var angle = Math.Atan2(ptDest.Y , ptDest.X) + Math.PI / 2;

     
            var sizeScreen = Root.Size;
            Position =  new IntVector2((int)(ptAnhor.X * sizeScreen.X), (int)(ptAnhor.Y * sizeScreen.Y));


            Rotation = (float)((angle * 180.0f) / Math.PI);

            SetScale(scale);
    
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
