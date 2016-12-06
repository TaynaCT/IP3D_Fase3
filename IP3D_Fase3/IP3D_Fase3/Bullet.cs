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
        Camera camera;
        Map terrain;
        public Vector2 placement;
        Vector3 position;
        public float scale;
        public float height;
        public float yaw = 0;
        bool bulletFlag;
        public Bullet(GraphicsDevice device, ContentManager content, Camera cam, Map map, Vector2 newPlacement)
        {
            placement = newPlacement;
            camera = cam;
            terrain = map;
            myBullet = content.Load<Model>("Cube");

            //direction

            float aspectRatio = (float)device.Viewport.Width /
                                      device.Viewport.Height;

            height = terrain.SurfaceFollow(placement.X, placement.Y);
            position = new Vector3(placement.X, height, placement.Y);
            scale = 100f;

            worldMatrix = cam.world;
            View = cam.view;
            Projection = cam.projection;
        }


        public void bulletUpdate()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                bulletFlag = true;

            
            if (bulletFlag)
                position += new Vector3(3, 3, 3);
            
        }


        public void Draw()
        {
            myBullet.Root.Transform = Matrix.CreateScale(scale)* Matrix.CreateTranslation(position);
            foreach (ModelMesh mesh in myBullet.Meshes) // Desenha o modelo
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
