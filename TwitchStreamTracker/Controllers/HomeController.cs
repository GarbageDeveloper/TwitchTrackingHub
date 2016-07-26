using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitchInfo.Data;

namespace TwitchStreamTracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddViewer(Viewer viewer)
        {
            var twitchManager = new TwitchDataRepo(Properties.Settings.Default.ConStr);
            twitchManager.AddViewer(viewer);
            return RedirectToAction("index");
        }
        [HttpPost]
        public ActionResult AddStreamer(Streamer streamer)
        {
            var twitchManager = new TwitchDataRepo(Properties.Settings.Default.ConStr);
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            twitchManager.AddStreamer(streamer);
            Viewer viewerName = twitchManager.GetViewerById(streamer.ViewerId);
            string streamerName = twitchManager.GetStreamerById(streamer.Id).StreamerName;
            context.Clients.All.StreamerAdded(new
            {
                ViewerFirstName = viewerName.FirstName,
                ViewerLastName = viewerName.LastName,
                StreamerName = streamerName
            });
            return RedirectToAction("StreamersByViewerId", new { id = streamer.ViewerId });
        }
        public ActionResult GetViewers()
        {
            var twitchManager = new TwitchDataRepo(Properties.Settings.Default.ConStr);
            return Json(new { Viewers = twitchManager.GetAllViewers() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StreamersByViewerId(int? id)
        {
            if (id == null)
            {
                return View("index");
            }
            var twitchManager = new TwitchDataRepo(Properties.Settings.Default.ConStr);
            return View(twitchManager.GetViewerByIdWithStreamers(id.HasValue ? id : null));//at this point the ternary not needed just randomly leaving it in:)
        }
        [HttpPost]
        public void DeleteStreamer(int streamerId, int viewerId)
        {
            var twitchManager = new TwitchDataRepo(Properties.Settings.Default.ConStr);
            var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            Viewer viewerName = twitchManager.GetViewerById(viewerId);
            string streamerName = twitchManager.GetStreamerById(streamerId).StreamerName;
            twitchManager.DeleteStreamer(streamerId, viewerId);
            context.Clients.All.StreamerDeleted(new
            {
                ViewerFirstName = viewerName.FirstName,
                ViewerLastName = viewerName.LastName,
                StreamerName = streamerName
            });

        }

    }
}
