using TesteJuntos.Domain.Models;
using TesteJuntos.Domain.Commands.Sample;
using System;
using System.Collections.Generic;
using System.Text;
using TesteJuntos.Domain.ViewModel;

namespace TesteJuntos.Domain.Interfaces.Service
{
    public interface ISampleService
    {
        SampleViewModel Get(int id);
        int Post(InsertSampleCommand sample);
        bool Put(UpdateSampleCommand sample);
        bool Delete(int id);
    }
}
