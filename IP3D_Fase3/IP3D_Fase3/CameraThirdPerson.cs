﻿using Microsoft.Xna.Framework;
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

            position = Vector3.Zero;

            direction = Vector3.Cross(Vector3.Forward, Vector3.Up);
            up = Vector3.Up;
                        
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                            (float)device.Viewport.Width / (float)device.Viewport.Height, 
                                                            nearPlane, 
                                                            farPlane);
            
        }


        public void Update(Point centre, float timePassed, Matrix tankRotation, Vector3 tankPos)
        {
            position = tankPos + new Vector3(0, .8f, 2f);
            target = tankPos;
        }


        public Matrix View()
        {
            view = Matrix.CreateLookAt(position, target, Vector3.Up);
            
            return view;
        }
        
    }
}
