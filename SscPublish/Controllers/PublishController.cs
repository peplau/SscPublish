using System;
using System.Web.Http;
using Sitecore.Data;
using Sitecore.Publishing;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using SscPublish.Filters;

namespace SscPublish.Controllers
{
    [ServicesController("PublishServices/PublishController")]
    public class PublishController : ServicesApiController
    {
        /// <summary>
        /// Publishes an item
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="deep">Deep publish</param>
        /// <param name="targetDb">Target DB</param>
        /// <param name="mode">Publish Mode</param>
        [ActionName("Publish")]
        [AcceptVerbs("GET")]
        [HttpGet]
        [AuthorizedRole(@"sitecore\Sitecore Client Publishing")]
        public bool Publish(string id, bool deep = false, string targetDb="web", string mode = "SingleItem")
        {
            try
            {
                var source = Database.GetDatabase("master");
                var target = Database.GetDatabase(targetDb);
                if (source == null || target == null)
                    return false;
                var rootItem = source.GetItem(id);
                if (rootItem == null)
                    return false;
                if (!Enum.TryParse(mode, out PublishMode publishMode))
                    publishMode = PublishMode.SingleItem;
                var po = new PublishOptions(source, target, publishMode,
                    Sitecore.Context.Language, DateTime.Now) {RootItem = rootItem, Deep = deep};
                new Publisher(po).Publish();
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception publishing items: " + ex, this);
                return false;
            }
        }
    }
}