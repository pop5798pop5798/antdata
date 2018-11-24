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
using SITDto.Request;
using SITAPI.Models.Repository;

namespace SITAPI.Controllers
{
    public class topicsController : baseController
    {
        private sitdbEntities db = new sitdbEntities();

        // GET: api/topics
        public IQueryable<topic> Gettopics()
        {
            return db.topics;
        }

        // GET: api/topics/5
        [ResponseType(typeof(topic))]
        public IHttpActionResult Gettopic(int id)
        {
            topic topic = db.topics.Find(id);
            if (topic == null)
            {
                return NotFound();
            }

            return Ok(topic);
        }

        // PUT: api/topics/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttopic(int id, topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topic.sn)
            {
                return BadRequest();
            }

            db.Entry(topic).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!topicExists(id))
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

        // POST: api/topics
        [ResponseType(typeof(topic))]
        public IHttpActionResult Posttopic(topic topic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.topics.Add(topic);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = topic.sn }, topic);
        }


        [Route("api/stopTopic")]
        [HttpPost]
        public IHttpActionResult stopTopic(StopTopicReq str)
        {
            TopicRepository tr = new TopicRepository();
            topic t = tr.setStop(str.topicSn);
            return Ok(t);
        }


        [Route("api/reopenTopic")]
        [HttpPost]
        public IHttpActionResult reopenTopic(StopTopicReq str)
        {
            TopicRepository tr = new TopicRepository();
            topic t = tr.setReopen(str.topicSn);
            return Ok(t);
        }

        // DELETE: api/topics/5
        [ResponseType(typeof(topic))]
        public IHttpActionResult Deletetopic(int id)
        {
            topic topic = db.topics.Find(id);
            if (topic == null)
            {
                return NotFound();
            }

            db.topics.Remove(topic);
            db.SaveChanges();

            return Ok(topic);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool topicExists(int id)
        {
            return db.topics.Count(e => e.sn == id) > 0;
        }
    }
}