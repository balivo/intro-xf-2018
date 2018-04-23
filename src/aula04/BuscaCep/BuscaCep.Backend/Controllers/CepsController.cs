using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BuscaCep.Backend.Models;

namespace BuscaCep.Backend.Controllers
{
    [Authorize]
    public class CepsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Ceps
        public IQueryable<CepDto> GetCeps()
        {
            return db.Ceps;
        }

        // GET: api/Ceps/5
        [ResponseType(typeof(CepDto))]
        public async Task<IHttpActionResult> GetCepDto(Guid id)
        {
            CepDto cepDto = await db.Ceps.FindAsync(id);
            if (cepDto == null)
            {
                return NotFound();
            }

            return Ok(cepDto);
        }

        // PUT: api/Ceps/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCepDto(Guid id, CepDto cepDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cepDto.Id)
            {
                return BadRequest();
            }

            db.Entry(cepDto).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CepDtoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Ceps
        [ResponseType(typeof(CepDto))]
        public async Task<IHttpActionResult> PostCepDto(CepDto cepDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ceps.Add(cepDto);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CepDtoExists(cepDto.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cepDto.Id }, cepDto);
        }

        // DELETE: api/Ceps/5
        [ResponseType(typeof(CepDto))]
        public async Task<IHttpActionResult> DeleteCepDto(Guid id)
        {
            CepDto cepDto = await db.Ceps.FindAsync(id);
            if (cepDto == null)
            {
                return NotFound();
            }

            db.Ceps.Remove(cepDto);
            await db.SaveChangesAsync();

            return Ok(cepDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CepDtoExists(Guid id)
        {
            return db.Ceps.Count(e => e.Id == id) > 0;
        }
    }
}