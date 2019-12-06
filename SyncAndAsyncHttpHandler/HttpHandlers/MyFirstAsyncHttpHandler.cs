using SyncAndAsyncHttpHandler.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SyncAndAsyncHttpHandler.HttpHandlers
{
    public class MyFirstAsyncHttpHandler : HttpAsyncHandler
    {
        protected override void Post(HttpContext context)
        {
            var receivedContent = "Server received(Async):" + GetReceivedContent(context);
            Thread.Sleep(1000 * 10);
            OK(context.Response, receivedContent);
        }
    }
}