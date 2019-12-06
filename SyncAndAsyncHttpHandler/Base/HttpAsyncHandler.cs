using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;

namespace SyncAndAsyncHttpHandler.Base
{
    public delegate void AsyncProcessorDelegate(HttpContext context);

    /// <summary>
    /// 异步接口：再补充
    /// https://msdn.microsoft.com/en-us/library/ms227433(v=vs.100).aspx
    /// </summary>
    public class HttpAsyncHandler : IHttpAsyncHandler
    {
        public HttpAsyncHandler()
        {
            this.Stopwatch = new Stopwatch();
        }

        protected AsyncProcessorDelegate _Delegate;

        public bool IsReusable { get { return false; } }

        protected HttpRequest Request { get; set; }

        protected HttpResponse Response { get; set; }

        protected Stopwatch Stopwatch { get; set; }

        public virtual IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            Logging.Write.Info("Async call: BeginProcessRequest(), threadid=" + Thread.CurrentThread.ManagedThreadId);
            _Delegate = new AsyncProcessorDelegate(ProcessRequest);
            return _Delegate.BeginInvoke(context, cb, extraData);
        }

        public virtual void EndProcessRequest(IAsyncResult result)
        {
            Logging.Write.Info("Async call: EndProcessRequest(), threadid=" + Thread.CurrentThread.ManagedThreadId);
            _Delegate.EndInvoke(result);
        }

        public virtual void ProcessRequest(HttpContext context)
        {
            try
            {
                Logging.Write.Info("Async call: ProcessRequest(), threadid=" + Thread.CurrentThread.ManagedThreadId);

                // Assign values.
                this.Request = context.Request;
                this.Response = context.Response;

                switch (context.Request.HttpMethod.ToUpper())
                {
                    case "GET":
                        if (BeforeGet(context) == false) break;
                        Get(context);
                        AfterGet(context);
                        break;
                    case "POST":
                        if (BeforePost(context) == false) break;
                        Post(context);
                        AfterPost(context);
                        break;
                    default:
                        throw new Exception("不支持的操作：" + context.Request.HttpMethod);
                }
            }
            catch (Exception ex)
            {
                BadRequest(this.Response, "内部错误，请联系相关人员。");
            }
        }

        /// <summary>
        /// GET
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Get(HttpContext context)
        {
            this.Response.Write(string.Format("服务'{0}'运行正常。", this.GetType().Name));
        }

        /// <summary>
        /// POST
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Post(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Before GET
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool BeforeGet(HttpContext context)
        {
            return true;
        }

        /// <summary>
        /// After GET
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual void AfterGet(HttpContext context)
        {
        }

        /// <summary>
        /// Before POST
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool BeforePost(HttpContext context)
        {
            this.Stopwatch.Restart();
            return true;
        }

        /// <summary>
        /// After POST
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual void AfterPost(HttpContext context)
        {
            this.Stopwatch.Stop();
        }

        /// <summary>
        /// OK : 200
        /// </summary>
        /// <param name="response"></param>
        /// <param name="msg"></param>
        protected virtual void OK(HttpResponse response, string msg)
        {
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Write(msg);
        }

        /// <summary>
        /// BadRequest : 400
        /// </summary>
        /// <param name="response"></param>
        /// <param name="msg"></param>
        protected virtual void BadRequest(HttpResponse response, string msg)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Write(msg);
        }

        /// <summary>
        /// InternalServerError : 500
        /// </summary>
        /// <param name="response"></param>
        /// <param name="msg"></param>
        protected virtual void InternalServerError(HttpResponse response, string msg)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.Write(msg);
        }

        /// <summary>
        /// 取得请求内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetReceivedContent(HttpContext context)
        {
            if (context.Request.HttpMethod.ToUpper() != "POST")
                return null;

            using (var sr = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                return sr.ReadToEnd();
            }
        }
    }
}