using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class DrawBoundingSphere
    {
        BoundingSphere mergingSphere;
        BoundingSphere[] boundingSpheres; 
        Model model3d;
        int meshCount;//quantidade de meshes no modelo

        /// <summary>
        /// Construtor da classe DrawBougingSphere, responçavel por desenhar a BoundingSpheres ao redor da mesh do tank
        /// </summary>
        /// <param name="model">modelo 3D a ser usado</param>
        /// <param name="modelPos">posição do modelo</param>
        /// <param name="scale">escala do modelo</param>
        public DrawBoundingSphere(Model model, Vector3 modelPos, float scale)
        {
            mergingSphere = new BoundingSphere();            
            model3d = model;
            meshCount = model3d.Meshes.Count;
            boundingSpheres = new BoundingSphere[meshCount];

            //associa uma boundingSphere a cada mesh do modelo
            int index = 0;
            foreach (ModelMesh mesh in model3d.Meshes)
            {
                boundingSpheres[index++] = mesh.BoundingSphere;/* multiplicar a escala do tank*/; //NÃO SERIA PRECISO 
            }

            //inclue as boundingSpheres à mergeSphere
            mergingSphere = boundingSpheres[0];

            if (model3d.Meshes.Count > 1)
            {
                index = 1;
                do
                {
                    //OPÇÃO OPTIMIZADA DA PASSAGEM DAS BOUNDING SPHERE
                    //mergingSphere = BoundingSphere.CreateMerged(mesh.BoundingSphere;, boundingSpheres[index]);
                    mergingSphere = BoundingSphere.CreateMerged(mergingSphere, boundingSpheres[index]);
                    index++;
                } while (index < model3d.Meshes.Count);
            }

            mergingSphere.Center = modelPos;
        }

        public void Draw(Matrix view, Matrix projection, ClsTank tankBoundingSphere)
        {
            //escala da bounding boundingsphere em relação ao tank
            Matrix scaleMatrix = Matrix.CreateScale(tankBoundingSphere.scale);
            Matrix translateMatrix = Matrix.CreateTranslation(mergingSphere.Center);

            Matrix worldMatrix = scaleMatrix * translateMatrix;

            foreach(ModelMesh mesh in model3d.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
       

    }
}
