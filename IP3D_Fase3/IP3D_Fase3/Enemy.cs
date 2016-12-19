using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Enemy : ClsTank
    {
        Dustgen generator;

        public Enemy(GraphicsDevice device, ContentManager content, Map map, Vector2 newPlacement)
            : base(device, content, map, newPlacement)
        {
            generator = new Dustgen(device);
            directionX = (float)Math.Sin(yaw);
            directionZ = (float)Math.Cos(yaw);
        }

        public void Update(GameTime gameTime)
        {
            turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.O) ? 1 : 0) -
                              (Keyboard.GetState().IsKeyDown(Keys.U) ? 1 : 0)) * -.02f;

            cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Y) ? 1 : 0) -
                          (Keyboard.GetState().IsKeyDown(Keys.H) ? 1 : 0)) * -.02f;
            
            //andar com o tank
            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                position += new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                wheelRotation += .2f; //rotação das rodas
              
                //generator.Ciclo(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                position -= new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                wheelRotation -= .2f;
               // generator.Ciclo(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ);
            }

            //rodar o tanque
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                yaw += .05f;

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                yaw -= .05f;

            //BULLET 
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                if (bamB == null)
                    bamB = new Bullet(device, content, terrain, Position + new Vector3(0, 0.3f, 0), Target);
                bamB.BulletFlag = true;
            }

            if (bamB != null && bamB.BulletFlag)
                bamB.bulletUpdate(gameTime);
        }

        public void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
            generator.Draw(view, projection);
        }
    }
}
