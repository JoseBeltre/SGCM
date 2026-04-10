using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SGCM.Desktop.Utils
{
    /// <summary>
    /// Clase de utilidades para validación de datos en los formularios.
    /// Principio: toda validación ocurre ANTES de llamar al Service/API.
    /// </summary>
    public static class Validators
    {
        /// <summary>
        /// Acumula errores de validación. Retorna true si no hay errores.
        /// </summary>
        public static bool Validate(Action<ValidationBuilder> configure, out string errorMessage)
        {
            var builder = new ValidationBuilder();
            configure(builder);
            errorMessage = builder.GetErrors();
            return builder.IsValid;
        }
    }

    public class ValidationBuilder
    {
        private readonly List<string> _errors = new();
        public bool IsValid => _errors.Count == 0;

        public string GetErrors() => string.Join("\n", _errors);

        /// <summary>El campo no puede estar vacío.</summary>
        public ValidationBuilder Required(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
                _errors.Add($"• El campo '{fieldName}' es obligatorio.");
            return this;
        }

        /// <summary>El valor debe ser un número entero positivo mayor a cero.</summary>
        public ValidationBuilder PositiveInteger(string value, string fieldName)
        {
            if (!int.TryParse(value, out int parsed) || parsed <= 0)
                _errors.Add($"• '{fieldName}' debe ser un número entero positivo.");
            return this;
        }

        /// <summary>El valor debe ser un número entero no negativo (incluye cero).</summary>
        public ValidationBuilder NonNegativeInteger(string value, string fieldName)
        {
            if (!int.TryParse(value, out int parsed) || parsed < 0)
                _errors.Add($"• '{fieldName}' debe ser un número válido (mayor o igual a cero).");
            return this;
        }

        /// <summary>Valida formato de email básico.</summary>
        public ValidationBuilder Email(string value, string fieldName)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(value))
                    _errors.Add($"• '{fieldName}' no tiene un formato de correo válido.");
            }
            return this;
        }

        /// <summary>La contraseña debe cumplir longitud mínima.</summary>
        public ValidationBuilder MinLength(string value, string fieldName, int minLength)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.Length < minLength)
                _errors.Add($"• '{fieldName}' debe tener al menos {minLength} caracteres.");
            return this;
        }

        /// <summary>La fecha de cita debe ser futura.</summary>
        public ValidationBuilder FutureDate(DateTime value, string fieldName)
        {
            if (value <= DateTime.Now)
                _errors.Add($"• '{fieldName}' debe ser una fecha y hora futura.");
            return this;
        }

        /// <summary>La hora de inicio debe ser anterior a la de fin.</summary>
        public ValidationBuilder TimeRange(TimeSpan start, TimeSpan end, string fieldName)
        {
            if (start >= end)
                _errors.Add($"• '{fieldName}': La hora de inicio debe ser anterior a la hora de fin.");
            return this;
        }

        /// <summary>El campo debe tener un ítem seleccionado (ComboBox no vacío).</summary>
        public ValidationBuilder SelectionRequired(object? selectedItem, string fieldName)
        {
            if (selectedItem == null)
                _errors.Add($"• Debe seleccionar un valor para '{fieldName}'.");
            return this;
        }

        /// <summary>Aplica una condición personalizada con mensaje.</summary>
        public ValidationBuilder Custom(bool isValid, string errorMessage)
        {
            if (!isValid)
                _errors.Add($"• {errorMessage}");
            return this;
        }
    }
}
