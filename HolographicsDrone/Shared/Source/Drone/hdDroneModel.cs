using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Shapes;

namespace HolographicsDrone.Drone
{
    ///-------------------------------------------------------------------




     ///-------------------------------------------------------------------
    ///
    /// <summary>
    /// Компанент, визуализация дрона, его модель
    /// </summary>
    ///
    ///--------------------------------------------------------------------
    public class ADroneModel
            :
                StaticModelGroup
    {
        ///-------------------------------------------------------------------

        ///-------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public ADroneModel()
        {
           // createModel();

        }
        ///--------------------------------------------------------------------




         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// инциализация
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        public override void OnAttachedToNode(Node node)
        {
            base.OnAttachedToNode(node);
            createModel();
        }
        ///--------------------------------------------------------------------





         ///-------------------------------------------------------------------
        ///
        /// <summary>
        /// система создание модели
        /// </summary>
        ///
        ///--------------------------------------------------------------------
        private void createModel()
        {
            var cache = Application.ResourceCache;

            var nodeBody = Node.CreateChild();
            var modelBody = nodeBody.CreateComponent<StaticModel>();
            modelBody.Model = cache.GetModel("Models/Mutant.mdl");
            

            AddInstanceNode(nodeBody);


            //Model = cache.GetModel("Models/Mutant.mdl");
            //SetMaterial(cache.GetMaterial("robot.xml"));
        }
        ///--------------------------------------------------------------------





    }
}
