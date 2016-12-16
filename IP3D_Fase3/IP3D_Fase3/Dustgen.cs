using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP3D_Fase3
{
    class Dustgen
    {
        BasicEffect effect;
        int particulas;
        float raio, comprimento, largura, altura;
        List<Particle> dust;
        GraphicsDevice d;
        CameraSurfaceFollow cam;
        Random rnd;

        public Dustgen(GraphicsDevice d,CameraSurfaceFollow cam)
        {

        }
    }
}
