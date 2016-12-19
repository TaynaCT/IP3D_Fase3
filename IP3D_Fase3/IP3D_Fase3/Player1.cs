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
    class Player1 : ClsTank
    {
        Dustgen generator;
        Bullet bamB;

        public Player1(GraphicsDevice device, ContentManager content, Map map, Vector2 newPlacement)
            : base(device, content, map, newPlacement)
        {
            generator = new Dustgen(device);
            
            directionX = (float)Math.Sin(yaw);
            directionZ = (float)Math.Cos(yaw);
        }

        public void Update(GameTime gameTime)
        {

            turretRotation += ((Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : 0) -
                              (Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : 0)) * -.02f;

            cannonRotation += ((Keyboard.GetState().IsKeyDown(Keys.Up) ? 1 : 0) -
                          (Keyboard.GetState().IsKeyDown(Keys.Down) ? 1 : 0)) * -.02f;

            //andar com o tank
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position += new Vector3(directionX, 0, directionZ) * .02f;
                wheelRotation += .2f; //rotação das rodas
                position.Y = terrain.SurfaceFollow(position.X, position.Z);

                generator.Ciclo(directionX, directionZ, position.X, terrain.SurfaceFollow(position.X, position.Z), position.Z);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position -= new Vector3(directionX, terrain.SurfaceFollow(position.X, position.Z), directionZ) * .02f;
                wheelRotation -= .2f;

            }

            //rodar o tanque
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                yaw += .05f;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                yaw -= .05f;

            directionX = (float)Math.Sin(yaw);
            directionZ = (float)Math.Cos(yaw);

            //BULLET 
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (bamB == null)
                    bamB = new Bullet(device, content, terrain, Position + new Vector3(0, 0.3f, 0), Target);
                bamB.BulletFlag = true;
            }

            if (bamB != null && bamB.BulletFlag)
                bamB.bulletUpdate(gameTime);            

            if (bamB != null && bamB.BulletFlag)
                if ((bamB.Position.X < 0 || bamB.Position.X > terrain.MapLimit) || (bamB.Position.Z < 0 || bamB.Position.Z > terrain.MapLimit))
                    bamB = null;

            base.Update(gameTime);
        }
        public void Draw(Matrix view, Matrix projection)
        {
            base.Draw(view, projection);
            generator.Draw(view, projection);
            if (bamB != null && bamB.BulletFlag)
                bamB.Draw(view, projection);
        }

        public Bullet GetSetBullet
        {
            get { return bamB; }
            set { bamB = value; }
        }
    }
}

