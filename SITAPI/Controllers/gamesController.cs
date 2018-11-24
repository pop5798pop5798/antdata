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
using System.Web.Configuration;
using Dapper;
using System.Data.SqlClient;
using SITAPI.Models.Repository;
using AutoMapper;
using SITDto.Request;
using SITAPI.Filter;

namespace SITAPI.Controllers
{
    [LogParamFilter]
    public class gamesController : baseController
    {
        private sitdbEntities db = new sitdbEntities();

        // GET: api/games
        public List<gameDto> Getgames()
        {
            return new GameRepository().GetGameList();
        }

        // GET: api/games/5
        [ResponseType(typeof(gameDto))]
        public IHttpActionResult Getgame(int id)
        {
            gameDto gamedto = new GameRepository().GetGame(id);
            if (gamedto.sn == 0)
                return NotFound();

            return Ok(gamedto);
        }

        // PUT: api/games/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putgame(int id, gameDto game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != game.sn)
            {
                return BadRequest();
            }
            bool isOK = new GameRepository().SetGame(id, game);
            if (!isOK)
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/games
        [ResponseType(typeof(api))]
        public IHttpActionResult Postgame(gameDto gameD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user u = db.users.Where(p => p.userID == gameD.userId && p.comSn == gameD.comSn).FirstOrDefault();
            if (u == null)
            {
                return BadRequest("UserID不存在");
            }
            else
            {
                gameD.userSn = u.sn;
            }

            Mapper.Initialize(cfg => {
                cfg.CreateMap<gameDto, api>();
                cfg.CreateMap<topicDto, topic>();
                cfg.CreateMap<choiceDto, choice>();
            });
            api game = Mapper.Map<api>(gameD);

            game.apiStatus = 1;      //遊戲建立的時候是關閉狀態
            game.valid = 1;
            db.apis.Add(game);
            db.SaveChanges();
            foreach(topicDto tD in gameD.topicList)
            {
                if (tD.valid.HasValue && tD.valid.Value == 1)
                {
                    topic t = Mapper.Map<topic>(tD);
                    t.apiSn = game.sn;
                   
                    db.topics.Add(t);
                    db.SaveChanges();
                    foreach (choiceDto cD in tD.choiceList)
                    {
                        if (cD.valid.HasValue && cD.valid.Value == 1)
                        {
                            choice c = Mapper.Map<choice>(cD);
                            c.topicSn = t.sn;
                            db.choices.Add(c);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return CreatedAtRoute("DefaultApi", new { id = game.sn }, game);
        }

        [Route("api/StartBet")]
        [HttpPost]
        public IHttpActionResult StartBet(StartBetReq sbr)
        {

            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus != 1)
                return BadRequest("此遊戲無法開始");
            gr.setGameCloseStatus(sbr.gameSn, 0);
            return Ok();
        }
        [Route("api/setClose")]
        [HttpPost]
        public IHttpActionResult setClose(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus != 0)
                return BadRequest("此遊戲無法關閉");
            gr.setGameCloseStatus(sbr.gameSn, 2);
            return Ok();
        }

        [Route("api/deleteClose")]
        [HttpPost]
        public IHttpActionResult deleteClose(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            gr.setGameCloseStatus(sbr.gameSn, 5);
            return Ok();
        }

        [Route("api/reopen")]
        [HttpPost]
        public IHttpActionResult reopen(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus == 3)
                return BadRequest("已完成派彩，無法重新開啟");
            new GameRepository().setGameCloseStatus(sbr.gameSn, 1);
            return Ok();
        }

        [Route("api/setDone")]
        [HttpPost]
        public IHttpActionResult setDone(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus == 3)
            {
                return Content(HttpStatusCode.BadRequest, "已完成派彩，無法重新派彩");
                //return BadRequest("已完成派彩，無法重新派彩");
            }
            new GameRepository().setGameCloseStatus(sbr.gameSn, 3);
            return Ok();
        }

        [Route("api/setWinner")]
        [HttpPost]
        public IHttpActionResult setWinner(SetWinnerReq swq)
        {
            user u = db.users.Where(p => p.userID == swq.UserID && p.comSn == swq.comSn).FirstOrDefault();
            if (u == null)
            {
                return BadRequest("UserID不存在");
            }
            GameRepository gr = new GameRepository();
            gr.setWinner(swq.choiceList);
            gr.setGameCloseStatus(swq.gameSn, 4);
            return Ok();
        }

        [Route("api/pays")]
        [HttpPost]
        public IHttpActionResult pays(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus != 4)
                return BadRequest("此遊戲未設定結果無法派彩");
            List<payoutDto> payoutList = gr.pays(sbr.gameSn);
            return Ok(payoutList);
        }

        [Route("api/paysRollback")]
        [HttpPost]
        public IHttpActionResult paysRollback(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus != 3)
                return BadRequest("此遊戲尚未派彩無法退回");
            List<payoutDto> payoutList = gr.paysRollback(sbr.gameSn);
            gr.setGameCloseStatus(sbr.gameSn, 2);
            return Ok(payoutList);
        }

        [Route("api/gamereturn")]
        [HttpPost]
        public IHttpActionResult gamereturn(StartBetReq sbr)
        {
            GameRepository gr = new GameRepository();
            byte? gameclosestatus = gr.getGameCloseStatus(sbr.gameSn);
            if (gameclosestatus != 4)
                return BadRequest("此遊戲無法重設");
            gr.setGameCloseStatus(sbr.gameSn, 2);
            return Ok();
        }

        // DELETE: api/games/5
        //[ResponseType(typeof(game))]
        //public IHttpActionResult Deletegame(int id)
        //{
        //    game game = db.games.Find(id);
        //    if (game == null)
        //    {
        //        return NotFound();
        //    }

        //    db.games.Remove(game);
        //    db.SaveChanges();

        //    return Ok(game);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool gameExists(int id)
        {
            return db.apis.Count(e => e.sn == id) > 0;
        }
    }
}