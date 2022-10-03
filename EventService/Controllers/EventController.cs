using EventService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace EventService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        /// <summary>
        /// Kaldes af en anden service, når den har brug for at offentliggøre (publishe) et event
        /// </summary>
        /// <param name="e">Information om den event, der er opstået</param>
        [HttpPost]
        public void RaiseEvent(Event e) {
            /// TODO: Skriv din kode her - husk også at implementere event-klassen
        }

        /// <summary>
        /// Henter events
        /// </summary>
        /// <param name="startIndex">Index på det første event der skal hentes</param>
        /// <param name="antal">Antallet af events der maksimalt skal hentes (der kan være færre)</param>
        /// <returns></returns>
        [HttpGet]
        public List<Event> ListEvents(int startIndex, int antal)
        {
            // TODO Skriv din kode her. Du må gerne ændre returtype og parametre, hvis du vil. Koden her er bare tænkt som et udgangspunkt.

            return new();
        }
    }
}
