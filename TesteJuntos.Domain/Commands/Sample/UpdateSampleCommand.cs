using System;
using System.Collections.Generic;
using System.Text;

namespace TesteJuntos.Domain.Commands.Sample
{
    public class UpdateSampleCommand
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}
