using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SITAPI.Models;

namespace SITAPI.Controllers
{
    public class choicesController : baseController
    {
        private sitdbEntities db = new sitdbEntities();

        // GET: api/choices
        public IQueryable<choice> Getchoices()
        {
            return db.choices;
        }

        // GET: api/choices/5
        [ResponseType(typeof(choice))]
        public IHttpActionResult Getchoice(int id)
        {
            choice choice = db.choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            return Ok(choice);
        }

        // PUT: api/choices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putchoice(int id, choice choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != choice.sn)
            {
                return BadRequest();
            }

            db.Entry(choice).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!choiceExists(id))
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

        // POST: api/choices
        [ResponseType(typeof(choice))]
        public IHttpActionResult Postchoice(choice choice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.choices.Add(choice);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = choice.sn }, choice);
        }

        // DELETE: api/choices/5
        [ResponseType(typeof(choice))]
        public IHttpActionResult Deletechoice(int id)
        {
            choice choice = db.choices.Find(id);
            if (choice == null)
            {
                return NotFound();
            }

            db.choices.Remove(choice);
            db.SaveChanges();

            return Ok(choice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool choiceExists(int id)
        {
            return db.choices.Count(e => e.sn == id) > 0;
        }
    }
}