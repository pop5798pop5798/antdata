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
using SITDto;
using AutoMapper;
using SITDto.ViewModel;
using SITAPI.Models.Repository;
using SITDto.Request;

namespace SITAPI.Controllers
{
    public class betsController : baseController
    {
        private sitdbEntities db = new sitdbEntities();

        // GET: api/bets
        public IQueryable<bet> Getbets()
        {
            return db.bets;
        }

        // GET: api/bets/5
        [ResponseType(typeof(bet))]
        public IHttpActionResult Getbet(int id)
        {
            bet bet = db.bets.Find(id);
            if (bet == null)
            {
                return NotFound();
            }

            return Ok(bet);
        }

        // PUT: api/bets/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putbet(int id, bet bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bet.sn)
            {
                return BadRequest();
            }

            db.Entry(bet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!betExists(id))
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

        // POST: api/bets
        [ResponseType(typeof(bet))]
        public IHttpActionResult Postbet(betDto bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user u = db.users.Where(p => p.userID == bet.userId && p.comSn == bet.comSn).FirstOrDefault();
            if (u == null)
            {
                return BadRequest("UserID不存在");
            }
            bet.userSn = u.sn;
            bet.valid = 1;
            Mapper.Initialize(cfg => {
                cfg.CreateMap<betDto, bet>();
            });
            bet b = Mapper.Map<bet>(bet);

            choice cho = db.choices.Where(p => p.sn == b.choiceSn).FirstOrDefault();
            b.topicSn = cho.topicSn;

            db.bets.Add(b);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = bet.sn }, bet);
        }

        // DELETE: api/bets/5
        [ResponseType(typeof(bet))]
        public IHttpActionResult Deletebet(int id)
        {
            bet bet = db.bets.Find(id);
            if (bet == null)
            {
                return NotFound();
            }

            db.bets.Remove(bet);
            db.SaveChanges();

            return Ok(bet);
        }

        [HttpGet]
        [Route("api/BetsByUserID")]
        [ResponseType(typeof(List<BetListDto>))]
        public IHttpActionResult BetsByUserID(string userID)
        {
            return Ok(new BetRepository().getBetsByUserID(userID));
        }

        [HttpPost]
        [Route("api/BetsByGame")]
        [ResponseType(typeof(List<BetListDto>))]
        public IHttpActionResult BetsByGame(BetsByGameReq bbgr)
        {
            return Ok(new BetRepository().getBetsByGame(bbgr));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool betExists(int id)
        {
            return db.bets.Count(e => e.sn == id) > 0;
        }
    }
}