using System;

namespace a7D.PDV.EF.Models
{
    public partial class tbHorarioDelivery
    {
        public int DiaSemana { get; set; } // de Domingo a Sábado

        public TimeSpan Turno1Inicio { get; set; }
        public TimeSpan Turno1Fim { get; set; }

        public TimeSpan Turno2Inicio { get; set; }
        public TimeSpan Turno2Fim { get; set; }

        public TimeSpan Turno3Inicio { get; set; }
        public TimeSpan Turno3Fim { get; set; }
    }
}
