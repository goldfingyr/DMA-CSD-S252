using DMI2Grafana.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DMI2Grafana.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoordinatesController : ControllerBase
    {

        // GET: api/<CoordinatesController>
        [HttpGet]
        public IActionResult Get()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://opendataapi.dmi.dk/v2/metObs/collections/station/items");
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            // SLUT POSTMAN
            DMIStationItems myDeserializedClass = JsonConvert.DeserializeObject<DMIStationItems>(response.Content.ReadAsStringAsync().Result);
            if (response.IsSuccessStatusCode)
            {
                List<GrafanaSimpleGeos> grafanaSimpleGeosList = new List<GrafanaSimpleGeos>();
                foreach (var feature in myDeserializedClass.features)
                {
                    if (feature.geometry != null && feature.geometry.coordinates != null && feature.geometry.coordinates.Count >= 2)
                    {
                        grafanaSimpleGeosList.Add(new GrafanaSimpleGeos
                        {
                            name = feature.properties.name,
                            lattitude = (float)feature.geometry.coordinates[1],
                            longitude = (float)feature.geometry.coordinates[0]
                        });
                    }
                }
                return Ok(grafanaSimpleGeosList);
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET api/<CoordinatesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoordinatesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CoordinatesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoordinatesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
