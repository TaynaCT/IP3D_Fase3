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
        
        public static bool IsColliding(Model tank1, Matrix worldMatrix1, Model tank2, Matrix worldMatrix2)
        {
            for (int i = 0; i < tank1.Meshes.Count; i++)
            {
                BoundingSphere sphere1 = tank1.Meshes[i].BoundingSphere;
                sphere1.Transform(worldMatrix1);

                for (int j = 0; j < tank2.Meshes.Count; j++)
                {
                    BoundingSphere sphere2 = tank2.Meshes[j].BoundingSphere;
                    sphere2.Transform(worldMatrix2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }

            return false;
        }

        public static bool Test()
        {
            BoundingSphere s1 = new BoundingSphere(Vector3.Zero, 1), s2 = new BoundingSphere(5 * Vector3.One, 1);

            return s1.Intersects(s2);
        }
    }
}
