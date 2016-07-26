using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchInfo.Data
{
    public class TwitchDataRepo
    {
        private string _connectionString;

        public TwitchDataRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddViewer(Viewer viewer)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                context.Viewers.InsertOnSubmit(viewer);
                context.SubmitChanges();
            }
        }

        public void AddStreamer(Streamer streamer)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                context.Streamers.InsertOnSubmit(streamer);
                context.SubmitChanges();
            }
        }
        public Viewer GetViewerById(int id)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                return context.Viewers.FirstOrDefault(v => v.Id == id);
            }
        }
        public Streamer GetStreamerById(int id)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                return context.Streamers.FirstOrDefault(s => s.Id == id);
            }
        }
        public Viewer GetViewerByIdWithStreamers(int? id)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Viewer>(v => v.Streamers);
                context.LoadOptions = loadOptions;
                return context.Viewers.FirstOrDefault(v => v.Id == id);
            }
        }
        public IEnumerable<Viewer> GetAllViewers()
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                context.DeferredLoadingEnabled = false;
                return context.Viewers.ToList();
            }
        }
        public void DeleteStreamer(int streamerId, int viewerId)
        {
            using (var context = new TwitchViewerAndStreamerDbDataContext())
            {
                context.ExecuteCommand("DELETE FROM Streamers WHERE Id = {0} AND ViewerId = {1}", streamerId, viewerId);
            }
        }
    }
}
