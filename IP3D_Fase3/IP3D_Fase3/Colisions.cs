using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Colisions
    {


        public bool IsColliding(Model tank1, Matrix worldMatrix1, Model tank2, Matrix worldMatrix2)
        {
            for (int meshIndex1 = 0; meshIndex1 < tank1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = tank1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(worldMatrix1);
                sphere1.Radius *= 0.001f;
                //Matrix scaleMatrix = Matrix.CreateScale(sphere1.Radius);                

                for (int meshIndex2 = 0; meshIndex2 < tank2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = tank2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(worldMatrix2);
                    sphere2.Radius *= 0.001f;
                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
    }
}
