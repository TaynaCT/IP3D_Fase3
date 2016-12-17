using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Collisions
    {
        //Classe que faz a verificação de colisões       
        /// <summary>
        /// Função que recebe dois modelos 3d e suas matizes de tranformação, depois percorre cada mesh do dos modelos e verifica se há colisões.
        /// </summary>
        /// <param name="tank1">modelo 1</param>
        /// <param name="worldMatrix1">array com as martizes de cada bone</param>
        /// <param name="tank2">modelo 2</param>
        /// <param name="worldMatrix2">array com as matrizes de cada bone do modelo 2</param>
        /// <returns></returns>
        public static bool TankCollision(Model tank1, Matrix[] worldMatrix1, Model tank2, Matrix[] worldMatrix2)
        {
            for (int i = 0; i < tank1.Meshes.Count; i++)
            {
                BoundingSphere sphere1 = tank1.Meshes[i].BoundingSphere;
                sphere1 = sphere1.Transform(worldMatrix1[i]);
                
                for (int j = 0; j < tank2.Meshes.Count; j++)
                {
                    BoundingSphere sphere2 = tank2.Meshes[j].BoundingSphere;
                    sphere2 = sphere2.Transform(worldMatrix2[j]);

                    if (sphere1.Intersects(sphere2))
                        return true; //há colisões
                }
            }

            return false;//não há colisões
        }


        public static bool BulletCollision(Model tank, Matrix[] worldMatrix1, Model bullet, Matrix worldMatrix)
        {
            for (int i = 0; i < bullet.Meshes.Count; i++)
            {
                BoundingSphere bulletSphere = bullet.Meshes[i].BoundingSphere;
                bulletSphere = bulletSphere.Transform(worldMatrix);

                for (int j = 0; j < tank.Meshes.Count; j++)
                {
                    BoundingSphere tankSphere = tank.Meshes[j].BoundingSphere;
                    tankSphere = tankSphere.Transform(worldMatrix1[j]);

                    if (bulletSphere.Intersects(tankSphere))
                        return true; //há colisões
                }
            }

            return false;//não há colisões
        }
                
    }
}
