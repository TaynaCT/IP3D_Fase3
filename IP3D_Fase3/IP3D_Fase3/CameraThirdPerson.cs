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

        public void Update(Matrix tankRotationMatrix, Vector3 tankPos, Point centre, float timePassed)
        {
            position = tankRotationMatrix.Translation + (tankRotationMatrix.Backward * new Vector3(-5, 0, -5));
            target = tankRotationMatrix.Translation;
                       
        }
                    
        public Matrix View()
        {
            view = Matrix.CreateLookAt(position, target, Vector3.Up);
            
            return view;
        }
        
    }
}
