using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class MenuCamera
    {
        Matrix projection;
        Matrix view;
        Vector3 position;

        CameraFree freeCam;
        CameraSurfaceFollow sfCam;
        CameraThirdPerson tpCam;

        public MenuCamera(GraphicsDevice device, Vector3 pos)
        {
            position = pos;
            Select(device);
        }

        public void Select(GraphicsDevice device)
        {
            //F1 - seleciona camera 3° pessoa
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                tpCam = new CameraThirdPerson(device, position);
                projection = tpCam.projection;
                view = tpCam.view;
            }
            //F2 - seleciona camera com surface follow
            else if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                sfCam = new CameraSurfaceFollow(device);
                projection = sfCam.projection;
                view = sfCam.view;
            }
            //F3 - seleciona camera livre 
            else if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                freeCam = new CameraFree(device);
                projection = freeCam.projection;
                view = freeCam.view;
            }
        }
        


        public Matrix Projection
        {
            get { return projection; }
        }

        public Matrix View
        {
            get { return view; }
        }

    }
}
