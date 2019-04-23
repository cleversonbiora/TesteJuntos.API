using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteJuntos.Service.Services
{
    public class BaseService
    {
        internal void Validate<T>(T obj, AbstractValidator<T> validator)
        {
            if (obj == null)
                throw new Exception("Objeto Inv�lido!");

            validator.ValidateAndThrow(obj);
        }
    }
}
