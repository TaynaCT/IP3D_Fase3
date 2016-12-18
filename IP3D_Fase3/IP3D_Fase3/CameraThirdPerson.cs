using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace IP3D_Fase3
{
    class CameraThirdPerson
    {
        public Matrix view; 
        public Matrix projection { get; protected set; }
        public Matrix world;

        public BasicEffect effect;
        Vector3 target;
        Vector3 direction;
        Vector3 position;
        Vector3 up;
                        
        float nearPlane = .1f;
        float farPlane = 700f;
        float speed = .2f;

        public CameraThirdPerson(GraphicsDevice device)
        {
            world = Matrix.Identity;

            effect = new BasicEffect(device);

            position = new Vector3(10, 10, 10);

            direction = Vector3.Cross(Vector3.Forward, Vector3.Up);
            up = Vector3.Up;
            
            
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                            (float)device.Viewport.Width / (float)device.Viewport.Height, 
                                                            nearPlane, 
                                                            farPlane);
            
        }


        public void Update(Point centre, float timePassed, Vector3 tankPos)
        {
            Vector3 lastPos = position;
            //rotação 
            Vector2 mouseRotation = (Mouse.GetState().Position - centre).ToVector2() * speed * timePassed;
            Vector3 cameraDirection = Vector3.Cross(direction, Vector3.Up);// O cross dos dois vetores devolve o vetor direção para a qual a camera deve se mover           

            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseRotation.X));
            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(cameraDirection, -mouseRotation.Y));
            
            target = direction + position;
            
            position = tankPos + new Vector3(1f, .5f, 1f);
            
            try
            {
                Mouse.SetPosition(centre.X, centre.Y); // mantem o o ponteiro dentro da janela do jogo
            }
            catch (Exception) { }
        }


        public Matrix View()
        {
            view = Matrix.CreateLookAt(position, target, Vector3.Up);
            
            return view;
        }
        
    }
}
