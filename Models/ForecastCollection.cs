using System.Collections;

namespace TestWebApi.Models
{
    public class ForecastEnumerator : IEnumerator<WeatherForecast>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private WeatherForecast[] weatherCollection;
        private int index;

        public WeatherForecast Current => this.weatherCollection[index];

        public ForecastEnumerator()
        {
            this.index = 0;
            this.weatherCollection = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            this.weatherCollection = [];
        }

        public bool MoveNext()
        {
            if (this.index < this.weatherCollection.Length-1) {
                this.index++;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.index = 0;
        }
    }
    public class ForecastCollection : IEnumerable<WeatherForecast>
    {
        private IEnumerator<WeatherForecast> forecastEnumerator;

        public ForecastCollection()
        {
            this.forecastEnumerator = new ForecastEnumerator();
        }
        public IEnumerator<WeatherForecast> GetEnumerator()
        {
            return this.forecastEnumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
