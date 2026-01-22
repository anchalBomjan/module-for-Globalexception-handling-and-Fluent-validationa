
using FluentValidation;

namespace demo.Validators
{
    public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
    {
        public WeatherForecastValidator()
        {
            RuleFor(x => x.Summary).NotEmpty().WithMessage("Summary is required.");
            RuleFor(x => x.TemperatureC).InclusiveBetween(-100, 100).WithMessage("Temperature must be between -100 and 100.");
        }
    }
}
