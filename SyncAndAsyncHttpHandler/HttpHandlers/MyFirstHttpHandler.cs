using SyncAndAsyncHttpHandler.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyncAndAsyncHttpHandler.HttpHandlers
{
    public class MyFirstHttpHandler: HttpSyncHandler
    {
        protected override void Post(HttpContext context)
        {
            var receivedContent = "Server received(Sync):" + GetReceivedContent(context);

            OK(context.Response, receivedContent);
        }
    }
}