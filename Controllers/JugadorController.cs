using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiJuegoPsp.Models;

namespace ApiJuegoPsp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JugadorController : ControllerBase
    {
        private readonly Context _context;

        public JugadorController(Context context)
        {
            _context = context;
        }

        // GET: api/Jugador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jugador>>> GetJugadores()
        {
            return await _context.Jugadores.ToListAsync();
        }

        // GET: api/Jugador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jugador>> GetJugador(long id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);

            if (jugador == null)
            {
                return NotFound();
            }

            return jugador;
        }

        // PUT: api/Jugador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJugador(long id, Jugador jugador)
        {
            if (id != jugador.Id)
            {
                return BadRequest();
            }

            _context.Entry(jugador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JugadorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jugador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Jugador>> PostJugador(Jugador jugador)
        {

            if (jugador.Coins < 0 || jugador.Points < 0 || string.IsNullOrEmpty(jugador.Name))
            {
                return BadRequest();
            }

            _context.Jugadores.Add(jugador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetJugador), new { id = jugador.Id }, jugador);
        }

        // DELETE: api/Jugador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugador(long id)
        {
            var jugador = await _context.Jugadores.FindAsync(id);
            if (jugador == null)
            {
                return NotFound();
            }

            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JugadorExists(long id)
        {
            return _context.Jugadores.Any(e => e.Id == id);
        }
    }
}
