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
    class Bullet
    {
        Model myBullet;
        Matrix worldMatrix;
        Matrix View;
        Matrix Projection;
        CameraSurfaceFollow camera;
        Map terrain;
        public Vector2 placement;
        Vector3 position, inicialPos;
        Vector3 speed, aceleration;
        public float scale;
        public float height;
        public float yaw = 0;
        bool bulletFlag;


        public Bullet(GraphicsDevice device, ContentManager content, CameraSurfaceFollow cam, Map map, Vector2 newPlacement)
        {
            placement = newPlacement;
            camera = cam;
            terrain = map;
            myBullet = content.Load<Model>("Cube");
            bulletFlag = false;

            //direction
            float aspectRatio = (float)device.Viewport.Width /
                                      device.Viewport.Height;

            height = terrain.SurfaceFollow(placement.X, placement.Y);
            position = new Vector3(placement.X, height, placement.Y);
            inicialPos = position;
            scale = 0.11f;

            worldMatrix = cam.world;
            View = cam.view;
            Projection = cam.projection;
        }
        
        public void bulletUpdate(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                bulletFlag = true;


            if (bulletFlag)
            {
                position += new Vector3(1, 1, 1) * 0.002f;
                Trajectory(gameTime);
            }       
        }

        /// <summary>
        /// Calcula a trajetoria o projetil a ser disparado
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns>Posição final do projetil</returns>
        public Vector3 Trajectory(GameTime gameTime)
        {
            float timePassed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //x = xo + vo t + ½ a t2 
            Vector3 finalPos = inicialPos + speed * timePassed + aceleration * 1 / 2 * (float)Math.Pow(timePassed, 2);

            return finalPos;
        }

        public void Draw()
        {
            foreach (ModelMesh mesh in myBullet.Meshes) // Desenha o modelo
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(scale)* Matrix.CreateTranslation(position);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
