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
        }

        public void Select(GraphicsDevice device)
        {
            //F1 - seleciona camera 3° pessoa
            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                tpCam = new CameraThirdPerson(device, position);
            }
            //F2 - seleciona camera com surface follow
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                sfCam = new CameraSurfaceFollow(device);
            }
            //F3 - seleciona camera livre 
            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                freeCam = new CameraFree(device);
            }
        }

        public void Update()
        {

        }

    }
}
