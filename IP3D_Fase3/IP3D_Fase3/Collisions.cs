using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Collisions
    {
        //calcular colisão entre tanks
        //calcular colisão tank bullet
        //calcular over kill
        ClsTank tank1, tank2;
        Bullet bullet;

        public Collisions()
        {

        }            

        /// <summary>
        /// Função que calcula se há colisão entre as bounding sphere 
        /// </summary>
        /// <param name="inicialPos"></param>
        /// <param name="finalPos"></param>
        /// <returns></returns>
        public bool Collision(Vector3 inicialPos, Vector3 v0, Vector3 acceleration, GameTime gameTime)
        {
            // final pos
            //x = xo + vo t + ½ a t2
            Vector3 finalPos;
            finalPos = inicialPos + v0 + (1 / 2) * acceleration * (float)Math.Pow(gameTime.ElapsedGameTime.TotalSeconds, 2);

            //distancia entre o centro da merging sphere e a posição inical da bala
            float a = Math.Abs((mergingSphere.Center.X - finalPos.X) + (mergingSphere.Center.Y - finalPos.Y) + (mergingSphere.Center.Z - finalPos.Z));
            //distancia entre o centro da merging sphere e a posição inical da bala
            float b = Math.Abs((mergingSphere.Center.X - inicialPos.X) + (mergingSphere.Center.Y - inicialPos.Y) + (mergingSphere.Center.Z - inicialPos.Z));
            //distancia entre a posição final e inicial
            float c = Math.Abs((inicialPos.X - finalPos.X) + (inicialPos.Y - finalPos.Y) + (inicialPos.Z - finalPos.Z));
            //semi perimetro 
            float sp = (a + b + c) / 2;

            //area do terreno
            float area = (float)Math.Sqrt(sp * (sp - a) * (sp - b) * (sp - c));

            //distancia entre o centro da bounding sphere e a trajetoria realisada pela bala
            float d = 2 * area / c;

            if (d < mergingSphere.Radius)
                return true;
            else
                return false;

        }
    }
}
