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
    public class payoutsController : baseController
    {
        private sitdbEntities db = new sitdbEntities();

        // GET: api/payouts
        public IQueryable<payout> Getpayouts()
        {
            return db.payouts;
        }

        // GET: api/payouts/5
        [ResponseType(typeof(payout))]
        public IHttpActionResult Getpayout(int id)
        {
            payout payout = db.payouts.Find(id);
            if (payout == null)
            {
                return NotFound();
            }

            return Ok(payout);
        }

        // PUT: api/payouts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpayout(int id, payout payout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payout.sn)
            {
                return BadRequest();
            }

            db.Entry(payout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!payoutExists(id))
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

        // POST: api/payouts
        [ResponseType(typeof(payout))]
        public IHttpActionResult Postpayout(payout payout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.payouts.Add(payout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = payout.sn }, payout);
        }

        // DELETE: api/payouts/5
        [ResponseType(typeof(payout))]
        public IHttpActionResult Deletepayout(int id)
        {
            payout payout = db.payouts.Find(id);
            if (payout == null)
            {
                return NotFound();
            }

            db.payouts.Remove(payout);
            db.SaveChanges();

            return Ok(payout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool payoutExists(int id)
        {
            return db.payouts.Count(e => e.sn == id) > 0;
        }
    }
}