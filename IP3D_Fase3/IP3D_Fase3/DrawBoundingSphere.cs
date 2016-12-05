using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FASE3
{
    class DrawBoundingSphere
    {
        BoundingSphere mergingSphere;
        BoundingSphere[] boundingSpheres; 
        Model model3d;

        public DrawBoundingSphere(Model model, Vector3 modelPos)
        {
            mergingSphere = new BoundingSphere();            
            model3d = model;
            int meshCount = model3d.Meshes.Count;
            boundingSpheres = new BoundingSphere[meshCount];

            //associa uma boundingSphere a cada mesh do modelo
            int index = 0;
            foreach (ModelMesh mesh in model3d.Meshes)
            {
                boundingSpheres[index++] = mesh.BoundingSphere/* mutiplicar a escala do tank*/; //NÃO SERIA PRECISO 
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

        public void Draw()
        {
            
        }

    }
}
