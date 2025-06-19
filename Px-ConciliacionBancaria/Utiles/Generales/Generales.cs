using Px_ConciliacionBancaria.Utiles.Modelos;
using Px_Utiles.Models.Sistemas.ConciliacionBancaria.Catalogos;
using Px_Utiles.Models.Sistemas.ConciliacionBancaria.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Px_ConciliacionBancaria.Utiles.Generales
{
    public static class Generales
    {
        public static AppState _AppState { get; set; } = new AppState();
    }
}
