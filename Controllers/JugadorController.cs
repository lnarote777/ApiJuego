using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiJuegoPsp.Models;

namespace ApiJuegoPsp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly IMongoCollection<Jugador> _jugadores;
        private readonly IMongoCollection<Contador> _contador;

        public JugadorController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("JuegoPsp");
            _jugadores = database.GetCollection<Jugador>("Jugadores");
            _contador = database.GetCollection<Contador>("Contador");
        }

        private async Task<long> ObtenerSiguienteId()
        {
            var filter = Builders<Contador>.Filter.Eq(c => c.Id, "jugador");
            var update = Builders<Contador>.Update.Inc(c => c.Secuencia, 1);
            var options = new FindOneAndUpdateOptions<Contador>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true // Si no existe, lo crea
            };

            var contador = await _contador.FindOneAndUpdateAsync(filter, update, options);
            return contador.Secuencia;
        }

        // GET: api/Jugador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jugador>>> GetJugadores()
        {
            var jugadores = await _jugadores.Find(j => true).ToListAsync();
            return Ok(jugadores);
        }

        // GET: api/Jugador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jugador>> GetJugador(long id)
        {
            var jugador = await _jugadores.Find(j => j.Id == id).FirstOrDefaultAsync();
            if (jugador == null)
            {
                return NotFound();
            }
            return Ok(jugador);
        }

        // PUT: api/Jugador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJugador(long id, Jugador jugador)
        {
            if (id != jugador.Id)
            {
                return BadRequest("El ID de la URL y del cuerpo no coinciden.");
            }

            var result = await _jugadores.ReplaceOneAsync(j => j.Id == id, jugador);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Jugador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Jugador>> PostJugador(Jugador jugador)
        {

            if (string.IsNullOrEmpty(jugador.Name) || jugador.Coins < 0 || jugador.Points < 0)
            {
                return BadRequest("Datos inválidos.");
            }

            jugador.Id = await ObtenerSiguienteId();
            jugador.UltimaConexion = DateTime.UtcNow;
            await _jugadores.InsertOneAsync(jugador);

            return CreatedAtAction(nameof(GetJugador), new { id = jugador.Id }, jugador);
        }

        // DELETE: api/Jugador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugador(long id)
        {
            var result = await _jugadores.DeleteOneAsync(j => j.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
