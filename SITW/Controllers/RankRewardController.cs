using SITW.Models;
using SITW.Models.Interface;
using SITW.Models.Repository;
using SITW.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SITW.Controllers
{
    [Authorize(Roles = "Admin")]    
    public class RankRewardController : Controller
    {
        private IRankRewardRepository rewardRepository;
        private IRewardTitleRepository rewardtitleRepository;


        public RankRewardController()
        {
            this.rewardRepository = new RnakRewardRepository();
            this.rewardtitleRepository = new RewardTitleRepository();
        }

        //[AllowAnonymous]
        public ActionResult Index()
        {
            var reward = this.rewardRepository.GetAll().ToList();
            var rewardtitle = this.rewardtitleRepository.GetAll().ToList();
    
            RankRewardViewModel viewModel = new RankRewardViewModel
            {
                RewardData = reward,
                RewardTitle = rewardtitle
            };
          
            return View(viewModel);
        }
        [AllowAnonymous]
        public ActionResult _RankReward()
        {
            var reward = this.rewardRepository.GetAll().ToList();
            var rewardtitle = this.rewardtitleRepository.GetAll().ToList();
            RankRewardViewModel viewModel = new RankRewardViewModel
            {
                RewardData = reward,
                RewardTitle = rewardtitle
            };
            if(DateTime.Now >= DateTime.Parse("2018-05-01T00:00"))
            {
                ViewBag.time = 1;
            }
            

            return View(viewModel);
        }



        [ValidateInput(false)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Ranking_content reward)
        {
            if (reward != null && ModelState.IsValid)
            {
                this.rewardRepository.Create(reward);
                return RedirectToAction("Index");
            }
            else
            {
                return View(reward);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var reward = this.rewardRepository.Get(id.Value);
                return View(reward);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Ranking_content reward)
        {
            if (reward != null && ModelState.IsValid)
            {
                this.rewardRepository.Update(reward);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult EditTitle(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var reward = this.rewardtitleRepository.Get(id.Value);
                return View(reward);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditTitle(Ranking_title reward)
        {
            if (reward != null && ModelState.IsValid)
            {
                this.rewardtitleRepository.Update(reward);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }



        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var category = this.rewardRepository.Get(id.Value);
                return View(category);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = this.rewardRepository.Get(id);
                this.rewardRepository.Delete(category);
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id });
            }
            return RedirectToAction("Index");
        }

    }
}