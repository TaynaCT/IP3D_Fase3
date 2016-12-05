using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FASE3
{
    class Camera
    {
        public Matrix view; 
        public Matrix projection { get; protected set; }
        public Matrix world;

        public BasicEffect effect;
        Vector3 target;
        Vector3 direction;
        public Vector3 position;
        Vector3 up;
        private float speed;

        VertexPositionNormalTexture[] vertex;

        float nearPlane = .1f;
        float farPlane = 700f;

        public Camera(GraphicsDevice device)
        {
            world = Matrix.Identity;

            effect = new BasicEffect(device);
                       
            position = new Vector3(10, 10, 10);
            direction = Vector3.Cross(Vector3.Forward, Vector3.Up);
            up = Vector3.Up;
            speed = .2f;
            
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                            (float)device.Viewport.Width / (float)device.Viewport.Height, 
                                                            nearPlane, 
                                                            farPlane);
            
        }

        public void Update(Point centre, float timePassed, float surfaceFollow)
        {
            Vector3 lastPos = position;
            //rotação 
            Vector2 mouseRotation = (Mouse.GetState().Position - centre).ToVector2() * speed * timePassed;
            Vector3 cameraDirection = Vector3.Cross(direction, Vector3.Up);// O cross dos dois vetores devolve o vetor direção para a qual a camera deve se mover           

            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, -mouseRotation.X));
            direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(cameraDirection, -mouseRotation.Y));

            position.Y = surfaceFollow + .5f;
            
            target = direction + position;

            //movimentação da camera 
            /*
             * 8 - frente
             * 5 - para trás
             * 4 - esquerda
             * 6 - direita
             */
            //é acrescentado a posição de acordo com a verificação de qual tecla foi acionada.
            //se for a tecla 6 a soma fica: position += cameraRight * timePassed;
            //se for 4: position += -cameraRight * timePassed;
            //o mesmo para verificação das teclas 8 e 5, so que nesse caso é somado a variavel direction
            if (surfaceFollow == -1)
                position = lastPos;

            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad6) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad4) ? 1 : 0)) * cameraDirection * .02f;            

            position += ((Keyboard.GetState().IsKeyDown(Keys.NumPad8) ? 1 : 0) -
                         (Keyboard.GetState().IsKeyDown(Keys.NumPad5) ? 1 : 0)) * direction * .02f;

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
