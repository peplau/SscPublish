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
        /// <param name="async">Async publish</param>
        /// <param name="targetDb">Target DB</param>
        /// <param name="mode">Publish Mode</param>
        [ActionName("DefaultAction")]
        [AcceptVerbs("GET")]
        [HttpGet]
        [AuthorizedRole(@"sitecore\Sitecore Client Publishing")]
        public string DefaultAction(string id, bool deep = false, bool async = false, string targetDb = "web", string mode = "SingleItem")
        {
            return Publish(id, deep, async, targetDb);
        }

        /// <summary>
        /// Publishes an item
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="deep">Deep publish</param>
        /// <param name="async">Async publish</param>
        /// <param name="targetDb">Target DB</param>
        /// <param name="mode">Publish Mode</param>
        [ActionName("Publish")]
        [AcceptVerbs("GET")]
        [HttpGet]
        [AuthorizedRole(@"sitecore\Sitecore Client Publishing")]
        public string Publish(string id, bool deep = false, bool async = false, string targetDb = "web", string mode = "SingleItem")
        {
            try
            {
                var source = Database.GetDatabase("master");
                var target = Database.GetDatabase(targetDb);
                if (source == null || target == null)
                    return "Source or target database not found";
                var rootItem = source.GetItem(id);
                if (rootItem == null)
                    return "Root item not found";
                if (!Enum.TryParse(mode, out PublishMode publishMode))
                    publishMode = PublishMode.SingleItem;
                var po = new PublishOptions(source, target, publishMode,
                    Sitecore.Context.Language, DateTime.Now)
                { RootItem = rootItem, Deep = deep };

                if (async)
                {
                    new Publisher(po).PublishAsync();
                    return "Async publish successfully triggered";
                }

                var result = new Publisher(po).PublishWithResult();
                return result.ToString();
            }
            catch (Exception ex)
            {
                var msg = "Exception publishing items: " + ex;
                Sitecore.Diagnostics.Log.Error(msg, this);
                return msg;
            }
        }
    }
}