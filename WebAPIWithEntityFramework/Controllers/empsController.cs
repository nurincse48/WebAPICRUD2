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
using WebAPIWithEntityFramework.Models;

namespace WebAPIWithEntityFramework.Controllers
{
    public class empsController : ApiController
    {
        private Demo2WebAppDBEntities db = new Demo2WebAppDBEntities();

        // GET: api/emps
        public IQueryable<emp> Getemps()
        {
            IQueryable<emp> emps = db.emps;
            return emps;
        }

        // GET: api/emps/5
        [ResponseType(typeof(emp))]
        public async Task<IHttpActionResult> Getemp(int id)
        {
            emp emp = await db.emps.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            return Ok(emp);
        }

        // PUT: api/emps/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putemp(int id, emp emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emp.id)
            {
                return BadRequest();
            }

            db.Entry(emp).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!empExists(id))
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

        // POST: api/emps
        [ResponseType(typeof(emp))]
        public async Task<IHttpActionResult> Postemp(emp emp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.emps.Add(emp);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emp.id }, emp);
        }

        // DELETE: api/emps/5
        [ResponseType(typeof(emp))]
        public async Task<IHttpActionResult> Deleteemp(int id)
        {
            emp emp = await db.emps.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            db.emps.Remove(emp);
            await db.SaveChangesAsync();

            return Ok(emp);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool empExists(int id)
        {
            return db.emps.Count(e => e.id == id) > 0;
        }
    }
}